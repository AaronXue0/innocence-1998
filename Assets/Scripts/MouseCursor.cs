using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    [SerializeField]
    Sprite cursorSprite;

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
    }
}
