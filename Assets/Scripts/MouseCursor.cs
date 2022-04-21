using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Innocence;

public class MouseCursor : MonoBehaviour
{
    [SerializeField]
    Sprite cursorSprite, exitSprite;

    [SerializeField]
    Vector2 offset;

    private Image image;

    private void Awake()
    {
        Cursor.visible = false;
        image = GetComponent<Image>();
        image.sprite = cursorSprite;
    }
    private void Update()
    {
        Vector2 cursorPos = Input.mousePosition;
        transform.position = cursorPos + offset;

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
    }
}
