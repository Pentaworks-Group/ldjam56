using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Bee
    {
        public float RemainingBoost { get; set; } = 10;
        public float MaxBar { get; set; } = 10;
        public float BoostConsumption { get; set; } = 0;
        public float BoostStrength { get; set; } = 20;
        public float BaseSpeed { get; set; } = 1.3f;

        public List<TimedSpeedEvent> ActiveEvents { get; set; } = new List<TimedSpeedEvent>();
    }
}
