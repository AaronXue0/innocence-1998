using UnityEngine;

namespace CustomDrag
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TargetTrigger : MonoBehaviour
    {
        public string eventName;
    }
}