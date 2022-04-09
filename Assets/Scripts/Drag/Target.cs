using UnityEngine;

namespace CustomDrag
{
    [CreateAssetMenu(fileName = "Target", menuName = "Game/Drag/Target")]
    public class Target : ScriptableObject
    {
        public string eventName;
        public Vector2 leftTop;
        public Vector2 rightBottom;

        public bool IsInsideTarget(Vector2 position)
        {
            if (position.x < leftTop.x) return false;
            if (position.x > rightBottom.x) return false;
            if (position.y > leftTop.y) return false;
            if (position.y < rightBottom.y) return false;
            return true;
        }
    }
}