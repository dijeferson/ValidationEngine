using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Validation.Library;


namespace Validation.Library
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationAttribute : System.Attribute
    {
        #region Constructors
        public ValidationAttribute()
        {
            Required = false;
            MaxSize = -1;
            MinSize = -1;
            MaxValue = -1;
            MinValue = -1;
            AllowedInputType = InputType.Any;
        }
        #endregion

        #region Enums
        /// <summary>
        /// Allowed input types 
        /// </summary>
        public enum InputType
        {
            Numeric,
            Alphanumeric,
            Alphabetic,
            Email,
            URL,
            Any
        }

        /// <summary>
        /// Error codes used in the error treatment
        /// </summary>
        public enum ErrorCode
        {
            Valid = 0,
            IsNotNullOrEmptyError = 1,
            IsBetweenMinMaxSizeError = 2,
            IsBetweenMinMaxValueError = 4,
            IsCorrectInputTypeError = 8,
            PropertyNotInstantiated = 16
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Check if the field is required (not empty or null)
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Check the maximum length of the field as string
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// Check the minimum length of the field as string
        /// </summary>
        public int MinSize { get; set; }

        /// <summary>
        /// Check the maximum integer value allowed in the field
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Check the minimum integer value allowed in the field
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// Check for the data format as string
        /// </summary>
        public InputType AllowedInputType { get; set; }
        #endregion

        #region Methods

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

            // Check if the validation has been activated
            if (MaxSize != -1 || MinSize != -1)
            {
                var q = (string)item;
                if (q != null && ((MaxSize > -1 && q.Length > MaxSize) || (MinSize > -1 && q.Length < MinSize)))
                    isValid = ErrorCode.IsBetweenMinMaxSizeError;
            }

            return isValid;
        }

        public ErrorCode IsBetweenMinMaxValue(object item)
        {
            ErrorCode isValid = ErrorCode.Valid;

            if ((MaxValue != -1 || MinValue != -1) && item != null)
            {
                decimal q = 0;
                var success = decimal.TryParse(item.ToString(), out q);

                if ((MaxValue > -1 && q > MaxValue) || (MinValue > -1 && q < MinValue))
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
                    regex = new Regex(@"^\d+$");
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
                    isValid = !regex.IsMatch(item.ToString()) ? ErrorCode.IsCorrectInputTypeError : isValid;

            return isValid;
        }
        #endregion
    }
}


