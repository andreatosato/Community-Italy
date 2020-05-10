using Blazorise;
using CommunityItaly.Shared;
using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Components.People;
using CommunityItaly.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommunityItaly.Web.Pages.Events
{
	public partial class EventReports : ComponentBase
	{
		[Inject] IHttpServices Http { get; set; }
		[Inject] ISnackbarService SnackbarService { get; set; }
		[Inject] JavaScriptServices JSService { get; set; }
		public string MVP_Url { get; set; }

		private Modal personRef;
		public SearchReport Search { get; set; }

		public List<EventViewModelReadOnly> ReportLists { get; set; } = new List<EventViewModelReadOnly>();
		bool ManagerIsOpen { get; set; } = false;
		PersonUpdateViewModel ManagerSelected { get; set; } = new PersonUpdateViewModel();

		protected override void OnInitialized()
		{
			Search = new SearchReport
			{
				StartDate = DateTime.Now.AddMonths(-3),
				EndDate = DateTime.Now
			};
			base.OnInitialized();
		}

		async Task SearchEvents()
		{
			if (Search.StartDate != null && Search.EndDate != null && Search.StartDate > Search.EndDate)
			{
				SnackbarService.Show("Le date non sono corrette", SnackbarType.Info);
			}
			else
			{
				ReportLists = await Http.GetReportConfirmedIntervalledAsync(Search.StartDate.ToLocalTime(), Search.EndDate.ToLocalTime().EndOfDay());
			}
		}

		async Task Export()
		{
			var response = await Http.GenerateReportEvents(Search.StartDate, Search.EndDate);
			if(response.IsSuccessStatusCode)
			{
				SnackbarService.Show("Il report verrà inviato via mail agli amministratori", SnackbarType.Info);
			}
			else
			{
				SnackbarService.Show("Errore generazione del report", SnackbarType.Error);
			}
		}

		void OpenManagers(PersonUpdateViewModel person)
		{
			ManagerSelected = person;
			if (!string.IsNullOrEmpty(ManagerSelected.MVP_Code))
				MVP_Url = $"https://mvp.microsoft.com/it-it/PublicProfile/{ManagerSelected.MVP_Code}";
			personRef.Show();
		}

		public async Task OpenNewTabLink(string link)
		{
			await JSService.OpenNewTabLinkAsync(link);
		}

		public void Close()
		{
			personRef.Hide();
			ManagerSelected = new PersonUpdateViewModel();
		}

	}

	public class SearchReport
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
