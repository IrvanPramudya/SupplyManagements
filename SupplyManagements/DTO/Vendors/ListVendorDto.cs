using System;

namespace SupplyManagements.DTO.Vendors
{
    public class ListVendorDto
    {
        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string BusinessField { get; set; }
        public string CompanyType{ get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string AdminName { get; set; }
        public string LogisticManagerName{ get; set; }
        public bool IsActive { get; set; }
    }
}
