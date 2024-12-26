using DemoWebsite.DataAccess.Repository;
using DemoWebsite.DataAccess.Repository.IRepository;
using DemoWebsite.Models.ViewModels;
using DemoWebsiteApi.Areas.Identity.Pages.Account;
using DemoWebsiteApi.DataAccess;
using DemoWebsiteApi.Models;
using DemoWebsiteApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoWebsiteApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: User
        public IActionResult Index()
        {
            var users = _db.applicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var item in users)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == item.Id).RoleId;
                item.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return View(users);


        }

        public IActionResult Edit(string Id)
        {
            var roleId = _db.UserRoles.FirstOrDefault(u => u.UserId == Id).RoleId;
            RoleManagementVM roleVM = new RoleManagementVM()
            {
                ApplicationUser = _db.applicationUsers.FirstOrDefault(u => u.Id == Id),
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id
                }),

            };
            roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;
            return View(roleVM);
        }

        [HttpPost]
        public IActionResult Edit(RoleManagementVM roleVM)
        {
            // Retrieve the user from the database
            var user = _db.applicationUsers.FirstOrDefault(u => u.Id == roleVM.ApplicationUser.Id);
            if (user == null)
            {
                // Handle case when user is not found
                return NotFound();
            }

            // Update user details
            user.Email = roleVM.ApplicationUser.Email;
            user.PhoneNumber = roleVM.ApplicationUser.PhoneNumber;
            user.Address = roleVM.ApplicationUser.Address;
            user.City = roleVM.ApplicationUser.City;
            user.State = roleVM.ApplicationUser.State;
            user.PickupAddress = roleVM.ApplicationUser.PickupAddress;
            user.Name = roleVM.ApplicationUser.Name;
            // Update other fields as needed

            // Update user role
            var currentRole = _db.UserRoles.FirstOrDefault(u => u.UserId == user.Id);
            if (currentRole != null)
            {
                _db.UserRoles.Remove(currentRole);
            }

            if (!string.IsNullOrEmpty(roleVM.ApplicationUser.Role))
            {
                _db.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = roleVM.ApplicationUser.Role
                });
            }

            // Save changes to the database
            _db.SaveChanges();

            // Redirect to the Index action or another appropriate action
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            // Retrieve the user from the database
            var user = _db.applicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }


            // Remove the user from the database
            _db.applicationUsers.Remove(user);
            _db.SaveChanges();


            // Redirect to the Index action or another appropriate action
            return RedirectToAction("Index");
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _db.applicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var item in users)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == item.Id).RoleId;
                item.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

             return Json(new { data = users });
        }
        #endregion


    }
}
