using DemoWebsite.Models.ViewModels;
using DemoWebsiteApi.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemoWebsite.Models.Models;
using Microsoft.AspNetCore.Identity;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using Syncfusion.Pdf;
using DemoWebsiteApi.Utility;

namespace DemoWebsiteApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FinanceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public FinanceController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                var users = _db.Users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToList();

                var model = new UploadExcelViewModel
                {
                    Users = users
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("UserPdfs");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile excelFile, string applicationUserId)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a valid Excel file.");
                var users = _db.Users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToList();
                return View("Index", new UploadExcelViewModel { Users = users });
            }

            // Ensure the uploads directory exists
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var excelFilePath = Path.Combine(uploadPath, excelFile.FileName);

            using (var stream = new FileStream(excelFilePath, FileMode.Create))
            {
                await excelFile.CopyToAsync(stream);
            }

            // Convert Excel to PDF
            var pdfFilePath = ConvertExcelToPdf(excelFilePath);
            var pdfFileName = Path.GetFileName(pdfFilePath);
            var pdfFileRelativePath = $"/uploads/{pdfFileName}";

            // Save the PDF file path to the database
            var userPdf = new UserPdf
            {
                ApplicationUserId = applicationUserId,
                PdfFilePath = pdfFileRelativePath
            };
            _db.userPdfs.Add(userPdf);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private string ConvertExcelToPdf(string excelFilePath)
        {
            // Initialize the ExcelEngine and load the Excel file
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = application.Workbooks.Open(fileStream);

                    // Initialize the PDF renderer and settings
                    XlsIORendererSettings xlsIORendererSettings = new XlsIORendererSettings
                    {
                        LayoutOptions = LayoutOptions.FitSheetOnOnePage
                    };
                    XlsIORenderer renderer = new XlsIORenderer();

                    // Convert Excel to PDF using Syncfusion.Pdf.PdfDocument
                    PdfDocument pdfDocument = renderer.ConvertToPDF(workbook, xlsIORendererSettings);

                    // Generate PDF file path
                    string pdfFilePath = Path.ChangeExtension(excelFilePath, ".pdf");

                    // Save the PDF file
                    using (FileStream pdfStream = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
                    {
                        pdfDocument.Save(pdfStream);
                    }

                    return pdfFilePath;
                }
            }
        }

        public IActionResult UserPdfs()
        {
            var userId = _userManager.GetUserId(User); // Get the current user's ID
            var userPdfs = _db.userPdfs
                .Where(up => up.ApplicationUserId == userId)
                .ToList(); // Convert to a List or use AsEnumerable()

            return View(userPdfs); // Pass the List to the view
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetUser()
        {
            var userId = _userManager.GetUserId(User); // Get the current user's ID
            var userPdfs = _db.userPdfs
                .Where(up => up.ApplicationUserId == userId)
                .ToList(); // Convert to a List or use AsEnumerable()
            return Json(new { data = userPdfs });
        }
#endregion
    }
}
