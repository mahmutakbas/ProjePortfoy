using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class ProjeDetayValidator:AbstractValidator<ProjeDetay>
    {
        public ProjeDetayValidator()
        {
            RuleFor(pd => pd.KaynakId).NotNull();
            RuleFor(pd => pd.KaynakMiktari).GreaterThan(0);
            RuleFor(pd=> pd.ProjeId).NotNull();
        }
    }
}
