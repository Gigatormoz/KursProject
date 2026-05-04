namespace ProdjectApi.Domain.Dtos
{
    public class ExpenseParticipantDto
    {
        public int Id { get; set; }
        public int ExpensesId { get; set; }
        public int UsersId { get; set; }
        public string UserName { get; set; }
        public decimal TotalDebt { get; set; }
    }
}
