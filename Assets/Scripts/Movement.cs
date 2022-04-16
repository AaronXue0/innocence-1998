using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Innocence
{
    public class Movement : MonoBehaviour
    {
        public static Movement instance;
        public static bool isLocked = false;

        Vector2 targetPos;
        public float speed = 2.5f;
        bool moving;
        Animator animWalk;

        private PlayerData playerData;
        private bool isClickOnUI = false, isPressing = false;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }

            instance = this;

            animWalk = GetComponent<Animator>();

            playerData = GameManager.instance.GetPlayerData();

            if (playerData.lastAnimator != "")
                animWalk.SetTrigger(playerData.lastAnimator);
        }

        private void OnEnable()
        {
            // SetPosition(GameManager.instance.GetPlayerPos());
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetTrigger(string name)
        {
            playerData.lastAnimator = name;
            animWalk.SetTrigger(name);
        }

        public void StopMovingInPos()
        {
            targetPos = transform.position;
            animWalk.SetBool("move", false);
            moving = false;
        }


        /// <summary>
        /// Call by singnals
        /// </summary>
        /// <param name="state"></param>
        public void StopMoving(bool state)
        {
            isLocked = state;
            if (isLocked)
            {
                targetPos = transform.position;
            }
        }

        private void Update()
        {
            if (isLocked)
            {
                if (moving)
                {
                    animWalk.SetBool("move", false);
                    targetPos = transform.position;
                }
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    isClickOnUI = true;
                    animWalk.SetBool("move", false);
                    targetPos = transform.position;
                    moving = false;
                    return;
                }
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
}