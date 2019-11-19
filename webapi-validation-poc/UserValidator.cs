using FluentValidation;

namespace WebApiValidationPoC
{
    // todo: model validator
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is mandatory");
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is mandatory");

            RuleFor(x => x.SIN)
                .NotEmpty()
                .WithMessage("SIN is mandatory")
                .Must((user, sin, context) =>
                {
                    if (null == sin)
                    {
                        return true;
                    }
                    
                    context.MessageFormatter.AppendArgument("SIN", sin);
                    return Utilities.IsValidSIN(int.Parse(sin));

                })
                .WithMessage("SIN ({SIN}) is not valid");
        }
    }
}
