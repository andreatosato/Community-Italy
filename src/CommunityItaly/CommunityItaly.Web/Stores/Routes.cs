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
		public const string Home = "";
		public const string EventList = "event";
		public const string EventEdit = "event/edit/";
		public const string EventCreate = "event/create";
		public const string EventReport = "event/reports";
			   
		public const string CommunityList = "community";
		public const string CommunityEdit = "community/edit";
		public const string CommunityCreate = "community/create";
	}

	public class BreadCrumLink
	{
		public BreadCrumLink(string link, string description, bool active = false)
		{
			Link = link;
			Description = description;
			Active = active;
		}
		public string Link { get; }
		public string Description { get; }
		public bool Active { get; }
	}
}
