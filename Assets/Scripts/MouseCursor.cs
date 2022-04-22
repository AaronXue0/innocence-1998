using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Innocence;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor Instance = null;
    [SerializeField]
    Sprite cursorSprite, exitSprite;

    [SerializeField]
    Vector2 offset;

    private Image image;

    private bool isOnUI = false;

    public void OnSceneLoaded(string name)
    {
        switch (name)
        {
            case "EVScene":
            case "PVScene":
                image.enabled = false;
                break;
            default:
                image.enabled = true;
                break;
        }
    }
    public void OnUIEnter(Sprite sprite)
    {
        image.sprite = sprite;
        isOnUI = true;
    }
    public void OnUIExit()
    {
        image.sprite = cursorSprite;
        isOnUI = false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Cursor.visible = false;
            image = GetComponent<Image>();
            image.sprite = cursorSprite;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Cursor.visible == true)
            Cursor.visible = false;

        Vector2 cursorPos = Input.mousePosition;
        transform.position = cursorPos + offset;

        if (isOnUI)
        {
            return;
        }

        try
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                image.sprite = cursorSprite;
                return;
            }
        }
        catch
        {

        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
        if (hit)
        {
            switch (hit.collider.tag)
            {
                case "GameItem":
                    image.sprite = hit.collider.GetComponent<ItemProp>().GetHintSprite;
                    break;
                case "Exit":
                    image.sprite = exitSprite;
                    break;
                default:
                    image.sprite = cursorSprite;
                    break;
            }
        }
        else if (image.sprite != cursorSprite)
        {
            image.sprite = cursorSprite;
        }
    }
}
