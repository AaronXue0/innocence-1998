using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item/Item Info")]
public class ItemInfo : ScriptableObject
{
    public int id;
    public bool canDrag;
    public Sprite onBagSprite;
    public Sprite onCheckSprite;
    public Sprite onGetSprite;
}