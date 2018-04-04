using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleSQLCallApp.Startup))]
namespace SimpleSQLCallApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
