using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Innocence
{
    public class SceneTransition : MonoBehaviour
    {
        [HideInInspector] public bool isSceneFading;
        public GameObject screenTransitionPanel;
        public float duration = 0.25f;

        private Image mask;
        private List<string> openedAdditiveScene = new List<string>();

        AsyncOperation closeupSceneAsync;
        Scene mainScene;

        private void Awake()
        {
            mask = screenTransitionPanel.GetComponent<Image>();

        }
        public void ChangeScene(string sceneName)
        {
            Fade(() => SceneManager.LoadScene(sceneName), delegate { });
        }
        public void ChangeSceneAdditive(string mainScene, string closeupScene)
        {
            Fade(() => LoadSceneAdditive(mainScene, closeupScene), delegate { });
        }

        #region LoadSceneAdditive
        private void LoadSceneAdditive(string mainScene, string closeupScene)
        {
            StartCoroutine(WaitForFade(mainScene, closeupScene));
        }
        IEnumerator WaitForFade(string mainScene, string closeupScene)
        {
            this.mainScene = SceneManager.GetSceneByName(mainScene);

            yield return null;

            //Begin to load the Scene you specify
            this.closeupSceneAsync = SceneManager.LoadSceneAsync(closeupScene);
            closeupSceneAsync.allowSceneActivation = false;
            while (!closeupSceneAsync.isDone)
            {
                if (closeupSceneAsync.progress >= 0.9f)
                {
                    closeupSceneAsync.allowSceneActivation = true;
                }

                yield return null;
            }
        }
        #endregion


        #region LoadSceneWithFading
        private void Fade(Action midCallBack, Action endCallBack)
        {
            StartCoroutine(WaitForFade(midCallBack, endCallBack));
        }
        IEnumerator WaitForFade(Action midCallBack, Action endCallBack)
        {
            isSceneFading = true;
            screenTransitionPanel.SetActive(true);
            mask.color = new Color(0, 0, 0, 0);

            while (mask.color.a < 1)
            {
                mask.color += new Color(0, 0, 0, 0.01f / duration);
                yield return new WaitForSeconds(0.01f);
            }
            mask.color = new Color(0, 0, 0, 1);

            midCallBack();
            GC.Collect();

            while (mask.color.a > 0)
            {
                mask.color -= new Color(0, 0, 0, 0.01f / duration);
                yield return new WaitForSeconds(0.01f);
            }
            mask.color = new Color(0, 0, 0, 0);

            endCallBack();

            screenTransitionPanel.SetActive(false);
            isSceneFading = false;
        }
        #endregion
    }
}