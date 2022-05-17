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

        public void NewGame()
        {
            GameManager.instance.NewGame();
        }

        public void ExitGame()
        {
            GameManager.instance.ExitGame();
        }
    }
}