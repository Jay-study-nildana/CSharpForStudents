using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheInterfaces.Interfaces
{
    public interface IDoPost : ITalkToApiBase
    {
        public Task<IResponse> PerformPostAsync(IRequest Request);

    }
}
