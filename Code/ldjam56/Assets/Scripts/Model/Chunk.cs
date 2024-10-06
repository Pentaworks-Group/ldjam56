using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Model
{
    public class Chunk
    {
        public GameFrame.Core.Math.Vector2 Position { get; set; }
        public Boolean IsHome { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Field> Fields { get; set; }

        public List<Field> GetEdgeFields(EdgeSide direction)
        {
            return this.Fields.Where(f => f.Edges.HasFlag(direction)).ToList();
        }
    }
}
