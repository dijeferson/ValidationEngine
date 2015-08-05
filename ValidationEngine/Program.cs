using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationEngine.Library;

namespace ValidationEngine
{
    public class Users
    {
        [Validation(Required = true, MaxSize = 100, MinSize = 5, AllowedInputType = ValidationEngine.Library.ValidationAttribute.InputType.Email)]
        public string UserID { get; set; }

        [Validation(Required = true, MaxSize = 8, MinSize = 1, AllowedInputType = ValidationEngine.Library.ValidationAttribute.InputType.Any)]
        public string UserName { get; set; }

        [Validation(Required = false, MaxSize = 100, MinSize = 5, AllowedInputType = ValidationEngine.Library.ValidationAttribute.InputType.Alphanumeric)]
        public string UserCountry { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            ValidationEngine.Library.ValidationEngine engine = new ValidationEngine.Library.ValidationEngine();
            Users users = new Users
            {
                UserID = "fulano[at]site.com",
                UserName = "Jefersons-123"
            };

            engine.Validate(users);

            foreach (var prop in engine.Error)
                foreach (var msg in prop.Value)
                    Console.WriteLine(string.Format("{0} {1}", prop.Key.Name, msg));

            Console.ReadKey();
        }
    }
}
