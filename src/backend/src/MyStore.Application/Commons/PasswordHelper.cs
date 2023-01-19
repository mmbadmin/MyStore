namespace MyStore.Application.Commons
{
    using BCrypt.Net;

    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            return BCrypt.HashPassword(password);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Verify(text, hash);
        }
    }
}
