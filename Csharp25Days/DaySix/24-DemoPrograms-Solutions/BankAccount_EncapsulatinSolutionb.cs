PiggySavings piggyone = new PiggySavings("Jay", 100);

void AddSomeMoney(int money)
{
    try
    {
        piggyone.AddMoneyToPiggy(money);
        Console.WriteLine("Latest Amount in the Piggy Balance : " + piggyone.latestsavings);
    }
    catch(InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }


}

AddSomeMoney(169);
AddSomeMoney(-69);
int amountrecovered = piggyone.RemoveMoney();
Console.WriteLine("Piggy Broken. Amount Recovered : " + amountrecovered);
AddSomeMoney(69);


public class PiggySavings
{
    //keep actual money private
    private int savings_balance;
    //piggy broken or not
    private bool broken = false;
    //name of the savings balance name 
    public string Name {
        get; 
        private set;
    }
    public int latestsavings
    {
        get => savings_balance;
    }

    public PiggySavings(string name, int initialsavings = 0)
    {
        Name = name;
        savings_balance = initialsavings;
    }

    public void AddMoneyToPiggy(int pigamount)
    {
        if(broken == true)
        {
            throw new InvalidOperationException("Cannot add money because piggy has been broken already");
        }

        if(pigamount <=0)
        {
            throw new Exception("You cannot add negative because you can only add money to the piggy");
        }
        savings_balance = savings_balance + pigamount;
    }

    public int RemoveMoney() //one time action beacause you are breaking the piggy
    {
        var totalmoney = savings_balance;
        savings_balance = 0;
        broken = true;
        return totalmoney;
    }

}