using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIConsumerHelper
{
    public class ConstantHelper
    {
        private string baseURI { get; set; }

        public ConstantHelper()
        {
            baseURI = "https://randomstuffapizeropoint4.azurewebsites.net/"; 
        }

        public string returnbaseURI()
        {
            return baseURI;
        }
    }
}
