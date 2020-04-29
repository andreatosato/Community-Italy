using System;

namespace CommunityItaly.Shared.ViewModels
{
	public class CallForSpeakerViewModel
	{
		public string Url { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime EndDate { get; set; } = DateTime.Now;
	}
}
