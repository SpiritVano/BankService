namespace BankService.Dtos
{
    // DTO для создания счёта
    public record CreateAccountDto(int UserId, decimal InitialBalance);

    // DTO для возврата счёта
    public record AccountDto(int Id, int UserId, string AccountNumber, decimal Balance);

    // DTO для перевода средств
    public record TransferDto(int FromAccountId, int ToAccountId, decimal Amount);
}
