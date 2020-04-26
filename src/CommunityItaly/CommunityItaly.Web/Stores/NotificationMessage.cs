using Blazorise.Snackbar;
using System;
using System.Transactions;

namespace CommunityItaly.Web.Stores
{
	public class NotificationMessage
	{
		public NotificationMessage(string message, Exception ex) : 
			this(message, MessageType.Danger)
		{
			Exception = ex;
		}

		public NotificationMessage(string message, MessageType notificationType)
		{
			Message = message;
			NotificationType = notificationType;
		}

		public string Message { get; set; }
		public Exception Exception { get; set; }
		public MessageType NotificationType { get; set; }
		public enum MessageType
		{
			Danger = 0,
			Dark = 1,
			Info = 2,
			Light = 3,
			Link = 4,
			Primary = 5,
			Secondary = 6,
			Success = 7,
			Warning = 8
		}
	}

	public class NotificationUI
	{
		public Snackbar Snackbar { get; set; }
		public string Body { get; private set; }
		public NotificationUI()
		{
			Snackbar = new Snackbar();
		}
		public void AddMessage(NotificationMessage message)
		{
			Body = message.Message;
			switch (message.NotificationType)
			{
				case NotificationMessage.MessageType.Danger:
					Snackbar.Color = SnackbarColor.Danger;
					break;
				case NotificationMessage.MessageType.Dark:
					break;
				case NotificationMessage.MessageType.Info:
					break;
				case NotificationMessage.MessageType.Light:
					break;
				case NotificationMessage.MessageType.Link:
					break;
				case NotificationMessage.MessageType.Primary:
					break;
				case NotificationMessage.MessageType.Secondary:
					break;
				case NotificationMessage.MessageType.Success:
					break;
				case NotificationMessage.MessageType.Warning:
					break;
				default:
					break;
			}
		}
	}
}
