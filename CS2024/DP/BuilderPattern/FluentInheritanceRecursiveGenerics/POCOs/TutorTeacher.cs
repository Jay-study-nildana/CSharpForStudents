using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentInheritanceRecursiveGenerics.POCOs
{
    public class TutorTeacher
    {
        public string TutorName;

        public string TopicBeingTaught;

        public DateTime DateOfNextWorkshop;

        //here focus the connection to the Builder
        //Builder links up with NextWorkshopDateBuilder
        //NextWorkshopDateBuilder links up with TopicBeingTaughtBuilder
        //TopicBeingTaughtBuilder links up with TutorInfoBuilder
        //all of them use the type SELF, so, they are able to use inheritance across different types

        public class Builder : NextWorkshopDateBuilder<Builder>
        {
            internal Builder() { }
        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(TutorName)}: {TutorName}, {nameof(TopicBeingTaught)}: {TopicBeingTaught}, {nameof(DateOfNextWorkshop)} : {DateOfNextWorkshop}";
        }
    }
}
