using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebQLKhoDuoc.Startup))]
namespace WebQLKhoDuoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
