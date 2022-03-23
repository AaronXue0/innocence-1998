using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int chapter;
    public List<int> progressStates;
    public List<int> objectStates;
    public List<int> eventStates;

    public List<int> items = new List<int>();

    public int language;     // 0: Chinese, 1: English
    public bool bgmSwitch, seSwitch;
    public int getHintTimes, currentHintID;

    public GameData()
    {
        chapter = 1;
        progressStates = new List<int>();
        objectStates = new List<int>();
        eventStates = new List<int>();

        items = new List<int>();

        language = 0;
        bgmSwitch = true;
        seSwitch = true;

        getHintTimes = 0;
        currentHintID = -1;
    }
}