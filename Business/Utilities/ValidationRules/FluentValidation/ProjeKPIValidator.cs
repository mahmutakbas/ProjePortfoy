using Entities.Concrete;
using FluentValidation;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class ProjeKPIValidator : AbstractValidator<ProjeKPI>
    {
        public ProjeKPIValidator()
        {
            RuleFor(pk => pk.ProjeId).NotNull();
            RuleFor(pk => pk.KPID).NotNull();
        }
    }
}
