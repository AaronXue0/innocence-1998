using UnityEngine;

[CreateAssetMenu(fileName = "DialogueItem", menuName = "Archived/Dialogue", order = 1)]
public class DialogueItem : ScriptableObject
{
    [SerializeField] string[] msg;
    [SerializeField] float appaerGap = 0.1f, displaySecs = 1f, fadeoutDuration = 0.5f;
    [SerializeField] TextAnimationCallbackSelector animationSelector;

    public System.Action DoneCallback { get; set; }

    public string[] GetMsg { get { return msg; } }
    public float GetAppearGap { get { return appaerGap; } }
    public float GetDisplaySec { get { return displaySecs; } }
    public float GetFadeoutDuration { get { return fadeoutDuration; } }
    public TextAnimationCallbackSelector GetAnimationSelector { get { return animationSelector; } }
}

public enum TextAnimationCallbackSelector { None, GetItem }
