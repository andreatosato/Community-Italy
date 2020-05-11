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

		public EventViewModel CreateViewModel { get; set; }

		TimeSpan StartHour { get; set; }
		TimeSpan EndHour { get; set; }
		TimeSpan StartCFPHour { get; set; }
		TimeSpan EndCFPHour { get; set; }
		string ImageUploaded { get; set; } = "";

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			CreateViewModel = new EventViewModel()
			{
				Id = Guid.NewGuid().ToString("N"),
				CFP = new CallForSpeakerViewModel()
			};
			StartHour = CreateViewModel.StartDate.TimeOfDay;
			EndHour = CreateViewModel.EndDate.TimeOfDay;
			if (CreateViewModel.CFP == null)
				CreateViewModel.CFP = new CallForSpeakerViewModel();
			else
			{
				StartCFPHour = CreateViewModel.CFP.StartDate.TimeOfDay;
				EndCFPHour = CreateViewModel.CFP.EndDate.TimeOfDay;
			}
			AppStore.EventImage = null;
		}

		async Task Success()
		{
			var result = await Http.CreateEvent(CreateViewModel).ConfigureAwait(false);
			if (result.IsSuccessStatusCode)
			{
				string resultId = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
				if (AppStore.EventImage != null)
				{
					await Http.UploadEventImage(CreateViewModel.Id, AppStore.EventImage).ConfigureAwait(false);
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
			double seconds = StartHour.TotalSeconds - CreateViewModel.StartDate.TimeOfDay.TotalSeconds;
			CreateViewModel.StartDate = CreateViewModel.StartDate.AddSeconds(seconds);
			StateHasChanged();
		}
		void EndDateTimeChanged(TimeSpan time)
		{
			EndHour = time;
			double seconds = EndHour.TotalSeconds - CreateViewModel.EndDate.TimeOfDay.TotalSeconds;
			CreateViewModel.EndDate = CreateViewModel.EndDate.AddSeconds(seconds);
			StateHasChanged();
		}

		void StartDateCFPTimeChanged(TimeSpan time)
		{
			StartCFPHour = time;
			double seconds = StartCFPHour.TotalSeconds - CreateViewModel.CFP.StartDate.TimeOfDay.TotalSeconds;
			CreateViewModel.CFP.StartDate = CreateViewModel.CFP.StartDate.AddSeconds(seconds);
			StateHasChanged();
		}
		void EndDateTimeCFPChanged(TimeSpan time)
		{
			EndCFPHour = time;
			double seconds = EndHour.TotalSeconds - CreateViewModel.CFP.EndDate.TimeOfDay.TotalSeconds;
			CreateViewModel.CFP.EndDate = CreateViewModel.CFP.EndDate.AddSeconds(seconds);
			StateHasChanged();
		}

		private async Task ValidateFieldAsync(ValidatorEventArgs args, string fieldName)
		{
			var validationResult = await Validator.ValidateAsync(CreateViewModel, default, fieldName);
			if (!validationResult.IsValid)
				SetValidationState(args, validationResult);
		}
	}
}
