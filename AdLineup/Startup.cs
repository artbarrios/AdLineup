using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdLineup.Startup))]
namespace AdLineup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
