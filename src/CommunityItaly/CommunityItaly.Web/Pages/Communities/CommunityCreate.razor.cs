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

namespace CommunityItaly.Web.Pages.Communities
{
	public partial class CommunityCreate : ValidationComponent	
	{
		[Inject] private IHttpServices Http { get; set; }
		[Inject] IValidator<CommunityViewModel> Validator { get; set; }
		[Inject] private ISnackbarService SnackbarService { get; set; }
		public CommunityViewModel CreateViewModel { get; set; }
		string ImageUploaded { get; set; } = "";

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			CreateViewModel = new CommunityViewModel();
			AppStore.CommunityImage = null;
		}

		async Task Success()
		{
			var result = await Http.CreateCommunity(CreateViewModel).ConfigureAwait(false);
			if (result.IsSuccessStatusCode)
			{
				string resultId = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
				if (AppStore.EventImage != null)
				{
					string shortname = CreateViewModel.Name.Replace(" ", "-").ToLowerInvariant();
					await Http.UploadCommunityImage(shortname, AppStore.CommunityImage).ConfigureAwait(false);
				}
				SnackbarService.Show("Community sottomessa", SnackbarType.Success);
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
			AppStore.CommunityImage = await FileUploadEntry.FromBlazorise(image).ConfigureAwait(false);
			var uploadImages = new MemoryStream();
			await AppStore.CommunityImage.StreamData.CopyToAsync(uploadImages).ConfigureAwait(false);
			string typeImage = e.Files.FirstOrDefault().Type;
			ImageUploaded = $"data:{typeImage};base64,{Convert.ToBase64String(uploadImages.ToArray())}";
		}

		private async Task ValidateFieldAsync(ValidatorEventArgs args, string fieldName)
		{
			var validationResult = await Validator.ValidateAsync(CreateViewModel, default, fieldName);
			if (!validationResult.IsValid)
				SetValidationState(args, validationResult);
		}
	}
}
