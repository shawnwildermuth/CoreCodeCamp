using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models.Validators
{
  public class EventInfoValidator : AbstractValidator<EventInfoViewModel>
  {
    public EventInfoValidator()
    {
      RuleFor(c => c.Moniker).NotEmpty().Length(2,20);
      RuleFor(c => c.Name).NotEmpty().Length(5, 100);
      RuleFor(c => c.Description).NotEmpty().Length(100, 4000);
      RuleFor(c => c.EventDate).NotNull();
      RuleFor(c => c.EventLength).GreaterThan((short)0);
      RuleFor(c => c.CallForSpeakersOpened).NotNull();
      RuleFor(c => c.CallForSpeakersClosed).NotNull();
      RuleFor(c => c.ContactEmail).NotNull().NotEmpty().EmailAddress();
      RuleFor(c => c.SessionizeId).MaximumLength(250);
      RuleFor(c => c.SessionizeEmbedId).MaximumLength(250);
      RuleFor(c => c.FacebookLink).MaximumLength(250);
      RuleFor(c => c.InstagramLink).MaximumLength(250);
      RuleFor(c => c.RegistrationLink).MaximumLength(250);
      RuleFor(c => c.TwitterLink).MaximumLength(250);

    }
  }
}
