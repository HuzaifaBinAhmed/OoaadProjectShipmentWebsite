using DemoWebsiteApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Models.Models
{
    public class ShiperAdvice
    {
        [Key]
        public int Id { get; set; }


        public int shipmentId { get; set; }

        [ForeignKey("shipmentId")]
        public Shipment Shipment { get; set; }

        public string AdviceMessage { get; set; }
    }
}
