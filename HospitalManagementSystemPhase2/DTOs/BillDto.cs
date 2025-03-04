using HospitalManagementSystemPhase2.Entities;

namespace HospitalManagementSystemPhase2.DTOs
{
    public class BillDto
    {
        public int BillId { get; set; }

        public decimal Amount { get; set; }

        public DateTime BillDate { get; set; }

        public BillStatus Status { get; set; }

        public int PrescriptionId { get; set; }
    }
}
