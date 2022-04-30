using WebAPIWithDependencyInjection.Interfaces;

namespace WebAPIWithDependencyInjection.POCO
{
    public class SayMyNameTwo : ISayMyName
    {
        public string IAmName()
        {
            var name = "I am Vengence!";

            return name;
        }
    }
}
