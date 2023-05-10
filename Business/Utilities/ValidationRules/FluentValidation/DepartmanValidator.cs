using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class DepartmanValidator:AbstractValidator<Departman>
    {
        public DepartmanValidator()
        {
            RuleFor(d => d.DepartmanAdi).MinimumLength(3).MaximumLength(150).NotNull();
        }
    }
}
