﻿using Blazorise;
using CommunityItaly.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CommunityItaly.Web.Components.People
{
	public partial class PersonModal : ComponentBase
	{
		[Parameter] public PersonUpdateViewModel PersonData { get; set; } = new PersonUpdateViewModel();

		private Modal modalRef;

		public string MVP_Url { get; set; }

		protected override void OnInitialized()
		{
			if (!string.IsNullOrEmpty(PersonData.MVP_Code))
				MVP_Url = $"https://mvp.microsoft.com/it-it/PublicProfile/{PersonData.MVP_Code}";
		}

		public void Close()
		{
			modalRef.Hide();
			PersonData = new PersonUpdateViewModel();
		}

		public void Open() => modalRef.Show();
	}
}
