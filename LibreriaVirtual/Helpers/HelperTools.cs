namespace LibreriaVirtual.Helpers
{
    public enum Tipos { Pelicula, Serie, Documental}
    public enum Generos { Accion, Comedia, Drama, Ficcion, Fantasia, Terror, Suspense, Romance, Familiar }

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