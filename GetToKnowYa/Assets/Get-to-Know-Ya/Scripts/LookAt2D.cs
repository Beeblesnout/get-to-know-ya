using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt2D : MonoBehaviour
{
    public void Cursor()
    {
        Vector3 mouseCoordinates;
        float angleInRadians;
        float angleInDegrees;

        //Determine mouse coordinates
        mouseCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, 0.0f));

        //Find the angle in radians
        angleInRadians = Mathf.Atan2(mouseCoordinates.y - transform.position.y,
            mouseCoordinates.x - transform.position.x);

        //Convert angle from radians to degrees
        angleInDegrees = (180 / Mathf.PI) * angleInRadians;

        //Rotate player to face mouse coordinates
        transform.rotation = Quaternion.Euler(0, 0, angleInDegrees - 90);
    }
}
