using Blazorise;
using Blazorise.Sidebar;
using CommunityItaly.Shared.ViewModels;
using System;

namespace CommunityItaly.Web.Stores
{
	public static class AppStore
	{
		public static Sidebar Sidebar { get; set; } = new Sidebar();
		public static void AddNotification(NotificationMessage message)
		{
			AddMessage.Invoke(message);
		}
		public static Action<NotificationMessage> AddMessage;

		public static EventViewModelReadOnly EventEdit { get; set; }
		public static CommunityUpdateViewModel CommunityEdit { get; set; }
		public static FileUploadEntry EventImage { get; set; }
		public static FileUploadEntry CommunityImage { get; set; }

		public static Theme Tema { get; set; } = new Theme()
		{
			ColorOptions = new ThemeColorOptions
			{
				Primary = "#3700B3",
				Secondary = "#018786",
			},
			BackgroundOptions = new ThemeBackgroundOptions
			{
				Primary = "#3700B3",
			},
			SidebarOptions = new ThemeSidebarOptions
			{
				Color = "#FFFFFF",
				BackgroundColor = "#3700B3"
			},
		};
	}
}
