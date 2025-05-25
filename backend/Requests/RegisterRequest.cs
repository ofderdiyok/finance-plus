namespace FinancePlus.Requests
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string IBAN { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public int UserType { get; set; } = 0;
    }
}
