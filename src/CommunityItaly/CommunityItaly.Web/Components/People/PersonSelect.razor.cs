using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Components.People
{
	public partial class PersonSelect : ComponentBase
	{
		[Inject] private IHttpServices Http { get; set; }
		[Parameter]	public PersonSelectViewModel PersonSelected { get; set; }
		[Parameter]	public EventCallback<PersonUpdateViewModel> PersonSelectedChanged { get; set; }

		public IReadOnlyList<PersonSelectViewModel> PeopleToSelect { get; set; } = new List<PersonSelectViewModel>();


		protected override async Task OnInitializedAsync()
		{
			PeopleToSelect = (IReadOnlyList<PersonSelectViewModel>)await Http.GetPersonSelect().ConfigureAwait(false);
			await PersonSelectedChanged.InvokeAsync(PersonSelected);
		}

		public bool SelectRow(PersonSelectViewModel m)
		{
			m.IsSelected = true;
			return true;
		}
	}
}
