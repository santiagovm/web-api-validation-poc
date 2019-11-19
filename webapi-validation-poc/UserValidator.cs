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
                .Must((user, list, context) =>
                {
                    if (null == user.SIN)
                    {
                        return true;
                    }
                    
                    context.MessageFormatter.AppendArgument("SIN", user.SIN);
                    return Utilities.IsValidSIN(int.Parse(user.SIN));

                })
                .WithMessage("SIN ({SIN}) is not valid");
        }
    }
}
