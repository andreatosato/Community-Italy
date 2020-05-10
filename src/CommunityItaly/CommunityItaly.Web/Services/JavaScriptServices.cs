using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Services
{
	public class JavaScriptServices
	{
		private IJSRuntime JSRuntime { get; }
		public JavaScriptServices(IJSRuntime jsruntime)
		{
			JSRuntime = jsruntime;
		}

		public async Task OpenNewTabLinkAsync(string link)
		{
			await JSRuntime.InvokeAsync<object>("open", link, "_blank");
		}
	}
}
