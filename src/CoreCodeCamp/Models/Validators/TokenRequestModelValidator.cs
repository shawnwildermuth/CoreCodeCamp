using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models.Validators
{
    public class TokenRequestModelValidator : AbstractValidator<TokenRequestModel>
    {
    public TokenRequestModelValidator()
    {
      RuleFor(c => c.Username).NotEmpty();
      RuleFor(c => c.Password).NotEmpty();
    }
  }
}
