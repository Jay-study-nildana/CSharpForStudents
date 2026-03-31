
var examplecitybig = CityFactory.CreateCity(1);
var examplecitysmall = CityFactory.CreateCity(2);
var examplecitymedium = CityFactory.CreateCity(3);

examplecitybig.GetCityInfo();
examplecitysmall.GetCityInfo();
examplecitymedium.GetCityInfo();

public interface ICityInfo
{
    public void GetCityInfo();
}

public class BigCity : ICityInfo
{
    public void GetCityInfo()
    {
        Console.WriteLine("This is a big city.");
    }
}

public class SmallCity : ICityInfo
{
    public void GetCityInfo()
    {
        Console.WriteLine("This is a small city.");
    }
}

public class MediumCity : ICityInfo
{
    public void GetCityInfo()
    {
        Console.WriteLine("This is a medium city.");
    }
}

public static class CityFactory
{
    public static ICityInfo CreateCity(int type)
    {
        switch (type)
        {
            case 1:
                return new BigCity();
            case 2:
                return new SmallCity();
            case 3:
                return new MediumCity();
            default:
                throw new ArgumentException("Invalid city type");
        }
    }
}