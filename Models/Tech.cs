using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public class Tech : ITech
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int RequiresYear { get; set; }
    }
}
