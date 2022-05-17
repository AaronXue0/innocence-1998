using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClick : MonoBehaviour
{
    float coldDuration = 0.5f;
    float coldDurationCounter = 0;
    private void OnEnable()
    {
        coldDurationCounter = 0;
    }
    private void OnDisable()
    {
        coldDurationCounter = 0;
    }
    private void Update()
    {
        if (coldDurationCounter >= coldDuration)
        {

            if (Input.GetMouseButton(0))
            {
                BagManager.Instance.UnCheckItem();
            }
        }
        else
        {
            coldDurationCounter += Time.deltaTime;
        }
    }
}
