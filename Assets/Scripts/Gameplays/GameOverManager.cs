using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] GameObject hintGO;
        [SerializeField] float coldDuration = 3f;

        private float coldDurationTimer = 0;

        private void Awake()
        {
            hintGO.SetActive(false);
        }

        private void Update()
        {
            if (coldDurationTimer < coldDuration)
            {
                coldDurationTimer += Time.deltaTime;
                return;
            }

            if (hintGO.activeSelf == false)
            {
                hintGO.SetActive(true);
            }

            if (Input.anyKeyDown)
            {
                GameManager.instance.ChangeScene("MainMenu");
            }
        }
    }

}