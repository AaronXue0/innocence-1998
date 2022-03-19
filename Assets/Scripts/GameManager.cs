using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        TextPlayer textPlayer;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                textPlayer = GetComponent<TextPlayer>();
            }
        }

        public void PlayText(DialogueItem item) => textPlayer.Play(item);
    }
}