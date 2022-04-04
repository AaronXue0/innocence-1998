using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ButtonFeatures : MonoBehaviour
    {
        public void ChangeScene(string name)
        {
            GameManager.instance.ChangeScene(name);
        }
    }
}