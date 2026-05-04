namespace ProdjectApi.Domain.Dtos
{
    public class DebtDto
    {
        public int Id { get; set; }
        public int RoomsId { get; set; }
        public int DebtorId { get; set; }
        public string DebtorName { get; set; }
        public int LenderId { get; set; }
        public string LenderName { get; set; }
        public decimal Amount { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}
