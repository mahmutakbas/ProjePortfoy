﻿using Entities.Concrete;
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
            RuleFor(pd => pd.Aciklama).NotNull();
            RuleFor(pd=> pd.ProjeId).NotNull();
        }
    }
}
