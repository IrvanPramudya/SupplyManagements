using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyManagements.Data;
using SupplyManagements.DTO.Companies;
using SupplyManagements.Models;
using System.Diagnostics;

namespace SupplyManagements.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var _company = await _context.Company.OrderByDescending(x => x.Id).ToListAsync();
            //if(_company.Count <1)await
            return _context.Company != null ? View(_company) : Problem("Entity set 'ApplicationDbContext'");
        }
        [HttpGet("/list-company")]
        public async Task<IActionResult> GetAllCompany()
        {
            var companies = await _context.Company
                .OrderByDescending(x => x.Id)
                .Select(company => new ListCompanyDto
                {
                    Id = company.Id,
                    Name = company.Name
                })
                .ToListAsync(); 

            return Ok(companies);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || _context.Company == null)
            {
                return NotFound();
            }
            var getCompany = await _context.Company.FirstOrDefaultAsync(m=>m.Id == id);
            if(getCompany == null)
            {
                return NotFound();
            }
            return View(getCompany);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if(ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Company == null)
            {
                return NotFound();
            }
            var getCompany = await _context.Company.FindAsync(id);
            if (getCompany == null)
            {
                return NotFound();
            }
            return View(getCompany);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id", "Name", "Email", "PhoneNumber", "Logo", "RegistrationDate", "Status")] Company company)
        {
            if (id != company.Id) { return NotFound(id); }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!CompanyExists(id))
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
            return View(company);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _company = await _context.Company.FindAsync(id);
                if( _company == null)
                {
                    return NotFound(new { message = $"Company with id ${id} not found" });
                }
                _context.Company.Remove(_company);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new{
                     message = "An error occurred while deleting the company",ex.Message
                });
            }
        }
        public bool CompanyExists(long id)
        {
            return (_context.Company?.Any(Company => Company.Id == id)).GetValueOrDefault();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
