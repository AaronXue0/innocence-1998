using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;
using TMPro;
using Innocence;

public class BagManager : MonoBehaviour
{
    public static BagManager Instance { get; private set; }
    public static bool Enable { get; set; }

    [SerializeField]
    private ItemInfoList itemInfoList;

    [Space(10)]
    [SerializeField]
    private RectTransform bagCanvas;

    [Space(10)]
    [SerializeField]
    private int minItemBoxAmount;
    [SerializeField]
    private GameObject itemBox;
    [SerializeField]
    private ScrollRect bagScrollRect;
    [SerializeField]
    private Transform itemContent;
    [SerializeField]
    private RectTransform itemContentRect;

    [Space(10)]
    [Header("Check Item")]
    [SerializeField]
    private GameObject bagSwitchBtn;
    [SerializeField]
    private GameObject display;
    [SerializeField]
    private Image checkImg;
    [SerializeField]
    private Animator bagAnimator;

    // private AudioSource audioSource;
    private List<ItemBox> itemBoxs;
    private int focusIndex;

    public void SwitchBtnActive(bool state) => bagSwitchBtn.SetActive(state);
    public void OnSceneLoadeed(string scene)
    {
        switch (scene)
        {
            case "MainMenu":
                SwitchBtnActive(false);
                break;
            default:
                SwitchBtnActive(true);
                break;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Enable = true;

        itemBoxs = new List<ItemBox>();
        focusIndex = -1;

        RefreshItems();
        ShowContentBegin();
    }

    public Vector2 GetMousePosition()
    {
        return InputManager.Instance.GetMousePositionInUI(bagCanvas.sizeDelta); ;
    }

    public void OnItemPointerDown(bool enableDrag, out Vector2 pointerDownPosition)
    {
        if (enableDrag)
        {
            bagScrollRect.enabled = false;
            pointerDownPosition = GetMousePosition();
        }
        else pointerDownPosition = Vector2.zero;
    }

    public void OnItemPointerUp(bool isDragging)
    {
        bagScrollRect.enabled = true;

        if (isDragging) UnfocusItem();
    }

    public void RefreshItems()
    {
        List<int> items = GetItemData();

        for (int i = 0; i < itemBoxs.Count; i++) Destroy(itemBoxs[i].gameObject);
        itemBoxs.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            itemBoxs.Add(Instantiate(itemBox, itemContent).GetComponent<ItemBox>());
            InitializeItem(i, items[i]);
        }

        if (items.Count < minItemBoxAmount)
        {
            for (int i = 0; i < minItemBoxAmount - items.Count; i++)
            {
                itemBoxs.Add(Instantiate(itemBox, itemContent).GetComponent<ItemBox>());
                InitializeEmpty(items.Count + i);
            }
        }
    }

    private void InitializeItem(int boxIndex, int itemID)
    {
        ItemInfo itemInfo = itemInfoList.GetItemWithID(itemID);
        itemBoxs[boxIndex].Initialize(boxIndex, itemID, itemInfo.onBagSprite, itemInfo.canDrag);
    }

    private void InitializeEmpty(int boxIndex)
    {
        itemBoxs[boxIndex].SetEmpty();
    }

    private void ShowContentBegin()
    {
        itemContentRect.anchoredPosition = new Vector2(0, itemContentRect.anchoredPosition.y);
    }

    private void ShowContentLast()
    {
        itemContentRect.anchoredPosition = new Vector2(-itemContentRect.sizeDelta.x - bagCanvas.sizeDelta.x, itemContentRect.anchoredPosition.y);
    }

    public void OnClick_Item(int index)
    {
        if (!Enable) return;

        if (index == focusIndex)
            UnfocusItem();
        else
        {
            // ShowCheckButton();
            if (focusIndex != -1) itemBoxs[focusIndex].SetUnfocus();
            FocusItem(index);
            CheckItem();
        }
    }

    public void FocusItem(int index)
    {
        Debug.Log("Focus: " + index);
        itemBoxs[index].SetFocus();
        focusIndex = index;
    }

    public void UnfocusItem()
    {
        if (focusIndex != -1)
        {
            Debug.Log("Unfocus: " + focusIndex);
            HideCheckButton();
            itemBoxs[focusIndex].SetUnfocus();
            focusIndex = -1;
        }
    }

    //API
    public void CheckItem()
    {
        ActiveDisplayer(GetFocusedItemID());
    }
    public void CheckItem(int itemID)
    {
        ActiveDisplayer(itemID);
    }
    private void ActiveDisplayer(int id)
    {
        bagAnimator.SetTrigger("close");
        ItemInfo info = itemInfoList.GetItemWithID(id);
        checkImg.sprite = info.onCheckSprite;
        display.SetActive(true);
        bagSwitchBtn.SetActive(false);
    }
    public void UnCheckItem()
    {
        OnClick_Item(focusIndex);
        display.SetActive(false);
        bagSwitchBtn.SetActive(true);
    }

    public void ShowCheckButton()
    {
    }

    public void HideCheckButton()
    {
    }

    public void OnClick_CheckItem()
    {
        HideCheckButton();
    }

    public void OnClick_CloseCheckItem()
    {
        UnfocusItem();
    }

    public ItemInfo GetItem(int itemID)
    {
        return itemInfoList.GetItemWithID(itemID);
    }

    public void ObtainedItem(int itemID, bool doseCheckItem = true)
    {
        Debug.Log("Obtain");
        AudioClip clip = GetItem(itemID).onGetSound;

        if (clip != null)
        {
            GameManager.instance.PlaySFX(clip);
        }

        RefreshItems();

        if (doseCheckItem)
            CheckItem(itemID);
    }

    private IEnumerator GetItemCoroutine(int itemID)
    {
        yield return null;
    }

    public void DeleteItem(int itemID)
    {
        RefreshItems();
    }

    private List<int> GetItemData()
    {
        return itemInfoList.GetInBagItemsID;
    }

    private int GetFocusedItemID()
    {
        return GetItemData()[focusIndex];
    }
}