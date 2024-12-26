using DemoWebsiteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.DataAccess.Repository.IRepository
{
    public interface IShipmentRepository:IRepository<Shipment>
    {
        void Update(Shipment shipment);
    }
}
