using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour
{
    private Transform centerTarget;
    private Vector2 swipPosStart, swipPosEnd, currentSwipe;
    private float touchDistance;
    private Vector3 initialScale;
    private Touch touchLeft, touchRight;
    void Start()
    {
        centerTarget = gameObject.transform;
    }
    void Update()
    {
        ScaleScene();
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                swipPosStart = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                swipPosEnd = touch.position;
                Swipe();
            }
        }
    }
    public void Swipe()
    {
        if (SpawnDragon.swipe)
        {
            currentSwipe = swipPosEnd - swipPosStart;
            currentSwipe.Normalize();

            if (currentSwipe.x < 0 && currentSwipe.y < 0.5f && currentSwipe.y > -0.5f)  //left
            {
                centerTarget.RotateAround(centerTarget.position, -Vector3.up, 60 * Time.deltaTime);
            } 
            if (currentSwipe.x > 0 && currentSwipe.y < 0.5f && currentSwipe.y > -0.5f)  //right
            {
               centerTarget.RotateAround(centerTarget.position, Vector3.up, 60 * Time.deltaTime);
            }
            if (currentSwipe.y < 0 && currentSwipe.x < 0.5f && currentSwipe.x > -0.5f)  //up 
            {
                if (centerTarget.position.y < 5f)
                {
                    centerTarget.position += new Vector3(0f, 10 * Time.deltaTime, 0f);
                }
            }
            if (currentSwipe.y > 0 && currentSwipe.x < 0.5f && currentSwipe.x > -0.5f) // down
            {
                if (centerTarget.position.y > -0.5f)
                {
                    centerTarget.position -= new Vector3(0f, 10 * Time.deltaTime, 0f);
                }
            }
            swipPosStart = swipPosEnd;
        }
    }
    public void ScaleScene()
    {
      //  centerTarget = gameObject.transform;
        if (Input.touchCount > 0)
        {
            if (Input.touches.Length == 2)
            {
                touchLeft = Input.GetTouch(0);
                touchRight = Input.GetTouch(1);

                if (touchLeft.phase == TouchPhase.Began || touchRight.phase == TouchPhase.Began)
                {
                    touchDistance = Vector2.Distance(touchLeft.position, touchRight.position);
                    initialScale = transform.localScale;
                }
                else if (touchLeft.phase == TouchPhase.Moved || touchRight.phase == TouchPhase.Moved)
                {
                    var currentTouchDistance = Vector2.Distance(touchLeft.position, touchRight.position);
                    var scaleFactor = currentTouchDistance / touchDistance;
                    centerTarget.localScale = initialScale / scaleFactor; 
                }
            }
        }
    }
    public void ExitDragon()
    {
        Application.Quit();
    }
}
