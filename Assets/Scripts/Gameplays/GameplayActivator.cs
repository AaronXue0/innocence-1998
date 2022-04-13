using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        levelGO.SetActive(true);
        BagManager.Instance.SwitchBtnActive(false);
    }
}
