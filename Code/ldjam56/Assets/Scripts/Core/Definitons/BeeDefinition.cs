using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Model
{
    public class BeeDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public Single? BoostBarMaximum { get; set; }
        public Single? BoostRemaining { get; set; }
        public Single? BoostConsumption { get; set; }
        public Single? BoostStrength { get; set; }
        public Single? BaseSpeed { get; set; }
    }
}
