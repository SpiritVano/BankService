namespace BankService.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        // Навигационное свойство (пока не используется в in-memory)
        public User? User { get; set; }
    }
}
