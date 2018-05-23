using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AgProgramlamaOdev2.Startup))]

namespace AgProgramlamaOdev2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        
                app.MapSignalR();
            
        }
    }
}
