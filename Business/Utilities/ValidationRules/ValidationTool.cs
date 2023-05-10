using FluentValidation;

namespace Business.Utilities.ValidationRules
{
    public class ValidationTool
    {
        public static string Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            string message = string.Empty;
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    message += error + "\n";
                }
            }
            return message;
        }
    }
}
