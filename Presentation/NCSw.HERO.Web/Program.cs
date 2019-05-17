﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NCSw.HERO.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }


    }
}
