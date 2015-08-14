using System.Globalization;
using System.Windows.Controls;

namespace PhotoShop
{
    public class RangedIntValueValidationRule : ValidationRule
    {
        public int? MinValue
        {
            get;
            set;
        }

        public int? MaxValue
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int margin;

            // Is a number?
            if (!int.TryParse((string)value, out margin))
            {
                return new ValidationResult(false, "Not a number.");
            }

            // Is in range?
            if (MinValue.HasValue && (margin < MinValue) || MaxValue.HasValue && (margin > MaxValue))
            {
                string msg = $"Value must be between {MinValue} and {MaxValue}.";
                return new ValidationResult(false, msg);
            }

            // Number is valid
            return new ValidationResult(true, null);
        }
    }
}