using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Model
{
    public class BeeDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public float BoostBarMaximum { get; set; } = 10;
        public float BoostRemaining { get; set; } = 10;
        public float BoostConsumption { get; set; } = 1;
        public float BoostStrength { get; set; } = 5;
    }
}
