﻿namespace CommunityItaly.Web.Stores
{
	public class Routes
	{
		public static string EventEdit(string id) => $"/EventEdit/{id}";
		public static string EventCreate() => $"/EventCreate";

		public static string CommunityEdit(string shortname) => $"/CommunityEdit/{shortname}";
		public static string CommunityCreate() => $"/CommunityCreate";
	}
}
