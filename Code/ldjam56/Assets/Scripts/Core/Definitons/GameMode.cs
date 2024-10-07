﻿using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : BaseDefinition
    {
        public String Name { get; set; }
        public Boolean TestFlag { get; set; }
        public WorldDefinition World { get; set; }
    }
}
