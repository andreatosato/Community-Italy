using Blazorise;
using Blazorise.Sidebar;
using Blazorise.Snackbar;
using CommunityItaly.Shared.ViewModels;

namespace CommunityItaly.Web.Stores
{
	public static class AppStore
	{
		public static NotificationUI Notification { get; set; } = new NotificationUI();
		public static Sidebar Sidebar { get; set; } = new Sidebar();
		public static void AddNotification(NotificationMessage message)
		{
			Notification.AddMessage(message);
		}

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
