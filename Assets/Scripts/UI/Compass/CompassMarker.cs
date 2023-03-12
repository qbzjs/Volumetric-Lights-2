using UnityEngine;
using UnityEngine.UI;

namespace UI.Compass
{
    public class CompassMarker : MonoBehaviour
    {
        public Sprite Icon;
        [ReadOnly] public Image Image;

        public Vector2 Position
        {
            get
            {
                Vector3 position = transform.position;
                return new Vector2(position.x, position.z);
            }
        }
    }
}
