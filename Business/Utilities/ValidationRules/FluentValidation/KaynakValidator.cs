using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class KaynakValidator:AbstractValidator<Kaynak>
    {
        public KaynakValidator()
        {
            RuleFor(k => k.KaynakAdi).MinimumLength(3).MaximumLength(150);
        }
    }
}
