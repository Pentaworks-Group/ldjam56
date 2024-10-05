using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.TerrainTest
{
    public class FieldBehaviour : MonoBehaviour
    {
        public Field Field { get; private set; }

        public void SetField(Field field)
        {
            this.Field = field;
        }
    }
}
