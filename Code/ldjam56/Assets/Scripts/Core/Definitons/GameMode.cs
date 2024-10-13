using System;

using Assets.Scripts.Model;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : BaseDefinition
    {
        public String Name { get; set; }
        public BeeDefinition Bee { get; set; }
        public WorldDefinition World { get; set; }
    }
}
