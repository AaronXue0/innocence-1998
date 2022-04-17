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
        [SerializeField] List<int> passwords = new List<int>();
        [SerializeField] BoxCollider2D child;
        [SerializeField] int[] targetItems;

        private float closeDuration = 1, closeDurationCounter = 0;

        #region Override
        public override void GameplaySetup()
        {
        }
        public override void PuzzleSolvedCallback()
        {
            Debug.Log("Solved");
            CloseGameplay();
            BagManager.Instance.SwitchBtnActive(true);
            TimelineProp.instance.Invoke(completeProgress);
        }
        #endregion

        #region Unity
        private void Awake()
        {
            foreach (LoopScrollView loopScrollView in loopScrollViews)
            {
                loopScrollView.PasswordChangedAction = CheckPassword;
            }
        }
        private void OnEnable()
        {
            closeDurationCounter = 0;
        }
        private void Start()
        {
            GameplaySetup();
        }
        private void Update()
        {
            CheckAllItemsGot();
            if (closeDurationCounter >= closeDuration)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 32)), Vector2.zero);

                if (hit && Input.GetMouseButtonDown(0))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider == child)
                    {
                        CloseGameplay();
                    }
                }
            }
            else
            {
                closeDurationCounter += Time.deltaTime;
            }
        }
        #endregion

        public void CheckAllItemsGot()
        {
            foreach (int id in targetItems)
            {
                if (GameManager.instance.GetGameItem(id).currentState <= 0)
                {
                    return;
                }
            }

            GameManager.instance.SetItemState(12, 2);
        }

        public void CloseGameplay()
        {
            gameObject.SetActive(false);
            BagManager.Instance.SwitchBtnActive(true);
        }
        public void CheckPassword()
        {
            bool isCorrect = true;
            for (int i = 0; i < loopScrollViews.Count; i++)
            {
                if (passwords[i] == loopScrollViews[i].GetCurrentIndex())
                {
                    continue;
                }
                isCorrect = false;
                break;
            }

            if (isCorrect)
            {
                PuzzleSolved();
            }
        }
    }
}