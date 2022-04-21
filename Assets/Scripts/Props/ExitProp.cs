using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Innocence
{
    public class ExitProp : MonoBehaviour
    {
        [SerializeField] string sceneName;
        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            GameManager.instance.ChangeScene(sceneName);
        }
    }

}