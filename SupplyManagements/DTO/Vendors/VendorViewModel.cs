using Microsoft.AspNetCore.Mvc.Rendering;
using SupplyManagements.Models;

namespace SupplyManagements.DTO.Vendors
{
    public class VendorViewModel
    {
        public int CompanyId { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }

    }
}
