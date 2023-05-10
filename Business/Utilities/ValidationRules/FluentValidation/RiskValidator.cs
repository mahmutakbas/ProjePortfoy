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
            RuleFor(r => r.RiskKategorisi).MinimumLength(3).MaximumLength(200).NotNull();
            RuleFor(r => r.Olasilik).GreaterThan(0);
            RuleFor(r => r.Etki).GreaterThan(0);
            RuleFor(r => r.RiskSkoru).GreaterThan(0);
            RuleFor(r => r.RiskOnceligi).GreaterThan(0);
        }
    }
}
