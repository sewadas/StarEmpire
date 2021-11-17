using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public interface ITech
    {
        string Name { get; }
        int Cost { get; }
        int RequiresYear { get; }
    }
}
