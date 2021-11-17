using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StarEmpire
{
    public class Game
    {
        public Empire Player;
        public List<IStarSystem> StarMap;
        public Queue<Func<Empire, string>> Incidents;
        
        public Game(string empireName = "Home world")
        {
            Player = EmpireFactory.Build(empireName, 0, 0, 0);
            StarMap = new List<IStarSystem>();
            StarMap.Add(Player.Homeworld);
            StarMap.AddRange(StarSystemFactory.SystemTemplateList.ToArray().OrderBy(c => Guid.NewGuid()).ToList());
            
            Incidents = new Queue<Func<Empire, string>>(IncidentFactory.IncidentTemplateList.OrderBy(c => Guid.NewGuid()).ToList());
        }

        
    }
}
