using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Demo.FinancialFlow.Api.Controllers.Validations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                    continue;

                Type validatorInterfaceType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                object? validatorInstance = context.HttpContext.RequestServices.GetService(validatorInterfaceType);

                if (validatorInstance is IValidator validator)
                {
                    // Build the type for ValidationContext<T> where T is the argument's type
                    Type validationContextType = typeof(ValidationContext<>).MakeGenericType(argument.GetType());
                    // Create a ValidationContext<T> instance for the argument
                    object? validationContextInstance = Activator.CreateInstance(validationContextType, argument);

                    if (validationContextInstance != null)
                    {
                        // Get the Validate method for the validator
                        var validateMethod = validator.GetType().GetMethod("Validate", [validationContextType]);
                        var validationResultObj = validateMethod?.Invoke(validator, [validationContextInstance]);

                        // Cast the result to ValidationResult and check if validation failed
                        if (validationResultObj is FluentValidation.Results.ValidationResult validationResult && !validationResult.IsValid)
                        {
                            // If validation failed, set the result to BadRequest with the errors and stop further execution
                            context.Result = new BadRequestObjectResult(validationResult.Errors);
                            return;
                        }
                    }
                }
            }
            await next();
        }
    }
}
