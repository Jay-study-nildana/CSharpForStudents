using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRetail
{

    //think of this as the Subscriber
    //this is linked to the Publisher where the event is created
    public class HandleCustomerComplaint
    {
        //the ultimate method that will take care of thigns
        //to be invoked when a customer complaint is raised via an event
        public void TakeCareOfComplaintOne(int complaintnumber)
        {
            var ComplaintHandledString = "Complaint number " + complaintnumber + " has been sorted";
            Console.WriteLine(ComplaintHandledString);
        }
    }
}
