using Microsoft.Owin;
using Owin;
using WorldofWords;

[assembly: OwinStartup(typeof(Startup))]

namespace WorldofWords
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
