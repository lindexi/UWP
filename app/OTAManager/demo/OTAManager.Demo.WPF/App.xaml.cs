using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace OTAManager.Demo.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // 启动主机调试
            Task.Run(() =>
            {
                var host = OTAManager.Server.Program.CreateHostBuilder(e.Args).Build();
                Host = host;

                host.Run();
            });

            base.OnStartup(e);
        }

        public static IHost Host { private set; get; }
    }
}
