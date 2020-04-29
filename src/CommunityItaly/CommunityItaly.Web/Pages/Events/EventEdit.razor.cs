using Blazorise;
using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Services;
using CommunityItaly.Web.Stores;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Pages.Events
{
	public partial class EventEdit : ComponentBase
	{

		[Inject]
		private IHttpServices Http { get; set; }

		[Parameter]
		public string Id { get; set; }
		TimeSpan StartHour { get; set; }
		TimeSpan EndHour { get; set; }
		TimeSpan StartCFPHour { get; set; }
		TimeSpan EndCFPHour { get; set; }

		public EventViewModelReadOnly EventViewModel { get; set; }
		
		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			EventViewModel = AppStore.EventEdit;
			StartHour = EventViewModel.StartDate.TimeOfDay;
			EndHour = EventViewModel.EndDate.TimeOfDay;
			if (EventViewModel.CFP == null)
				EventViewModel.CFP = new CallForSpeakerViewModel();
			else
			{
				StartCFPHour = EventViewModel.CFP.StartDate.TimeOfDay;
				EndCFPHour = EventViewModel.CFP.EndDate.TimeOfDay;
			}
			AppStore.EventImage = null;
		}

		async Task Success()
		{
			var e = new EventViewModel
			{
				Id = EventViewModel.Id,
				StartDate = EventViewModel.StartDate,
				EndDate = EventViewModel.EndDate,
				CFP = string.IsNullOrEmpty(EventViewModel.CFP.Url) ? null : EventViewModel.CFP,
				CommunityName = EventViewModel.Community.ShortName,
				Name = EventViewModel.Name
			};
			if (!string.IsNullOrEmpty(EventViewModel.BuyTicket))
			{
				e.BuyTicket = EventViewModel.BuyTicket;
			}
			var responseUpdate = await Http.UpdateEvent(e).ConfigureAwait(false);
			if (responseUpdate.IsSuccessStatusCode)
			{
				if (AppStore.EventImage != null)
				{
					var responseUploadMessage = await Http.UploadEventImage(e.Id, AppStore.EventImage).ConfigureAwait(false);
					if (!responseUploadMessage.IsSuccessStatusCode)
						AppStore.AddNotification(new NotificationMessage("Errore salvataggio", NotificationMessage.MessageType.Danger));
					else
						AppStore.AddNotification(new NotificationMessage("Evento salvato", NotificationMessage.MessageType.Success));
				}
				else
				{
					AppStore.AddNotification(new NotificationMessage("Evento salvato", NotificationMessage.MessageType.Success));
				}
			}
		}

		void SelectCommunity(string value)
		{
			EventViewModel.Community.ShortName = value;
		}

		async Task FilesReady(FileChangedEventArgs e)
		{
			var image = e.Files.FirstOrDefault();
			AppStore.EventImage = await FileUploadEntry.FromBlazorise(image).ConfigureAwait(false);
		}

		void OnProgressed(FileProgressedEventArgs e)
		{
			System.Console.WriteLine($"File: {e.File.Name} Progress: {e.Percentage}");
		}

		void StartDateTimeChanged(TimeSpan time)
		{
			StartHour = time;
			double seconds = StartHour.TotalSeconds - EventViewModel.StartDate.TimeOfDay.TotalSeconds;
			EventViewModel.StartDate = EventViewModel.StartDate.AddSeconds(seconds);
			StateHasChanged();
		}
		void EndDateTimeChanged(TimeSpan time)
		{
			EndHour = time;
			double seconds = EndHour.TotalSeconds - EventViewModel.EndDate.TimeOfDay.TotalSeconds;
			EventViewModel.EndDate = EventViewModel.EndDate.AddSeconds(seconds);
			StateHasChanged();
		}

		void StartDateCFPTimeChanged(TimeSpan time)
		{
			StartCFPHour = time;
			double seconds = StartCFPHour.TotalSeconds - EventViewModel.CFP.StartDate.TimeOfDay.TotalSeconds;
			EventViewModel.CFP.StartDate = EventViewModel.CFP.StartDate.AddSeconds(seconds);
			StateHasChanged();
		}
		void EndDateTimeCFPChanged(TimeSpan time)
		{
			EndCFPHour = time;
			double seconds = EndHour.TotalSeconds - EventViewModel.CFP.EndDate.TimeOfDay.TotalSeconds;
			EventViewModel.CFP.EndDate = EventViewModel.CFP.EndDate.AddSeconds(seconds);
			StateHasChanged();
		}
	}
}
