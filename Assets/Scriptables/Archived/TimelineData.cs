using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "TeimelineData", menuName = "Archived/Timeline", order = 0)]
public class TimelineData : ScriptableObject
{
    public TimelineAsset asset;
    public TimelineCondition condition;
}

public enum TimelineCondition { Awake, CertainCondition }
