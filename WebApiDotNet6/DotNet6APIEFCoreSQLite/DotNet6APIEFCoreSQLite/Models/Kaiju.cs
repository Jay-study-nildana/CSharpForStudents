namespace DotNet6APIEFCoreSQLite.Models
{
    //I was watching Godzilla Vs Kong when making this project. 
    public class Kaiju : ModelBase
    {
        //as they are strings, I have made them nullable with the ?
        //put Kaiju name here. For example, Godzilla or Kong
        public string Name { get; set; }
        //put a description
        public string Description { get; set; }
        //is it an alpha, like Godzilla or a level below like Mothra or something.
        //for example, Alpha or Beta and stuff like that. 
        public string FoodChainLevel { get; set; }
    }
}
