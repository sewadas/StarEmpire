using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public class EmpireFactory
    {
        public static Empire Build(string name, int military = 0, int resources = 0, int wealth = 0)
        {
            var emp = new Empire();
            emp.Name = name;
            emp.Military = military;
            emp.Resources = resources;
            emp.Wealth = wealth;
            if (string.IsNullOrEmpty(name) == false)
            {
                emp.Homeworld = StarSystemFactory.Build(emp, name, DistanceEnum.None, 1, 1, isHomeworld: true);
            }
            return emp;
        }
    }
}
