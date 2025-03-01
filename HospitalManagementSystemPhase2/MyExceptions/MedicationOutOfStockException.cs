namespace HospitalManagementSystemPhase2.MyExceptions
{
    public class MedicationOutOfStockException : Exception
    {

        public MedicationOutOfStockException(int medicationId)
            : base($"Medication with ID {medicationId} is out of stock.")
        {
        }
    }

}
