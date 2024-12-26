using DemoWebsite.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Models.ViewModels
{
    public class AdviceVM
    {
        [ValidateNever]
        public ShiperAdvice ShiperAdvice { get; set; }
        [ValidateNever]
        public List<SelectListItem> ShiperTrackingNumber { get; set; }
    }
}
