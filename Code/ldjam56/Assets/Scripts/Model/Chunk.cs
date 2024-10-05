using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Chunk
    {
        public GameFrame.Core.Math.Vector2 Position { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Field> Fields { get; set; }

    }
}
