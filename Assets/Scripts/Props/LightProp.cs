using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Experimental.Rendering.Universal;

namespace Innocence
{
    public class LightProp : MonoBehaviour
    {
        public int id;

        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
            }
        }
        public void LightSwitch(bool isActive)
        {
            switch (isActive)
            {
                case true:
                    anim.SetTrigger("On");
                    break;
                case false:
                    anim.SetTrigger("Off");
                    break;
            }
        }
    }
}