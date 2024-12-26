using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUserRepository { get; }

        IShipmentRepository ShipmentRepository { get; }

        IShiperAdviceRepository ShiperAdviceRepository { get; }

        void save();


    }
}
