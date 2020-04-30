using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Components.Communities
{
	public partial class CommunitySelect : ComponentBase
	{
		[Inject]
		private IHttpServices Http { get; set; }

		[Parameter]
		public string CommunitySelected { get; set; }

		[Parameter]
		public EventCallback<string> CommunitySelectedChanged { get; set; }

		public List<CommunityUpdateViewModel> CommunitiesList { get; set; } = new List<CommunityUpdateViewModel>();

		public CommunityUpdateViewModel CommunityReadOnly { get; set; }

		protected override async Task OnInitializedAsync()
		{
			CommunitiesList = (List<CommunityUpdateViewModel>)await Http.GetCommunitySelect().ConfigureAwait(false);
			if (!string.IsNullOrEmpty(CommunitySelected))
			{
				await CommunitySelectedChanged.InvokeAsync(CommunitySelected);
				CommunityReadOnly = CommunitiesList.FirstOrDefault(x => x.ShortName == CommunitySelected);
			}
		}

		async Task CommunityChanged(string value)
		{
			await CommunitySelectedChanged.InvokeAsync(value);
			if (!string.IsNullOrEmpty(value))
			{	
				CommunityReadOnly = CommunitiesList.FirstOrDefault(x => x.ShortName == value);
			}
			StateHasChanged();
		}
	}
}
