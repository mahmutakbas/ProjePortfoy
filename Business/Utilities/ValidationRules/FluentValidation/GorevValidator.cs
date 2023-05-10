using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class GorevValidator:AbstractValidator<Gorev>
    {
        public GorevValidator()
        {
            RuleFor(g => g.GorevAdi).MinimumLength(3).MaximumLength(300);
        }
    }
}
