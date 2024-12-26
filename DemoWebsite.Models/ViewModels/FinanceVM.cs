using DemoWebsiteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Models.ViewModels
{
    public class FinanceVM
    {
        public int TotalShipmentBooked { get; set; }
        public List<Shipment> BookedShipments { get; set; } = new List<Shipment>();

        public int TotalShipmentArrivedAtOrigin { get; set; }
        public List<Shipment> ArrivedAtOriginShipments { get; set; } = new List<Shipment>();

        public int TotalShipmentInTransit { get; set; }
        public List<Shipment> InTransitShipments { get; set; } = new List<Shipment>();

        public int TotalShipmentArrivedAtDeatination { get; set; }
        public List<Shipment> ArrivedAtDestinationShipments { get; set; } = new List<Shipment>();

        public int TotalShipmentOutForDelivery { get; set; }
        public List<Shipment> OutForDeliveryShipments { get; set; } = new List<Shipment>();

        public int TotalShipmentDelivered { get; set; }
        public List<Shipment> DeliveredShipments { get; set; } = new List<Shipment>();
    }
}
