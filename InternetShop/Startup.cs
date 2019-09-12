using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InternetShop.Startup))]
namespace InternetShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
