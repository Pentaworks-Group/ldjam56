using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Model
{
    public class BeeDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public Single? BoostBarMaximum { get; set; } = 10;
        public Single? BoostRemaining { get; set; } = 10;
        public Single? BoostConsumption { get; set; } = 1;
        public Single? BoostStrength { get; set; } = 5;
    }
}
