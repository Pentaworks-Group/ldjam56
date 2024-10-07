using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Model
{
    public class BeeDefinition : BaseDefinition
    {
        public float RemainingBoost { get; set; } = 10;
        public float MaxBar { get; set; } = 10;
        public float BoostConsumption { get; set; } = 1;
        public float BoostStrength { get; set; } = 5;
    }
}
