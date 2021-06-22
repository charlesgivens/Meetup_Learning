using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetupApi.Models;
using MeetupAPI.Entities;

namespace MeetupApi.Validators
{
  public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
  {
    public RegisterUserValidator(MeetupContext meetupContext)
    {
      RuleFor(x => x.FirstName).NotEmpty();
      RuleFor(x => x.LastName).NotEmpty();
      RuleFor(x => x.Email).NotEmpty();
      RuleFor(x => x.Password).MinimumLength(8);
      RuleFor(x => x).Custom((value, context) => {
        if(value.Password != value.ConfirmPassword)
        {
          context.AddFailure("Password", "Passwords must match Confirm Password");
          context.AddFailure("ConfirmPassword", "Confirm Password must match Passwords");
        }
      });
      RuleFor(x => x.Email).Custom((value, context) => {
        var UserExists = meetupContext.Users.Any(user => user.Email == value);

        if(UserExists)
        {
          context.AddFailure("Email", "Email already exists. Try signing in or use the forgot password tool.");
        }
      });
    }
  }
}