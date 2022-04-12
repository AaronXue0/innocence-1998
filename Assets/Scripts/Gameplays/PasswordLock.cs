using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace Innocence
{
    public class PasswordLock : IGameplay
    {
        [Header("Game Attrs")]
        [SerializeField] List<LoopScrollView> loopScrollViews = new List<LoopScrollView>();
        [SerializeField] BoxCollider2D child;

        private float closeDuration = 1, closeDurationCounter = 0;

        #region Override
        public override void GameplaySetup()
        {
        }
        public override void PuzzleSolvedCallback()
        {
        }
        #endregion

        #region Unity
        private void OnEnable()
        {
            closeDurationCounter = 0;
        }
        public void Update()
        {
            if (closeDurationCounter >= closeDuration)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 32)), Vector2.zero);
                if (hit && Input.GetMouseButtonDown(0))
                {
                    if (hit.collider == child)
                    {
                        gameObject.SetActive(false);
                        BagManager.Instance.SwitchBtnActive(true);
                    }
                }
            }
            else
            {
                closeDurationCounter += Time.deltaTime;
            }
        }
        #endregion
    }
}