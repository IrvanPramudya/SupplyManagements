using SupplyManagements.Models;
using System.ComponentModel.DataAnnotations;

namespace SupplyManagements.DTO.Companies
{
    public class CompanyEditDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public IFormFile? Logo { get; set; }
        public string? ExistingLogoPath { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public StatusSelection Status { get; set; }

        public static implicit operator Company(CompanyEditDto dto)
        {
            return new Company
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RegistrationDate = DateTime.Now,
                Logo = dto.Logo.FileName,
                Status = StatusSelection.Pending
            };

        }
    }
}
