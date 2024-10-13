using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Bee
    {
        public Single BoostRemaining { get; set; } = 10;
        public Single BoostBarMaximum { get; set; } = 10;
        public Single BoostConsumption { get; set; } = 1;
        public Single BoostStrength { get; set; } = 5;
        public Single BaseSpeed { get; set; } = 1.3f;

        public List<TimedSpeedEvent> ActiveEvents { get; set; } = new List<TimedSpeedEvent>();
    }
}
