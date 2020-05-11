using CommunityItaly.Shared.ViewModels;
using FluentValidation;

namespace CommunityItaly.Web.Validator
{
	public class CommunityValidator : AbstractValidator<CommunityViewModel>
	{
		public CommunityValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.WebSite).EmailAddress();
		}
	}
}
