using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentInheritanceRecursiveGenerics.POCOs
{
    public class TopicBeingTaughtBuilder<SELF>
      : TutorInfoBuilder<TopicBeingTaughtBuilder<SELF>>
      where SELF : TopicBeingTaughtBuilder<SELF>
    {
        public SELF TopicOfTeaching(string topic)
        {
            tutor.TopicBeingTaught = topic;
            return (SELF)this;
        }
    }

}
