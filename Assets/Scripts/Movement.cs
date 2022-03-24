using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static bool isLocked = false;

    Vector2 targetPos;
    public float speed = 2.5f;
    bool moving;
    Animator animWalk;

    void Start()
    {
        animWalk = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isLocked)
            return;

        // Debug.Log(transform.localScale);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moving = true;
            animWalk.SetBool("move", true);
            if (targetPos.x > transform.localPosition.x)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x * 1), transform.localScale.y * 1);

            }
            else
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y * 1);
            }
        }


        if (moving && (Vector2)transform.position != targetPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPos.x, transform.localPosition.y), step);
            if (transform.position.x == targetPos.x)
            {
                animWalk.SetBool("move", false);
            }
        }

        else
        {

            moving = false;

        }

    }

}






