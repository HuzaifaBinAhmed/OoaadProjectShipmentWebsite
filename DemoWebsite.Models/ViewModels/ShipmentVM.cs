using DemoWebsiteApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Models.ViewModels
{
    public class ShipmentVM
    {
        public Shipment Shipment { get; set; }

        [ValidateNever]
        public List<SelectListItem> UserList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ShipmentStatus { get; set; }


    }
}
