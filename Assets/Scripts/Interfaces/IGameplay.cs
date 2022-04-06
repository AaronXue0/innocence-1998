using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class IGameplay : MonoBehaviour
    {
        public int id;
        public int completeProgress;
        protected bool isSolved = false;
        public int completeObjectState;
        public virtual void PuzzleSolved() { }
        public virtual void GameplaySetup() { }
        public virtual bool IsComplete { get { return GameManager.instance.IsItemComplete(id); } }

        public IEnumerator PuzzleSolvedCoroutine(System.Action callback)
        {
            if (completeProgress != -1)
                GameManager.instance.SetProgress(completeProgress);

            GameManager.instance.SetItemComplete(id);

            if (completeObjectState != -1)
                GameManager.instance.SetItemState(id, completeObjectState);

            yield return null;

            if (callback != null)
                callback();
        }
    }
}