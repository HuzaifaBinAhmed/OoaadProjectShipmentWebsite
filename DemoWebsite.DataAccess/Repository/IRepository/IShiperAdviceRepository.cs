using DemoWebsite.Models.Models;
using DemoWebsiteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.DataAccess.Repository.IRepository
{
    public interface IShiperAdviceRepository:IRepository<ShiperAdvice>
    {
        void Update(ShiperAdvice advice);

    }
}
