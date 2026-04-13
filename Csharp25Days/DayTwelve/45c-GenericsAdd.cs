
//let's use the AddTheseTwo class to add two integers

AddTheseTwo<int> addIntegers = new AddTheseTwo<int>(3, 4);
int intResult = addIntegers.Add();

Console.WriteLine($"Adding integers: {intResult}");

//let's use the AddTheseTwo class to add two doubles

AddTheseTwo<double> addDoubles = new AddTheseTwo<double>(5.5, 6.5);

double doubleResult = addDoubles.Add();

Console.WriteLine($"Adding doubles: {doubleResult}");

//let's use the AddTheseTwo class to add two strings

AddTheseTwo<string> addStrings = new AddTheseTwo<string>("Hello, ", "World!");

string stringResult = addStrings.Add();

Console.WriteLine($"Adding strings: {stringResult}");

//let's use the AddTheseTwo class to add two imaginary numbers
AddTheseTwo<ImaginaryNumber> addImaginaryNumbers = new AddTheseTwo<ImaginaryNumber>(
    new ImaginaryNumber(1.0, 2.0),
    new ImaginaryNumber(3.0, 4.0)
);

ImaginaryNumber imaginaryResult = addImaginaryNumbers.Add();

Console.WriteLine($"Adding imaginary numbers: {imaginaryResult}");

public class AddTheseTwo<T>
{
    private T _firstItem;
    private T _secondItem;
    public AddTheseTwo(T FirstItem, T SecondItem)
    {
        _firstItem = FirstItem;
        _secondItem = SecondItem;
    }
    

    public T Add()
    {
        if(typeof(T) == typeof(int))
        {
            int result = Convert.ToInt32(_firstItem) + Convert.ToInt32(_secondItem);
            return (T)(object)result;
        }
        else if(typeof(T) == typeof(double))
        {
            double result = Convert.ToDouble(_firstItem) + Convert.ToDouble(_secondItem);
            return (T)(object)result;
        }
        else if(typeof(T) == typeof(string))
        {
            string result = Convert.ToString(_firstItem) + Convert.ToString(_secondItem);
            return (T)(object)result;
        }
        else if(typeof(ImaginaryNumber).IsAssignableFrom(typeof(T)))
        {
            dynamic? first = _firstItem;
            dynamic? second = _secondItem;
            double? realPart = first?.Real + second?.Real;
            double? imaginaryPart = first?.Imaginary + second?.Imaginary;

            if(realPart == null)
            {
                realPart = 0.0;
            }
            if(imaginaryPart == null)
            {
                imaginaryPart = 0.0;
            }

                return (T)(object)new ImaginaryNumber(realPart, imaginaryPart);
        }
        else
        {
            throw new InvalidOperationException("Unsupported type");
        }
    }
}

public class ImaginaryNumber
{
    public double? Real { get; set; }
    public double? Imaginary { get; set; }
    public ImaginaryNumber(double? real, double? imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public override string ToString()
    {
        return $"{Real} + {Imaginary}i";
    }
}