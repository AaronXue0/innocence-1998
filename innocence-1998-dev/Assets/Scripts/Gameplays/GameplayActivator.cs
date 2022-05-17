using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject levelGO;

    private void Awake()
    {
        levelGO.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        levelGO.SetActive(true);
        BagManager.Instance.SwitchBtnActive(false);
    }
}
