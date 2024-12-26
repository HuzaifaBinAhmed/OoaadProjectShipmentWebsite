using DemoWebsite.DataAccess.Repository.IRepository;
using DemoWebsite.Models.Models;
using DemoWebsiteApi.DataAccess;
using DemoWebsiteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.DataAccess.Repository
{
    public class ShiperAdviceRepository:Repository<ShiperAdvice>,IShiperAdviceRepository
    {
        private ApplicationDbContext _db;
        public ShiperAdviceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShiperAdvice s)
        {
            _db.shiperAdvices.Update(s);
        }
    }
}
