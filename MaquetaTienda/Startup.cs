using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MaquetaTienda.Startup))]
namespace MaquetaTienda
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
