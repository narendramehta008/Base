using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.API.Domain.Attributes;

public class OptionsAttribute : ValidationAttribute
{
    private readonly IList<string> _options;

    public OptionsAttribute(params string[] options)
    {
        _options = options;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ErrorMessage = $"Value must be from options : {string.Join(", ", _options)}";

        if (value.GetType() == typeof(IComparable))
        {
            throw new ArgumentException("value has not implemented IComparable interface");
        }

        var currentValue = (IComparable)value;

        bool compareToResult = _options.Contains(currentValue);

        return compareToResult ? ValidationResult.Success : new ValidationResult(ErrorMessage, new List<string>() { validationContext.MemberName });
    }
}