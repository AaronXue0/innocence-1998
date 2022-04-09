using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item/Item Info")]
public class ItemInfo : ScriptableObject
{
    public bool canDrag;
    public Sprite onBagSprite;
    public Sprite onCheckSprite;
    public Sprite onGetSprite;
}