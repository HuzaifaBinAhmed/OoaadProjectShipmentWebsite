using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebsiteApi.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        public string ConsigneeName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int CODAmount { get; set; }

        public string Netweight { get; set; }

        public string SpecialInstructions { get; set; }

        public long TrackingNumber { get; set; }

        [ValidateNever]
        public string ShipmentStatus { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }


        public float CompanyCharges { get; set; }






    }
}
