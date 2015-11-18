using System.Linq;
using Validation.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Validation.Tests
{
    [TestClass]
    public class ValidationTestCases
    {
        internal ValidationEngine engine = null;

        public ValidationEngine Engine
        {
            get
            {
                if (engine == null)
                    engine = new ValidationEngine();

                return engine;
            }
        }

        [TestMethod]
        public void SetupEngine()
        {
            Assert.IsNotNull(Engine);
        }

        public class TestModels
        {
            public class RequiredFieldModel
            {
                [Validation(Required = true)]
                public string Field { get; set; }
            }

            public class MaxMinSizeFieldModel
            {
                [Validation(MaxSize = 10)]
                public string MaxField { get; set; }

                [Validation(MinSize = 5)]
                public string MinField { get; set; }

                [Validation(MaxSize = 10, MinSize = 5)]
                public string MaxMinField { get; set; }

            }

            public class MaxMinValueFieldModel
            {
                [Validation(MaxValue = 10)]
                public string MaxField { get; set; }

                [Validation(MinValue = 5)]
                public string MinField { get; set; }
            }


            public class InputTypeFieldModel
            {
                [Validation(AllowedInputType = Validation.Library.ValidationAttribute.InputType.Alphabetic)]
                public string AlphabeticField { get; set; }

                [Validation(AllowedInputType = Validation.Library.ValidationAttribute.InputType.Alphanumeric)]
                public string AlphanumericField { get; set; }

                [Validation(AllowedInputType = Validation.Library.ValidationAttribute.InputType.Any)]
                public string AnyField { get; set; }

                [Validation(AllowedInputType = Validation.Library.ValidationAttribute.InputType.Email)]
                public string EmailField { get; set; }

                [Validation(AllowedInputType = Validation.Library.ValidationAttribute.InputType.Numeric)]
                public string NumericField { get; set; }

                [Validation(AllowedInputType = Validation.Library.ValidationAttribute.InputType.URL)]
                public string URLField { get; set; }
            }

        }

        [TestInitialize]
        public void Setup()
        {
            engine = new ValidationEngine();
        }

        [TestCleanup]
        public void TearDown()
        {
            engine = null;
        }

        [TestMethod]
        public void TestEngineInstance()
        {
            engine.Validate(this);
        }

        [TestMethod]
        public void TestRequiredFieldWithError()
        {
            var model = new TestModels.RequiredFieldModel
            {
                Field = string.Empty
            };

            Engine.Validate(model);

            var result = Engine.Error.FirstOrDefault().Value.First();

            Assert.AreEqual(ValidationAttribute.ErrorCode.IsNotNullOrEmptyError.ToString(), result);
        }

        [TestMethod]
        public void TestRequiredFieldPassed()
        {
            var model = new TestModels.RequiredFieldModel
            {
                Field = "Hello World"
            };

            Engine.Validate(model);

            var result = Engine.Error.FirstOrDefault().Value.Count == 0;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMaxSizeFieldPassed()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MaxField = "Super"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MaxField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMaxSizeFieldWithError()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MaxField = "Supercalifragilisticexpialidocious"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MaxField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMinSizeFieldPassed()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MinField = "Supercalifragilisticexpialidocious"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MinField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMinSizeFieldWithError()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MinField = "X"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MinField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMaxMinSizeFieldPassed()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MaxMinField = "Hello Test"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MaxMinField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMinSizeFieldWithMaxError()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MaxMinField = "1234567890ABCDEF"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MaxMinField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMinSizeFieldWithMinError()
        {
            var model = new TestModels.MaxMinSizeFieldModel
            {
                MaxMinField = "1"
            };

            Engine.Validate(model);

            var result = Engine.Error.Single(q => q.Key.Name == "MaxMinField").Value.Contains(ValidationAttribute.ErrorCode.IsBetweenMinMaxSizeError.ToString());

            Assert.IsTrue(result);
        }
    }

}
