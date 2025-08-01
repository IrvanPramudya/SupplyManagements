using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyManagements.Data;
using SupplyManagements.DTO.AppsUsers;
using SupplyManagements.DTO.Companies;
using SupplyManagements.Models;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SupplyManagements.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.ApplicationUser
                .OrderByDescending(x => x.Id)
                .Select(user => new ApplicationUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                })
                .ToListAsync();

            return View(users);
        }
        [HttpGet("/list-users")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _context.ApplicationUser
                .OrderByDescending(x => x.Id)
                .Select(user => new ApplicationUserDto
                {
                    Id = user.Id,
                    FullName = user.FullName
                })
                .ToListAsync();

            return Ok(users);
        }
        public async Task<IActionResult> Details(string id)
        {
            if(id == null || _context.ApplicationUser == null)
            {
                return NotFound();
            }
            var getApplicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m=>m.Id == id);
            if(getApplicationUser == null)
            {
                return NotFound();
            }
            return View(new ApplicationUserDto
            {
                UserName = getApplicationUser.UserName,
                FullName = getApplicationUser.FullName,
                Email = getApplicationUser.Email,
                PhoneNumber = getApplicationUser.PhoneNumber,

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUserDto applicationUser)
        {
            applicationUser.Id = GenerateCustomId();
            if (applicationUser.Id != null)
            {
                _context.Add((ApplicationUser)applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationUser == null)
            {
                return NotFound();
            }
            var getApplicationUser = await _context.ApplicationUser.FindAsync(id);
            if (getApplicationUser == null)
            {
                return NotFound();
            }
            return View(new ApplicationUserDto
            {
                UserName = getApplicationUser.UserName,
                FullName = getApplicationUser.FullName,
                Email = getApplicationUser.Email,
                PhoneNumber = getApplicationUser.PhoneNumber,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id", "FullName", "UserName", "Email", "PhoneNumber", "Password")] ApplicationUserDto applicationUser)
        {
            if (id != applicationUser.Id) { return NotFound(id); }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!ApplicationUserExists(long.Parse(id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _applicationUser = await _context.ApplicationUser.FindAsync(id);
                if( _applicationUser == null)
                {
                    return NotFound(new { message = $"ApplicationUser with id ${id} not found" });
                }
                _context.ApplicationUser.Remove(_applicationUser);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new{
                     message = "An error occurred while deleting the applicationUser",ex.Message
                });
            }
        }
        public bool ApplicationUserExists(long id)
        {
            return (_context.ApplicationUser?.Any(ApplicationUser => long.Parse(ApplicationUser.Id) == id)).GetValueOrDefault();
        }
        public IActionResult Create()
        {
            return View();
        }

        private string GenerateCustomId()
        {
            return $"USER-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }
    }
}
