using Blazorise;
using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Components;
using CommunityItaly.Web.Services;
using CommunityItaly.Web.Stores;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Pages.Events
{
	public partial class EventCreate : ValidationComponent
	{
		[Inject] IHttpServices Http { get; set; }
		[Inject] IValidator<EventViewModel> Validator { get; set; }
		[Inject] ISnackbarService SnackbarService { get; set; }

		public EventViewModel EventViewModel { get; set; }

		TimeSpan StartHour { get; set; }
		TimeSpan EndHour { get; set; }
		TimeSpan StartCFPHour { get; set; }
		TimeSpan EndCFPHour { get; set; }
		string ImageUploaded { get; set; } = "";

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			EventViewModel = new EventViewModel()
			{
				Id = Guid.NewGuid().ToString("N"),
				CFP = new CallForSpeakerViewModel()
			};
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
			var result = await Http.CreateEvent(EventViewModel).ConfigureAwait(false);
			if (result.IsSuccessStatusCode)
			{
				string resultId = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
				if (AppStore.EventImage != null)
				{
					await Http.UploadEventImage(EventViewModel.Id, AppStore.EventImage).ConfigureAwait(false);
				}
				SnackbarService.Show("Evento sottomesso", SnackbarType.Info);
			}
			else
			{
				string error = await result.Content.ReadAsStringAsync();
				SnackbarService.Show("Errore nella creazione", SnackbarType.Error);
			}
		}

		async Task FilesReady(FileChangedEventArgs e)
		{
			var image = e.Files.FirstOrDefault();
			AppStore.EventImage = await FileUploadEntry.FromBlazorise(image).ConfigureAwait(false);
			var uploadImages = new MemoryStream();
			await AppStore.EventImage.StreamData.CopyToAsync(uploadImages).ConfigureAwait(false);
			string typeImage = e.Files.FirstOrDefault().Type;
			ImageUploaded = $"data:{typeImage};base64,{Convert.ToBase64String(uploadImages.ToArray())}";
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

		private async Task ValidateFieldAsync(ValidatorEventArgs args, string fieldName)
		{
			var validationResult = await Validator.ValidateAsync(EventViewModel, default, fieldName);
			if (!validationResult.IsValid)
				SetValidationState(args, validationResult);
		}
	}
}
