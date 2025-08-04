using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupplyManagements.Data;
using SupplyManagements.DTO.Companies;
using SupplyManagements.Models;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<IActionResult> Create(CompanyCreateDto company)
        {
            string pathLogo = null;
            if(company.Logo != null)
            {
                var listExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExt = Path.GetExtension(company.Logo.FileName).ToLowerInvariant();
                if(!listExt.Contains(fileExt))
                {
                    return BadRequest("Format file tidak didukung. Harap upload file gambar (jpg, jpeg, png, gif).");
                }
                if (company.Logo.Length > 2 * 1024 * 1024)
                {
                    return BadRequest("Ukuran file terlalu besar. Maksimal 2MB.");
                }
                var uploadtoFile = Path.Combine("wwwroot", "uploads", "logos");
                Directory.CreateDirectory(uploadtoFile);

                var newFileName = Guid.NewGuid().ToString()+fileExt;
                var filePath = Path.Combine(uploadtoFile, newFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await company.Logo.CopyToAsync(fileStream);
                }
                pathLogo = Path.Combine("uploads", "logos", newFileName);
            }
            Company dataCompany = company;
            dataCompany.Logo = pathLogo;
            if(ModelState.IsValid)
            {
                _context.Add(dataCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataCompany);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var companyDto = new CompanyEditDto
            {
                Id = company.Id,
                Name = company.Name,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                ExistingLogoPath = company.Logo,
                RegistrationDate = company.RegistrationDate,
                Status = company.Status
            };

            return View(companyDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyEditDto company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string pathFile = null;
                    if (company == null)
                    {
                        return NotFound();
                    }

                    if (company.Logo != null && company.Logo.Length > 0)
                    {
                        // Validasi file
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(company.Logo.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Logo", "Format file tidak didukung. Harap upload file gambar (jpg, jpeg, png, gif).");
                            return View(company);
                        }

                        if (company.Logo.Length > 2 * 1024 * 1024) // 2MB
                        {
                            ModelState.AddModelError("Logo", "Ukuran file terlalu besar. Maksimal 2MB.");
                            return View(company);
                        }

                        if (!string.IsNullOrEmpty(company.Logo.FileName))
                        {
                            var oldFilePath = Path.Combine("wwwroot",company.ExistingLogoPath);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Simpan file baru
                        var uploadsFolder = Path.Combine("wwwroot", "uploads", "logos");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await company.Logo.CopyToAsync(fileStream);
                        }

                        pathFile = Path.Combine("uploads", "logos", uniqueFileName);
                    }

                    Company updateData = company;
                        updateData.Logo = pathFile;

                    _context.Update(updateData);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
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
                if (!string.IsNullOrEmpty(_company.Logo))
                {
                    var oldFilePath = Path.Combine("wwwroot", _company.Logo);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
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
            ViewBag.StatusList = Enum.GetValues(typeof(StatusSelection))
                                   .Cast<StatusSelection>()
                                   .Select(e => new SelectListItem
                                   {
                                       Value = e.ToString(),
                                       Text = e.ToString()
                                   }).ToList();
            return View();
        }
    }
}
