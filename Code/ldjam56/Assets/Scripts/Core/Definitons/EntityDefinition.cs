using System;

using GameFrame.Core.Definitions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Definitons
{
    public class EntityDefinition : BaseDefinition
    {
        public String ModelReference { get; set; }
        public Vector2? Size { get; set; }
        public Single? Chance { get; set; }
    }
}
