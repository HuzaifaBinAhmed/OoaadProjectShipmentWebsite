using DemoWebsite.DataAccess.Repository;
using DemoWebsite.DataAccess.Repository.IRepository;
using DemoWebsite.Models.Models;
using DemoWebsite.Models.ViewModels;
using DemoWebsiteApi.Models;
using DemoWebsiteApi.Utility;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Syncfusion.XlsIO;
using System.Security.Claims;

namespace DemoWebsiteApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShipmentController : Controller
    {
        private readonly IUnitOfWork _db;

        public ShipmentController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Shipment> shipments = _db.ShipmentRepository.GetAll(includeProperties: "ApplicationUser").ToList();
            return View(shipments);
        }

        private IEnumerable<SelectListItem> GetShipmentStatusList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Text = SD.ShipmentDropOffStatus, Value = SD.ShipmentDropOffStatus },
        new SelectListItem { Text = SD.ShipmentPickedStatus, Value = SD.ShipmentPickedStatus },
        new SelectListItem { Text = SD.ShipmentReturnStatus, Value = SD.ShipmentReturnStatus },
        new SelectListItem { Text = SD.ShipmentReadyReturnStatus, Value = SD.ShipmentReadyReturnStatus },
        new SelectListItem { Text = SD.ShipmentDispatchStatus, Value = SD.ShipmentDispatchStatus },
        new SelectListItem { Text = SD.ShipmentDeliveredStatus, Value = SD.ShipmentDeliveredStatus },
        new SelectListItem { Text = SD.ShipmentPendingdStatus, Value = SD.ShipmentPendingdStatus },
        new SelectListItem { Text = SD.ShipmentMissrouteStatus, Value = SD.ShipmentMissrouteStatus },
        new SelectListItem { Text = SD.ShipmentReturnShipperStatus, Value = SD.ShipmentReturnShipperStatus },
        new SelectListItem { Text = SD.ShipmentArrivedStatus, Value = SD.ShipmentArrivedStatus },
        new SelectListItem { Text = SD.ShipmentAssignCourierStatus, Value = SD.ShipmentAssignCourierStatus },
        new SelectListItem { Text = SD.ShipmentBookedStatus, Value = SD.ShipmentBookedStatus },
        new SelectListItem { Text = SD.ShipmentCancelledStatus, Value = SD.ShipmentCancelledStatus },
        new SelectListItem { Text = SD.ShipmentPickupRequestStatus, Value = SD.ShipmentPickupRequestStatus },
        new SelectListItem { Text = SD.ShipmentAutoCancelledStatus, Value = SD.ShipmentAutoCancelledStatus },
        new SelectListItem { Text = SD.ShipmentPickupRequestNotStatus, Value = SD.ShipmentPickupRequestNotStatus }
    };
        }

        public static float CalculateCharges(string netweight)
        {
            if (decimal.TryParse(netweight, out decimal weight))
            {
                if (weight > 0 && weight <= 1)
                {
                    return 10.0f; // Charge for 0-1kg
                }
                else if (weight > 1 && weight <= 2)
                {
                    return 20.0f; // Charge for 1-2kg
                }
                else if (weight > 2 && weight <= 3)
                {
                    return 30.0f; // Charge for 2-3kg
                }
                else if (weight > 3)
                {
                    return 50.0f; // Charge for >3kg
                }
            }
            return 0.0f; // Default charge if weight is invalid
        }

        public IActionResult Upsert(int? id)
        {
            ShipmentVM shipmentVM = new ShipmentVM()
            {
                UserList = _db.ApplicationUserRepository.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                ).ToList(),
                Shipment = new Shipment(),
                ShipmentStatus = GetShipmentStatusList()
            };

            if (id == null || id == 0)
            {
                // Create
                if (!User.IsInRole(SD.Role_Admin))
                {
                    // If the user is not an admin, get their ID
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    shipmentVM.Shipment.ApplicationUserId = userId;
                }

                return View(shipmentVM);
            }
            else
            {
                // Update
                shipmentVM.Shipment = _db.ShipmentRepository.Get(u => u.ShipmentId == id);

                if (!User.IsInRole(SD.Role_Admin))
                {
                    // If the user is not an admin, ensure they can only edit their own shipments
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (shipmentVM.Shipment.ApplicationUserId != userId)
                    {
                        // Redirect or show an error if they attempt to edit someone else's shipment
                        return RedirectToAction("Index");
                    }
                }

                return View(shipmentVM);
            }
        }

        public long GenerateConsignmentNumber()
        {
            System.Threading.Thread.Sleep(4);
            string dateTimePart = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // Includes milliseconds
            return long.Parse(dateTimePart);
        }

        public IActionResult ShiperAdvice()
        {
            var adviceList = _db.ShiperAdviceRepository.GetAll(includeProperties:"Shipment").ToList();
            return View(adviceList);
        }

        public IActionResult AddAdvice()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                AdviceVM adviceVM = new AdviceVM()
                {
                    ShiperTrackingNumber = _db.ShipmentRepository.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.TrackingNumber.ToString(),
                        Value = u.ShipmentId.ToString()
                    }).ToList(),
                    ShiperAdvice = new ShiperAdvice()
                };
                return View(adviceVM);
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                AdviceVM adviceVM = new AdviceVM()
                {
                    ShiperTrackingNumber = _db.ShipmentRepository.GetAll(filter:s=>s.ApplicationUserId == userId).Select(
                    u => new SelectListItem
                    {
                        Text = u.TrackingNumber.ToString(),
                        Value = u.ShipmentId.ToString()
                    }).ToList(),
                    ShiperAdvice = new ShiperAdvice()
                };
                return View(adviceVM);
            }
        }

        [HttpPost]
        public IActionResult AddAdvice(AdviceVM model)
        {
            if (ModelState.IsValid)
            {
                _db.ShiperAdviceRepository.Add(model.ShiperAdvice);
                _db.save(); // Ensure this is the correct save method

                return RedirectToAction("Index"); // Redirect to a suitable action
            }

            // If the model is not valid, reload the shipments for the dropdown
            model.ShiperTrackingNumber = _db.ShipmentRepository.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.ConsigneeName,
                Value = u.ShipmentId.ToString()
            }).ToList();
            
            return View(model);
        }


        [HttpPost]
        public IActionResult Upsert(ShipmentVM p)
        {
            if (ModelState.IsValid)
            {

                if (p.Shipment.ShipmentId == 0)
                {
                    //p.Shipment.CreatedDate = DateTime.Now;
                    p.Shipment.CompanyCharges = CalculateCharges(p.Shipment.Netweight);
                    p.Shipment.TrackingNumber = GenerateConsignmentNumber();
                    p.Shipment.ShipmentStatus = SD.ShipmentBookedStatus;
                    _db.ShipmentRepository.Add(p.Shipment);
                }
                else
                {
                    var shipment = _db.ShipmentRepository.Get(u => u.ShipmentId == p.Shipment.ShipmentId);
                    p.Shipment.TrackingNumber = shipment.TrackingNumber;
                    _db.ShipmentRepository.Update(p.Shipment);

                }
                _db.save();
                return RedirectToAction("Index");
            }
            else
            {
                p.UserList = _db.ShipmentRepository.GetAll().Select(
               u => new SelectListItem
               {
                   Text = u.ApplicationUser.Name,
                   Value = u.ApplicationUserId.ToString()
               }
               ).ToList();
                return View(p);
            }

        }

        public IActionResult BulkShipment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BulkShipment(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                // Retrieve the current logged-in user's ID
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Initialize the XlsIO engine
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;

                    // Open the Excel file as a stream
                    using (var stream = excelFile.OpenReadStream())
                    {
                        // Open the workbook
                        IWorkbook workbook = application.Workbooks.Open(stream);

                        // Access the first worksheet
                        IWorksheet worksheet = workbook.Worksheets[0];

                        // Get the number of rows in the worksheet
                        int rowCount = worksheet.Rows.Length;

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                        {
                            var consigneeNameText = worksheet[row, 1]?.Text?.Trim();
                            var phoneNumberText = worksheet[row, 2]?.Value?.Trim();//null value 
                            var addressText = worksheet[row, 3]?.Text?.Trim();
                            var cityText = worksheet[row, 4]?.Text?.Trim();
                            var codAmountText = worksheet[row, 5]?.Value;//null here
                            var netweightText = worksheet[row, 6]?.Value?.Trim();//null here
                            var specialInstructionsText = worksheet[row, 7]?.Text?.Trim();

                            var shipment = new Shipment
                            {
                                ConsigneeName = worksheet[row, 1].Text.Trim(),
                                PhoneNumber = worksheet[row, 2].Value.Trim(),
                                Address = worksheet[row, 3].Text.Trim(),
                                City = worksheet[row, 4].Text.Trim(),
                                CODAmount = !string.IsNullOrWhiteSpace(codAmountText) ? int.Parse(codAmountText) : 0, // Check for null or empty
                                Netweight = worksheet[row, 6].Value.Trim(),
                                SpecialInstructions = worksheet[row, 7].Text.Trim(),
                                ApplicationUserId = userId, // Assigning the current user ID
                                ShipmentStatus = SD.ShipmentBookedStatus, // Default status
                                TrackingNumber = GenerateConsignmentNumber()
                            };

                            // Add the shipment to the repository
                            _db.ShipmentRepository.Add(shipment);
                        }


                        // Save all the shipments to the database
                        _db.save();
                    }
                }
            }

            // Redirect to the Index action after processing the file
            return RedirectToAction("Index");
        }



        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var objFromDb = _db.ShipmentRepository.Get(u => u.ShipmentId == id);
                return View(objFromDb);
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ShipmentRepository.Get(u => u.ShipmentId == id);
            _db.ShipmentRepository.Remove(obj);
            _db.save();
            return RedirectToAction("Index");

        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                List<Shipment> shipments = _db.ShipmentRepository.GetAll(includeProperties: "ApplicationUser").ToList();
                return Json(new { data = shipments });
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                List<Shipment> shipments = _db.ShipmentRepository.GetAll(filter: s => s.ApplicationUserId == userId, includeProperties: "ApplicationUser").ToList();
                return Json(new { data = shipments });
            }

        }


        [HttpGet]
        public IActionResult GetshiperAdvice()
        {
            var adviceList = _db.ShiperAdviceRepository.GetAll(includeProperties: "Shipment").ToList();
            return Json(new { data = adviceList });
        }
        #endregion

    }
}
