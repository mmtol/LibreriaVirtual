using System.Security.Cryptography;
using System.Text;

namespace LibreriaVirtual.Helpers
{
    public class HelperCrytography
    {
        public static byte[] EncriptarPass(string pass, string salt)
        {
            string contenido = pass + salt;
            SHA512 managed = SHA512.Create();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            for (int i = 1; i <= 50; ++i)
            {
                salida = managed.ComputeHash(salida);
            }
            managed.Clear();

            return salida;
        }
    }
}
