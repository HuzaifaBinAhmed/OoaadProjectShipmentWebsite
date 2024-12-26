using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Utility
{
    public class Helper
    {
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
