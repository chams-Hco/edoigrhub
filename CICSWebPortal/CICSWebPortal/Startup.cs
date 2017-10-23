using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CICSWebPortal.Startup))]
namespace CICSWebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
