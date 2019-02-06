using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bestilvasketid.dk.Startup))]
namespace bestilvasketid.dk
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
