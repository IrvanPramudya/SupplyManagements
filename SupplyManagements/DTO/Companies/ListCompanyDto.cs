using SupplyManagements.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SupplyManagements.DTO.Companies
{
    public class ListCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Company(ListCompanyDto company)
        {
            return new Company
            {
                Id = company.Id,
                Name = company.Name,
            };
        }

        public static explicit operator ListCompanyDto(Company company)
        {
            return new ListCompanyDto
            {
                Id = company.Id,
                Name = company.Name,
            };
        }
    }
}
