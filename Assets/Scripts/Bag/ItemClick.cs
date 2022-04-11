using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClick : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            BagManager.Instance.UnCheckItem();
        }
    }
}
