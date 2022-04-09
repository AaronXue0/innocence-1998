using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;

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
    
    private List<ItemBox> itemBoxs;
    private int focusIndex;

    private void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        Instance = this;
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

        if(items.Count < minItemBoxAmount)
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
        itemBoxs[boxIndex].Initialize(boxIndex, itemID, itemInfoList.items[itemID].onBagSprite, itemInfoList.items[itemID].canDrag);
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
            ShowCheckButton();
            if (focusIndex != -1) itemBoxs[focusIndex].SetUnfocus();
            FocusItem(index);
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
    
    public void GetItem(int itemID)
    {
        
    }
    
    public void DeleteItem(int itemID)
    {
        
    }

    private List<int> GetItemData()
    {
        return new List<int>() { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
    }

    private int GetFocusedItemID()
    {
        return GetItemData()[focusIndex];
    }
}