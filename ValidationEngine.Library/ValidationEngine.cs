using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ValidationEngine.Library
{
    public class ValidationEngine
    {
        #region Private Attributes
        private Dictionary<PropertyInfo, List<string>> error;
        #endregion

        #region Public Attributes
        public Dictionary<PropertyInfo, List<string>> Error
        {
            get { return error; }
        }
        #endregion

        public ValidationEngine()
        {
            error = new Dictionary<PropertyInfo, List<string>>();
        }

        public void Validate<T>(T item)
        {
            var properties = item.GetType().GetRuntimeProperties();

            foreach (var property in properties)
            {
                var customAtt = property.GetCustomAttributes(typeof(ValidationAttribute), true);

                foreach (var att in customAtt)
                {
                    var valAtt = att as ValidationAttribute;
                    if (valAtt == null) continue;

                    var validity = valAtt.IsValid(property.GetValue(item, null));

                    if (validity == 0) continue;
                    else
                    {
                        var errorList = new List<string>();

                        for (int bit = 0; bit < 5; bit++)
                        {
                            int cbit = (int)Math.Pow(2, bit);
                            var cerror = (int)validity;

                            if ((cerror & cbit) == cbit)
                                errorList.Add(Enum.GetName(typeof(ValidationAttribute.ErrorCode), cbit));
                        }

                        this.error.Add(property, errorList);
                    }


                }

            }
        }
    }
}
