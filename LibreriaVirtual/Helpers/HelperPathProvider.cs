using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace LibreriaVirtual.Helpers
{
    public enum Carpetas { Contenidos, Users }

    public class HelperPathProvider
    {
        private IWebHostEnvironment host;
        private IServer server;

        public HelperPathProvider(IWebHostEnvironment host, IServer server)
        {
            this.host = host;
            this.server = server;
        }

        public string MapPath(string archivo, Carpetas carpeta)
        {
            string ruta = "";
            if (carpeta == Carpetas.Contenidos)
            {
                ruta = Path.Combine("images", "contenidos");
            }
            else if (carpeta == Carpetas.Users)
            {
                ruta = Path.Combine("images", "users");
            }
            string root = host.WebRootPath;
            string path = Path.Combine(root, ruta, archivo);
            return path;
        }

        public string MapUrlPath(string archivo, Carpetas carpeta)
        {
            string ruta = "";
            if (carpeta == Carpetas.Contenidos)
            {
                ruta = "images/contenidos";
            }
            else if (carpeta == Carpetas.Users)
            {
                ruta = "images/users";
            }
            var addresses = this.server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = serverUrl + "/" + ruta + "/" + archivo;
            return urlPath;
        }
    }
}
