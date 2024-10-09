using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Assets.Scripts.Model
{
    public class Chunk
    {
        public GameFrame.Core.Math.Vector2 Position { get; set; }
        public Boolean IsHome { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Field> Fields { get; set; }

        [JsonIgnore]
        public Boolean IsUpdateRequired { get; set; }

        public List<Field> GetEdgeFields(EdgeSide edge)
        {
            return this.Fields.Where(f => f.Edges.HasFlag(edge)).ToList();
        }

        public Field GetEdgeField(EdgeSide edge)
        {
            return this.Fields.FirstOrDefault(f => f.Edges == edge);
        }
    }
}
