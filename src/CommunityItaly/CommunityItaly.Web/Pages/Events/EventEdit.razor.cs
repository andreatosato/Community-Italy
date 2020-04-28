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

		public EventViewModelReadOnly EventViewModel { get; set; }
		
		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			EventViewModel = AppStore.EventEdit;
			StartHour = EventViewModel.StartDate.TimeOfDay;
			EndHour = EventViewModel.EndDate.TimeOfDay;
			if (EventViewModel.CFP == null)
				EventViewModel.CFP = new CallForSpeakerViewModel();
			AppStore.EventImage = null;
		}

		async Task Success()
		{
			var e = new EventViewModel
			{
				Id = EventViewModel.Id,
				StartDate = EventViewModel.StartDate.AddSeconds(StartHour.TotalSeconds),
				EndDate = EventViewModel.EndDate.AddSeconds(EndHour.TotalSeconds),
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

		async Task FilesReady(FileChangedEventArgs e)
		{
			var image = e.Files.FirstOrDefault();
			AppStore.EventImage = await FileUploadEntry.FromBlazorise(image).ConfigureAwait(false);
		}

		void OnWritten(FileWrittenEventArgs e)
		{
			System.Console.WriteLine($"File: {e.File.Name} Position: {e.Position}");
		}

		void OnProgressed(FileProgressedEventArgs e)
		{
			System.Console.WriteLine($"File: {e.File.Name} Progress: {e.Percentage}");
		}

		//void StartDateTimeChanged(TimeSpan time)
		//{
		//	EventViewModel.StartDate = EventViewModel.StartDate.AddSeconds(time.TotalSeconds - EventViewModel.StartDate.ToLocalTime().Second);
		//}
		//void EndDateTimeChanged(TimeSpan time)
		//{
		//	EventViewModel.EndDate = EventViewModel.EndDate.AddSeconds(time.TotalSeconds - EventViewModel.EndDate.ToLocalTime().Second);
		//}
	}
}
