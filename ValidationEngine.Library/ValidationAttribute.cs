using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValidationEngine.Library;


namespace ValidationEngine.Library
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationAttribute : System.Attribute
    {
        public ValidationAttribute()
        {
            Required = false;
            MaxSize = int.MaxValue;
            MinSize = int.MinValue;
            MaxValue = int.MaxValue;
            MinValue = int.MinValue;
            AllowedInputType = InputType.Any;
        }

        public enum InputType
        {
            Numeric,
            Alphanumeric,
            Alphabetic,
            Email,
            URL,
            Any
        }


        public enum ErrorCode
        {
            Valid = 0,
            IsNotNullOrEmptyError = 1,
            IsBetweenMinMaxSizeError = 2,
            IsBetweenMinMaxValueError = 4,
            IsCorrectInputTypeError = 8,
            PropertyNotInstantiated = 16

        }

        public bool Required { get; set; }
        public int MaxSize { get; set; }
        public int MinSize { get; set; }

        public int MaxValue { get; set; }
        public int MinValue { get; set; }

        public InputType AllowedInputType { get; set; }

        public ErrorCode IsValid(object item)
        {
            ErrorCode isValid = 0;

            isValid |= IsNotNullOrEmpty(item);

            if (isValid > 0) return isValid;

            isValid |= IsBetweenMinMaxSize(item);
            isValid |= IsCorrectInputType(item);
            isValid |= IsBetweenMinMaxValue(item);

            return isValid;
        }

        public ErrorCode IsNotNullOrEmpty(object item)
        {
            ErrorCode isValid = ErrorCode.Valid;

            if (Required && (item == null || String.IsNullOrEmpty((string)item)))
                isValid = ErrorCode.IsNotNullOrEmptyError;

            return isValid;
        }

        public ErrorCode IsBetweenMinMaxSize(object item)
        {
            ErrorCode isValid = ErrorCode.Valid;

            var q = (string)item;
            if (q != null && (q.Length > MaxSize || q.Length < MinSize))
                isValid = ErrorCode.IsBetweenMinMaxSizeError;

            return isValid;
        }

        public ErrorCode IsBetweenMinMaxValue(object item)
        {
            ErrorCode isValid = ErrorCode.Valid;

            decimal q = 0;

            if (item != null)
            {
                var success = decimal.TryParse(item.ToString(), out q);

                if (q > MaxValue || q < MinValue || !success)
                    isValid = ErrorCode.IsBetweenMinMaxValueError;
            }

            return isValid;
        }

        public ErrorCode IsCorrectInputType(object item)
        {
            ErrorCode isValid = ErrorCode.Valid;
            Regex regex = null;

            switch (AllowedInputType)
            {
                case InputType.Numeric:
                    regex = new Regex(@"^\d$");
                    break;
                case InputType.Alphabetic:
                    regex = new Regex(@"^[a-zA-Z ]+$");
                    break;
                case InputType.Alphanumeric:
                    regex = new Regex(@"^[a-zA-Z0-9]+$");
                    break;
                case InputType.Email:
                    regex = new Regex(@"^\S+@\S+$");
                    break;
                case InputType.URL:
                    regex = new Regex(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");
                    break;
            }

            if (item != null)
                if (AllowedInputType != InputType.Any)
                    isValid = !regex.IsMatch((string)item) ? ErrorCode.IsCorrectInputTypeError : isValid;

            return isValid;
        }
    }
}


