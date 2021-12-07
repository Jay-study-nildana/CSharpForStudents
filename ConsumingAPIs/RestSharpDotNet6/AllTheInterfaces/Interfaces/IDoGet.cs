using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheInterfaces.Interfaces
{
    public interface IDoGet : ITalkToApiBase
    {
        public Task<IResponse> PerformGetAsync(IRequest Request);

    }
}
