namespace BankService.Dtos
{
    // DTO для создания пользователя
    public record CreateUserDto(string Name, string Email);

    // DTO для обновления пользователя
    public record UpdateUserDto(string Name);

    // DTO для возврата данных (чтобы не отдавать всё из модели)
    public record UserDto(int Id, string Name, string Email);
}
