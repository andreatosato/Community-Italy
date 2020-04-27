namespace CommunityItaly.Web.Stores
{
	public class Routes
	{
		public static string Home() => "/";

		public static string EventList() => "/event";
		public static string EventEdit(string id) => $"/Event/Edit/{id}";
		public static string EventCreate() => $"/Event/Create";

		public static string CommunityList() => "/community";
		public static string CommunityEdit(string shortname) => $"/Community/Edit/{shortname}";
		public static string CommunityCreate() => $"/Community/Create";

		public static string EventReports() => "/EventReports";
	}

	public class BreadCrum
	{
		public static string Home = "";
		public static string EventList = "Event";
		public static string EventEdit = "Event/Edit/";
		public static string EventCreate = "Event/Create";
		public static string EventReport = "Event/Reports";

		public static string CommunityList = "Community";
		public static string CommunityEdit = "Community/Edit";
		public static string CommunityCreate = "Community/Create";

	}
}
