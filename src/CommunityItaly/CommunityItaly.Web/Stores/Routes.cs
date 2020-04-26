namespace CommunityItaly.Web.Stores
{
	public class Routes
	{
		public static string Home() => "/";

		public static string EventList() => "/events";
		public static string EventEdit(string id) => $"/EventEdit/{id}";
		public static string EventCreate() => $"/EventCreate";

		public static string CommunityList() => "/communities";
		public static string CommunityEdit(string shortname) => $"/CommunityEdit/{shortname}";
		public static string CommunityCreate() => $"/CommunityCreate";

		public static string EventReports() => "/EventReports";
	}
}
