using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public Queue<Vector2> linePoints = new Queue<Vector2>();
    public float distThreshold;
    public int maxPoints = 10;
    LineRenderer line;
    Vector2 mousePos;
    public bool isDrawing, isMoving;

    void Start() {
        line = GetComponent<LineRenderer>();
    }

    public int drawnPoints;
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);

        if (isDrawing) /*if so..then check*/ if (drawnPoints < maxPoints && drawnPoints > 0) /*if so..then check*/ 
        if (Vector3.Distance(linePoints.Last(), mousePos) > distThreshold)
        {
            print("drawn point");
            linePoints.Enqueue(mousePos);
            drawnPoints++;
        }

        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.Select(p => (Vector3)p).ToArray());
    }

    public float moveSpeed = 1, posThresh = .01f;
    void FixedUpdate() {
        if (Input.GetMouseButton(0) && !isDrawing)
        {
            Vector2 localMouse = mousePos - (Vector2)transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(localMouse.y, localMouse.x) * Mathf.Rad2Deg, Vector3.forward);

            //shoot periodically
        }

        if (linePoints.Count > 0)
        {
            Vector3 dir = new Vector3();
            isMoving = true;
            if (linePoints.Count > 1) /*if so..then check*/
            if (Vector3.Distance(transform.position, linePoints.ToList()[1]) < posThresh)
            {
                if (linePoints.Count > 1)
                    linePoints.Dequeue();
            }
            else
            {
                dir = (linePoints.ToList()[1] - (Vector2)transform.position).normalized;
            }
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    void OnMouseDown() {
        linePoints.Clear();
        line.positionCount = 0;
        linePoints.Enqueue(transform.position);
        drawnPoints = 1;
        isDrawing = true;
    }

    void OnMouseUp() {
        isDrawing = false;
    }
}
