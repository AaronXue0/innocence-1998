using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject levelGO;

    private void OnMouseDown()
    {
        Debug.Log("aosfpjsop");
        levelGO.SetActive(true);
        BagManager.Instance.SwitchBtnActive(false);
    }
}
