using DemoWebsite.DataAccess.Repository.IRepository;
using DemoWebsiteApi.DataAccess;
using DemoWebsiteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.DataAccess.Repository
{
    public class ShipmentRepository:Repository<Shipment>,IShipmentRepository
    {
        private ApplicationDbContext _db;
        public ShipmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Shipment s)
        {
            _db.shipments.Update(s);
        }
    }
}
