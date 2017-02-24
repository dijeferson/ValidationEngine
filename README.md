# ValidationEngine

A .NET Attribute based validation framework for Model and Domain classes

## Available Attribute X Validations

- Required:BOOL, check if the field is required (not empty or null).
- MaxSize:INT, check the maximum length of the field as string.
- MinSize:INT, check the minimum length of the field as string.
- MaxValue:INT, check the maximum integer value allowed in the field.
- MinValue:INT, check the minimum integer value allowed in the field.
- AllowedInputType:ENUM, check for the data format as string.

### AllowedInputType Enum

- Numeric, Only numbers.
- Alphanumeric, Letters, numbers, and symbols.
- Alphabetic, Only letters.
- Email, Email pattern.
- URL, Web URL patter.
- Any, Any character or pattern is accepted.

## Requirements

The project was developed and tested using Visual Studio 2013 and .NET Framework 4.5.

## Usage

```
public class TestModel
{
    [Validation(Required = true, MaxSize = 100, MinSize = 0, AllowedInputType = InputType.Any)]
    public string Field1 { get; set;}

    [Validation(Required = true, MaxValue = 10, MinValue = 1, AllowedInputType = InputType.Numeric)]
    public double Field2 { get; set;}

    [Validation(AllowedInputType = InputType.Email)]
    public string Field3 { get; set;}
}

// After loading the data into the Model in your Controller or ViewModel class:

static void Main(string[] args)
{
    var engine = new ValidationEngine();
    var testModelInstance = LoadTestModelWithData();

    engine.Validate(testModelInstance);

    foreach (var prop in engine.Error)
        foreach (var msg in prop.Value)
            Console.WriteLine(string.Format("{0} {1}", prop.Key.Name, msg));

    Console.ReadKey();
}

```
