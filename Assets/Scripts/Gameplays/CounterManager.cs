using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class CounterManager : IGameplay
    {
        [SerializeField] int[] targetItems;

        public override void GameplaySetup()
        {
        }
        public override void PuzzleSolvedCallback()
        {
        }

        private void Update()
        {
            if (isSolved == false)
                CheckForTargets();
        }

        public void CheckForTargets()
        {
            bool isComplete = true;
            foreach (int id in targetItems)
            {
                ItemContent content = GameManager.instance.GetItemContent(id);
                if (content.completed)
                    continue;

                isComplete = false;
                break;
            }

            if (isComplete)
            {
                PuzzleSolved();
            }
        }
    }
}