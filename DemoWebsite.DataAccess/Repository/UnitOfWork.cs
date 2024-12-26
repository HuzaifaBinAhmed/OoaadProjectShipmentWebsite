using DemoWebsite.DataAccess.Repository.IRepository;
using DemoWebsiteApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db;

        public IApplicationUserRepository ApplicationUserRepository { get; private set; }

        public IShipmentRepository ShipmentRepository { get; private set; }

        public IShiperAdviceRepository ShiperAdviceRepository { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ShipmentRepository = new ShipmentRepository(_db);
            ApplicationUserRepository = new ApplicationUserRepository(_db);
            ShiperAdviceRepository = new ShiperAdviceRepository(_db);
        }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
