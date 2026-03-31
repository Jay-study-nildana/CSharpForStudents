
ISaySomethingHelper saySomethingHelperOne = new SaySomethingHelperOne();
SaySomething saySomethingOne = new SaySomething(saySomethingHelperOne);
saySomethingOne._sayhelper.SaySomethingThroughHelper();

ISaySomethingHelper saySomethingHelperTwo = new SaySomethingHelperTwo();
SaySomething saySomethingTwo = new SaySomething(saySomethingHelperTwo);
saySomethingTwo._sayhelper.SaySomethingThroughHelper();



public interface ISaySomethingHelper
{
    void SaySomethingThroughHelper();
}

public class SaySomethingHelperTwo : ISaySomethingHelper
{
    public void SaySomethingThroughHelper()
    {
        Console.WriteLine("I said something from helper two");
    }
}

public class SaySomethingHelperOne : ISaySomethingHelper
{
    public void SaySomethingThroughHelper()
    {
        Console.WriteLine("I said something from helper one");
    }
}

public class SaySomething
{
    public ISaySomethingHelper _sayhelper;

    public SaySomething(ISaySomethingHelper sayHelper)
    {
        _sayhelper = sayHelper;
    }


}

