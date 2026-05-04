namespace ProdjectApi.Domain.Requests.Rooms
{
    public class GroupBalanceRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string BalanceText { get; set; } = string.Empty;
    }
}
