using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BreadSpread.Web.Startup))]
namespace BreadSpread.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
