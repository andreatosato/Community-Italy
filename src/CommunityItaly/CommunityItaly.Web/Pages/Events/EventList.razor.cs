﻿using CommunityItaly.Shared.ViewModels;
using CommunityItaly.Web.Services;
using CommunityItaly.Web.Stores;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Pages.Events
{
	public partial class EventList : ComponentBase
	{
		[Inject]
		private IHttpServices Http { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }
		public IEnumerable<EventViewModelReadOnly> EventViewModels { get; set; }
		public int PageSize { get; set; } = 10;
		public int PageIndex { get; set; } = 1;
		public int Total { get; set; }

		//protected override async Task OnInitializedAsync()
		//{
		//	//await LoadDataAsync().ConfigureAwait(false);
		//}

		//async Task OnPage(MatPaginatorPageEvent e)
		//{
		//	PageSize = e.PageSize;
		//	PageIndex = e.PageIndex;
		//	await LoadDataAsync().ConfigureAwait(false);
		//}

		void Edit(EventViewModelReadOnly args)
		{
			AppStore.EventEdit = args;
			NavigationManager.NavigateTo(Routes.EventEdit(AppStore.EventEdit.Id));
		}

		async Task Delete(string id)
		{
			await Http.DeleteEvents(id).ConfigureAwait(false);
		}

		private async Task LoadDataAsync()
		{
			var pagedViewModel = await Http.GetEventsConfirmed(PageSize, PageSize * (PageIndex - 1)).ConfigureAwait(false);
			EventViewModels = pagedViewModel.Entities;
			Total = pagedViewModel.Total;

			AppStore.AddNotification(new NotificationMessage("Prova", NotificationMessage.MessageType.Success));
			AppStore.AddNotification(new NotificationMessage("Start", NotificationMessage.MessageType.Success));
			StateHasChanged();
		}

		void Create()
		{
			NavigationManager.NavigateTo(Routes.EventCreate());
		}
	}
}
