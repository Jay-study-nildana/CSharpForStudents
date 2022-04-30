using WebAPIWithDependencyInjection.Interfaces;

namespace WebAPIWithDependencyInjection.POCO
{
    public class SayMyNameOne : ISayMyName
    {
        public string IAmName()
        {
            var name = "I am Batman!";

            return name;
        }
    }
}
