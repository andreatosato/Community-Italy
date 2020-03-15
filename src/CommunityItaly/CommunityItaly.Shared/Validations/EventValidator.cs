﻿using CommunityItaly.Shared.ViewModels;
using FluentValidation;

namespace CommunityItaly.Shared.Validations
{
	public class EventValidator : AbstractValidator<EventViewModel>
	{
		public EventValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.StartDate).NotNull().LessThanOrEqualTo(x => x.EndDate);
			RuleFor(x => x.EndDate).NotNull().GreaterThanOrEqualTo(x => x.StartDate);
			When(x => x.CFP != null, () =>
			{
				RuleFor(x => x.CFP.Url).NotEmpty();
				RuleFor(x => x.CFP.StartDate).NotNull().LessThanOrEqualTo(x => x.CFP.EndDate);
				RuleFor(x => x.CFP.EndDate).NotNull().GreaterThanOrEqualTo(x => x.CFP.StartDate);
			});
		}
	}
}
