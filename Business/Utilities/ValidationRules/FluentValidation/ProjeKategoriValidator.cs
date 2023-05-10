using Entities.Concrete;
using FluentValidation;

namespace Business.Utilities.ValidationRules.FluentValidation
{
    public class ProjeKategoriValidator : AbstractValidator<ProjeKategori>
    {
        public ProjeKategoriValidator()
        {
            RuleFor(pk => pk.ProjeKategoriAdi).MinimumLength(3).MaximumLength(300);
        }
    }
}
