using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public int Index { get; private set; }
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private BagItem item;

    //�]���Ϥ��٨S���]��SpriteAtlas�A�ҥH���γo�ӥN��
    [Header("ItemBox")]
    [SerializeField]
    private Image itemBoxImage;
    [SerializeField]
    private Sprite focusSprite;
    [SerializeField]
    private Sprite unfocusSprite;

    /*
    [Space(10)]
    public SpriteAtlasLoader spriteAtlasLoader;
    public string boxFocusSpriteName;
    public string boxUnfocusSpriteName;
    */

    public void Initialize(int index, int itemID, Sprite itemSprite, bool canDrag)
    {
        itemImage.enabled = true;

        Index = index;

        if (canDrag) item.eventName = "Item" + itemID;

        itemImage.sprite = itemSprite;
        // itemImage.SetNativeSize();

        item.enable = canDrag;
    }

    public void SetEmpty()
    {
        itemImage.enabled = false;
    }

    public void OnClick_Item()
    {
        BagManager.Instance.OnClick_Item(Index);
    }

    public void SetFocus()
    {
        //spriteAtlasLoader.SetSprite(boxFocusSpriteName);
        itemBoxImage.sprite = focusSprite;
    }

    public void SetUnfocus()
    {
        //spriteAtlasLoader.SetSprite(boxUnfocusSpriteName);
        itemBoxImage.sprite = unfocusSprite;
    }
}
