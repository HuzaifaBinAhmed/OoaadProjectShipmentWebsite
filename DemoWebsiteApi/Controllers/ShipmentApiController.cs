using DemoWebsiteApi.DataAccess;
using DemoWebsiteApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebsiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentApiController : ControllerBase
    {
        public readonly ApplicationDbContext _db;
        public ShipmentApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("AllShipments")]
        public async Task<IEnumerable<Shipment>> GetAllShipments()
        {
            var shipments = await _db.shipments.Include("ApplicationUser").ToListAsync();
            return shipments;
        }

        [HttpPost("CreateShipment")]
        public async Task<ActionResult<Shipment>> CreateShipment([FromBody] Shipment shipmentObj)
        {
            if (shipmentObj == null)
            {
                return BadRequest();
            }

            var user = await _db.Set<ApplicationUser>().FindAsync(shipmentObj.ApplicationUserId);
            if (user == null)
            {
                return BadRequest();
            }

            shipmentObj.ApplicationUser = user;
            await _db.shipments.AddAsync(shipmentObj);
            await _db.SaveChangesAsync();
            return Ok(shipmentObj);
        }

        [HttpPut("id")]
        public async Task<ActionResult<Shipment>> UpdateShipment(int id, [FromBody] Shipment ship)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var obj = await _db.shipments.FindAsync(id);
            var user = await _db.Set<ApplicationUser>().FindAsync(ship.ApplicationUserId);
            if (user == null)
            {
                return BadRequest();
            }
            obj.ConsigneeName = ship.ConsigneeName;
            obj.PhoneNumber = ship.PhoneNumber;
            obj.Address = ship.Address;
            obj.Netweight = ship.Netweight;
            obj.CODAmount = ship.CODAmount;
            obj.City = ship.City;
            obj.SpecialInstructions = ship.SpecialInstructions;
            obj.ApplicationUser = user;

            _db.shipments.Update(obj);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Shipment>> RemoveShipment(int id)
        {
            var obj = await _db.shipments.FindAsync(id);
            _db.shipments.Remove(obj);
            await _db.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("Count")]
        public async Task<ActionResult<int>> Count()
        {
            return await _db.shipments.CountAsync(CancellationToken.None);
        }
    }
}
