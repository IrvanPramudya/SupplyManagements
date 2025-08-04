using SupplyManagements.Models;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SupplyManagements.DTO.Companies
{
    public class CompanyCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public IFormFile Logo { get; set; }

        public static implicit operator Company(CompanyCreateDto company)
        {
            return new Company
            {
                Id = new Random().Next(1000,9999),
                Name = company.Name,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                Logo = company.Logo.FileName,
                RegistrationDate = DateTime.Now,
                Status = StatusSelection.Pending,
            };
        }
    }
}
