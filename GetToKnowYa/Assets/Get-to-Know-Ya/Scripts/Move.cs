using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //To Finger Variables
    private int clicks = 0;
    private float clickTime = 0;
    private float clickDelay = 0.5f;
    private bool doubleClick = false;
    public bool dashing = false;
    private Vector3 dashTarget;

    public void ToCursor(float velocity, float reducedInitialVelocity, bool fixedVelocity)
    {
        Vector3 moveTarget;
        float initialVelocity;

        //Set target vector equal to the x and y components of the mouse cursor within the game screen
        moveTarget = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        if (Input.GetMouseButton(0))
        {
            initialVelocity = reducedInitialVelocity;
        }
        else
        {
            initialVelocity = velocity;
        }

        //Drone position is increased by the difference between the mouse position and the drone position over time in 
        //seconds
        //The acceleration rate determines how fast initially the drone travels to the mouse position
        if (fixedVelocity == false)
        {
            transform.position += (moveTarget - transform.position) * initialVelocity * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.Normalize(moveTarget - transform.position) * velocity * Time.deltaTime;
        }
    }

    public void ToFinger(float fastVelocity, float slowVelocity, bool fixedVelocity)
    {
        Vector3 moveTarget;        
        float initialVelocity;

        //Set target vector equal to the x and y components of the mouse cursor within the game screen
        if (dashing == false)
        {
            moveTarget = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        }
        else
        {
            moveTarget = dashTarget;
        }

        if (Input.GetMouseButtonDown(0))
        {
            clicks++;
            if (clicks == 1)
            {
                clickTime = Time.time;
            }
        }
        if (clicks > 1 && Time.time - clickTime <= clickDelay)
        {
            clicks = 0;
            clickTime = 0;
            doubleClick = true;
            Debug.Log("Double clicked!");
        }
        else if (clicks > 2 || Time.time - clickTime > clickDelay)
        {
            clicks = 0;
            doubleClick = false;
        }

        if (doubleClick == true)
        {
            dashing = true;
            dashTarget = moveTarget;
        }

        if (dashing == true)
        {
            initialVelocity = fastVelocity;
        }
        else
        {
            initialVelocity = slowVelocity;
        }

        //Drone position is increased by the difference between the mouse position and the drone position over time in 
        //seconds
        //The acceleration rate determines how fast initially the drone travels to the mouse position
        if (fixedVelocity == false)
        {
            transform.position += (moveTarget - transform.position) * initialVelocity * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.Normalize(moveTarget - transform.position) * fastVelocity * Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, moveTarget) <= 0.8f)
        {
            dashing = false;
        }
    }
}
