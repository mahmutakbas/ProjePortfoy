using Entities.Concrete;
using FluentValidation;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class ProjeValidator : AbstractValidator<Proje>
    {
        public ProjeValidator()
        {
            RuleFor(p => p.ProjeAdi).MinimumLength(3).MaximumLength(300).NotNull();
            RuleFor(p => p.ProjeMusteri).MinimumLength(3).MaximumLength(150).NotNull();
        }
    }
}
