using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupplyManagements.Data;
using SupplyManagements.DTO.AppsUsers;
using SupplyManagements.DTO.Companies;
using SupplyManagements.DTO.Vendors;
using SupplyManagements.Models;

namespace SupplyManagements.Controllers
{
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var getAllData = await (from vendor in _context.Vendor
                                    join company in _context.Company on vendor.CompanyId equals company.Id
                                    join user in _context.ApplicationUser on vendor.ApprovedByAdminId equals user.Id
                                    select new ListVendorDto
                                    {
                                        VendorId = vendor.VendorId,
                                        CompanyName = company.Name,
                                        AdminName = user.FullName,
                                        LogisticManagerName = user.FullName,
                                        ApprovalDate = vendor.ApprovalDate,
                                        BusinessField = vendor.BusinessField,
                                        CompanyType = vendor.CompanyType,
                                        IsActive = vendor.IsActive,
                                    }).ToListAsync();

            if (!getAllData.Any())
            {
                return NotFound();
            }

            return View(getAllData);
        }
        public IEnumerable<Vendor> GetVendorData()
        {
            return _context.Set<Vendor>().ToList();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || _context.Vendor == null)
            {
                return NotFound();
            }
            var getVendor = await _context.Vendor.FirstOrDefaultAsync(m=>m.VendorId == id);
            if(getVendor == null)
            {
                return NotFound();
            }
            return View(getVendor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId", "CompanyId", "BusinessField", "CompanyType", "ApprovalDate", "ApprovedByAdminId", "ApprovedByLogisticManagerId", "ApprovedByLogisticManager", "isActive")] Vendor vendor)
        {
            if(vendor.CompanyId != null)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vendor == null)
            {
                return NotFound();
            }
            var getVendor = await _context.Vendor.FindAsync(id);
            if (getVendor == null)
            {
                return NotFound();
            }
            return View(getVendor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("VendorId", "CompanyId", "BusinessField", "CompanyType", "ApprovalDate", "ApprovedByAdminId", "ApprovedByLogisticManagerId", "ApprovedByLogisticManager", "isActive")] Vendor vendor)
        {
            if (id != vendor.VendorId) { return NotFound(id); }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!VendorExists(id))
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
            return View(vendor);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _vendor = await _context.Vendor.FindAsync(id);
                if( _vendor == null)
                {
                    return NotFound(new { message = $"Vendor with id ${id} not found" });
                }
                _context.Vendor.Remove(_vendor);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new{
                     message = "An error occurred while deleting the vendor",ex.Message
                });
            }
        }
        public bool VendorExists(long id)
        {
            return (_context.Vendor?.Any(Vendor => Vendor.VendorId == id)).GetValueOrDefault();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
