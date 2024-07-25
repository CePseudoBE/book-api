namespace BookApi.Validator{
    public class RegisterDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
    }

    public class ResetPasswordDto
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }

    public class AskTokenDto
    {
        public required string Email {get; set;}

        public required string RedirectUrl {get; set;}
    }
}