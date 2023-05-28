using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class RiskValidator:AbstractValidator<Risk>
    {
        public RiskValidator()
        {
            RuleFor(r => r.RiskTanimi).MinimumLength(3).MaximumLength(200).NotNull();
        }
    }
}
