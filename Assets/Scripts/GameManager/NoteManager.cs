using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance;
    [SerializeField] Image image;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
            image = GetComponent<Image>();
        }
    }

    public void RefreshItems()
    {

    }

    public void NextPage()
    {

    }


}
