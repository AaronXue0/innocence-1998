using UnityEngine;

[CreateAssetMenu(fileName = "Dialogues", menuName = "Innocene/Dialogues", order = 1)]
public class Dialogues : ScriptableObject
{
    [SerializeField] string[] msg;
    [SerializeField] float appaerGap = 0.1f, displaySecs = 1f, fadeoutDuration = 0.5f;
    [SerializeField] TextAnimationCallbackSelector animationSelector;

    public string[] GetMsg { get { return msg; } }
    public float GetAppearGap { get { return appaerGap; } }
    public float GetDisplaySec { get { return displaySecs; } }
    public float GetFadeoutDuration { get { return fadeoutDuration; } }
    // public System.Action<Innocence.FinishedResult> DoneCallback { get; set; }
}
