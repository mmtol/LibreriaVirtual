namespace LibreriaVirtual.Helpers
{
    public class HelperTools
    {
        public static string GenerarSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; ++i)
            {
                int num = random.Next(1, 255);
                char letra = Convert.ToChar(num);
                salt += letra;
            }

            return salt;
        }
    }
}