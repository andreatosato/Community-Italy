using CommunityItaly.Shared;
using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Pages.Reports
{
	public partial class EventReports : ComponentBase
	{
		[Inject] IHttpServices Http { get; set; }
		[Inject] ISnackbarService SnackbarService { get; set; }

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
			ManagerIsOpen = true;
		}

		void ManagerOnConfirm(bool isOpen)
		{
			ManagerIsOpen = isOpen;
			ManagerSelected = new PersonUpdateViewModel();
		}
	}

	public class SearchReport
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
