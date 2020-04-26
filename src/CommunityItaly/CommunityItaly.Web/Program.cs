using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CommunityItaly.Web.Services;
using FluentValidation;
using System.Net.Http;
using System;
using Blazorise;
using Blazorise.Material;
using Blazorise.Icons.Material;

namespace CommunityItaly.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			

			// More options https://devblogs.microsoft.com/aspnet/blazor-webassembly-3-2-0-preview-5-release-now-available/
			builder.Services.AddTransient(sp => new HttpClient { 
				BaseAddress = new Uri(builder.Configuration["BaseUrl"]) 
			});
			builder.Services
			  .AddBlazorise(options =>
			  {
				  options.ChangeTextOnKeyPress = true;
			  })
			  .AddMaterialProviders()
			  .AddMaterialIcons();

			builder.RootComponents.Add<App>("app");

			builder.Services.AddTransient<IHttpServices, HttpServices>();
			builder.Services.AddValidatorsFromAssemblyContaining<Program>();
			//builder.Services.AddMatToaster(config =>
			//{
			//	config.Position = MatToastPosition.TopRight;
			//	config.PreventDuplicates = true;
			//	config.NewestOnTop = true;
			//	config.ShowCloseButton = true;
			//	config.MaximumOpacity = 95;
			//	config.VisibleStateDuration = 3000;
			//});

			var host = builder.Build();
			
			host.Services
			  .UseMaterialProviders()
			  .UseMaterialIcons();

			await host.RunAsync();
		}
	}
}
