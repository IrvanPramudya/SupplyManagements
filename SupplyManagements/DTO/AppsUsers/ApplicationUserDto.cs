using SupplyManagements.Models;
using System.ComponentModel.DataAnnotations;

namespace SupplyManagements.DTO.AppsUsers
{
    public class ApplicationUserDto
    {
        [ScaffoldColumn(false)]
        public string Id {  get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator ApplicationUser(ApplicationUserDto applicationUser)
        {
            return new ApplicationUser
            {
                Id =applicationUser.Id,
                UserName = applicationUser.UserName,
                FullName = applicationUser.FullName,
                PasswordHash = applicationUser.Password,
                PhoneNumber = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
            };
        }
        public static implicit operator ApplicationUserDto(ApplicationUser applicationUser)
        {
            return new ApplicationUserDto
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                FullName = applicationUser.FullName,
                Email = applicationUser.Email,
                Password = applicationUser.PasswordHash,
                PhoneNumber = applicationUser.PhoneNumber
                
            };
        }
    }
}
