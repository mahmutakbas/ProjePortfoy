using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() {
            RuleFor(u => u.UserEmail).MinimumLength(5).MaximumLength(150).NotNull();
            RuleFor(u => u.Password).MinimumLength(3).MaximumLength(50).NotNull();
        }
    }
}
