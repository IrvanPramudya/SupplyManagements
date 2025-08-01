using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SupplyManagements.Models
{
    public class Vendor
    {
        public int VendorId { get; set; }

        public int CompanyId { get; set; }

        //[Required]
        public string BusinessField { get; set; }

        //[Required]
        public string CompanyType { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public string ApprovedByAdminId { get; set; }
        public ApplicationUser ApprovedByAdmin { get; set; }

        public string ApprovedByLogisticManagerId { get; set; }
        public ApplicationUser ApprovedByLogisticManager { get; set; }

        public bool IsActive { get; set; } = true;
        public Company Company { get; set; }
    }
}