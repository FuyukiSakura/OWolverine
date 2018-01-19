using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.Database
{
    public interface IUpdatable
    {
        void Update(IUpdatable obj);
    }
}
