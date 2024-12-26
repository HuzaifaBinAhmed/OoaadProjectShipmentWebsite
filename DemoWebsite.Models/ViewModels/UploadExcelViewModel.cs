using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Models.ViewModels
{
    public  class UploadExcelViewModel
    {
        public string ApplicationuserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
