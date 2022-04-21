using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ExitProp : MonoBehaviour
    {
        [SerializeField] string sceneName;
        private void OnMouseDown()
        {
            GameManager.instance.ChangeScene(sceneName);
        }
    }

}