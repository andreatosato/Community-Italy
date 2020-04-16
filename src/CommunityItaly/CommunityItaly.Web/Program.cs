using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CommunityItaly.Web.Services;
using CommunityItaly.Web.Stores;

namespace CommunityItaly.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddBaseAddressHttpClient();
			builder.Services.AddTransient<IHttpServices, HttpServices>();
			builder.Services.AddSingleton<ICommunityItalyStore, CommunityItalyStore>();

			await builder.Build().RunAsync();
		}
	}
}
