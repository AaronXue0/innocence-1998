using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class IGameplay : MonoBehaviour
    {
        public int id;
        public int completeProgress;
        public GampelaySetItems[] setItems;
        public GampelaySetLights[] setLights;
        protected bool isSolved = false;
        public int completeObjectState;
        public virtual void PuzzleSolved()
        {
            StartCoroutine(PuzzleSolvedCoroutine(PuzzleSolvedCallback));
            StartCoroutine(PuzzleSolvedChangeStates());
        }
        public virtual void GameplaySetup() { }
        public virtual void PuzzleSolvedCallback() { }
        public virtual bool IsComplete { get { return GameManager.instance.IsItemComplete(id); } }

        public IEnumerator PuzzleSolvedChangeStates()
        {
            if (GameManager.instance)
            {
                foreach (GampelaySetItems item in setItems)
                {
                    GameManager.instance.SetItemState(item.id, item.state);
                }

                yield return null;

                foreach (GampelaySetLights light in setLights)
                {
                    GameManager.instance.SetLightState(light.id, light.state);
                }
            }
        }

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

    [System.Serializable]
    public class GampelaySetItems
    {
        public int id;
        public int state;
    }
    [System.Serializable]
    public class GampelaySetLights
    {
        public int id;
        public int state;
    }
}