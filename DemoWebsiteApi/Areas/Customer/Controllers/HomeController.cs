using DemoWebsite.DataAccess.Repository.IRepository;
using DemoWebsite.Models.ViewModels;
using DemoWebsiteApi.DataAccess;
using DemoWebsiteApi.Models;
using DemoWebsiteApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DemoWebsiteApi.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Customer) || User.IsInRole(SD.Role_SalePerson))
            {
                var financeVM = new FinanceVM
                {
                    TotalShipmentBooked = _db.shipments.Where(s => s.ApplicationUserId == userId).Count(s => s.ShipmentStatus == SD.ShipmentBookedStatus),
                    BookedShipments = _db.shipments.Where(s => s.ShipmentStatus == SD.ShipmentBookedStatus).Where(s=>s.ApplicationUserId == userId).ToList(),

                    TotalShipmentArrivedAtOrigin = _db.shipments.Where(s => s.ApplicationUserId == userId).Count(s => s.ShipmentStatus == SD.ShipmentArrivedStatus),
                    ArrivedAtOriginShipments = _db.shipments.Where(s => s.ShipmentStatus == SD.ShipmentArrivedStatus).Where(s => s.ApplicationUserId == userId).ToList(),

                    TotalShipmentInTransit = _db.shipments.Where(s => s.ApplicationUserId == userId).Count(s => s.ShipmentStatus == "InTransit"),
                    InTransitShipments = _db.shipments.Where(s => s.ShipmentStatus == "InTransit").Where(s => s.ApplicationUserId == userId).ToList(),

                    TotalShipmentArrivedAtDeatination = _db.shipments.Where(s => s.ApplicationUserId == userId).Count(s => s.ShipmentStatus == "ArrivedAtDestination"),
                    ArrivedAtDestinationShipments = _db.shipments.Where(s => s.ShipmentStatus == "ArrivedAtDestination").Where(s => s.ApplicationUserId == userId).ToList(),

                    TotalShipmentOutForDelivery = _db.shipments.Where(s => s.ApplicationUserId == userId).Count(s => s.ShipmentStatus == SD.ShipmentDispatchStatus),
                    OutForDeliveryShipments = _db.shipments.Where(s => s.ShipmentStatus == SD.ShipmentDispatchStatus).Where(s => s.ApplicationUserId == userId).ToList(),

                    TotalShipmentDelivered = _db.shipments.Where(s=>s.ApplicationUserId == userId).Count(s => s.ShipmentStatus == SD.ShipmentDeliveredStatus),
                    DeliveredShipments = _db.shipments.Where(s => s.ShipmentStatus == SD.ShipmentDeliveredStatus).Where(s => s.ApplicationUserId == userId).ToList()
                };

                return View(financeVM);
            }
            else
            {
                return View();
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
