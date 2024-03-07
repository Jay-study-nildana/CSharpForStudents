using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRetail
{
    //think of this as the Publisher.
    //this is linked to the Subscriber that handles the actual event.

    //we need a delegate that will point to the method
    public delegate void DelegateForCustomerComplaint(int complaintnumber);

    public class ComplaintTrigger
    {

        //we will also need an event that will be linked with the delegate
        private DelegateForCustomerComplaint delegateForCustomerComplaint;

        public event DelegateForCustomerComplaint OnCustomerComplaint
        {
            add
            {
                delegateForCustomerComplaint += value;
            }
            remove
            {
                delegateForCustomerComplaint -= value;
            }
        }

        public void CustomerHaSRaisedComplaint(int complaintnumber)
        {
            if(delegateForCustomerComplaint!=null)
            {
                delegateForCustomerComplaint(complaintnumber);
            }
        }


    }


    public delegate void DelegateForCustomerComplaintPartTwo(int complaintnumber);

    public class ComplaintTriggerPartTwo
    {
        //using anonymouse methods for event
        public event DelegateForCustomerComplaintPartTwo OnCustomerComplaintUsingAnonymous;

        //raise the event.
        public void CustomerHaSRaisedComplaintWithAnonymous(int complaintnumber)
        {
            if (this.OnCustomerComplaintUsingAnonymous != null)
            {
                this.OnCustomerComplaintUsingAnonymous(complaintnumber);
            }
        }
    }
}
