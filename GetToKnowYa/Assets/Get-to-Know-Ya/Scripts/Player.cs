using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    // -= Variables =-
    [Header("Movement")]
    public Queue<Vector2> linePoints = new Queue<Vector2>();
    public float minDrawDist;
    public int maxPoints = 10;
    public float moveSpeed = 1, minMovePointDist = .1f;
    LineRenderer line;
    Vector2 mousePos;
    bool isDrawing, isMoving;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float shotRate;
    ParticleSystem gunFireEffect;
    float lastShotTime;

    // -= Basic Methods =-
    void Start() {
        line = GetComponent<LineRenderer>();
        gunFireEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public int drawnPoints;
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        DrawLine();
    }

    void FixedUpdate() {
        if (Input.GetMouseButton(0) && !isDrawing)
        {
            Vector2 localMouse = mousePos - (Vector2)transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(localMouse.y, localMouse.x) * Mathf.Rad2Deg, Vector3.forward);

            DoShot();
        }

        MoveOnLine();
    }

    // -= Helper Methods =-
    void DoShot()
    {
        if (Time.time - lastShotTime > shotRate)
        {
            lastShotTime = Time.time;
            Rigidbody2D rb2D = 
                Instantiate(bulletPrefab, transform.position + (transform.right * .5f), transform.rotation)
                    .GetComponent<Rigidbody2D>();
            rb2D.AddForce(transform.right * 2.5f, ForceMode2D.Impulse);
            gunFireEffect.Emit(7);
        }
    }
    
    void DrawLine()
    {
        if (isDrawing && drawnPoints < maxPoints && drawnPoints > 0
         && Vector2.Distance(linePoints.Last(), mousePos) > minDrawDist)
        {
            linePoints.Enqueue(mousePos);
            drawnPoints++;
        }

        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.Reverse().Select(p => (Vector3)p).ToArray());
    }

    void MoveOnLine()
    {
        if (linePoints.Count > 0)
        {
            Vector2 dir = new Vector2();
            isMoving = true;
            if (linePoints.Count > 1)
            {
                if (Vector2.Distance(transform.position, linePoints.ToList()[1]) < minMovePointDist)
                {
                    if (linePoints.Count > 1)
                        linePoints.Dequeue();
                }
                else
                {
                    dir = (linePoints.ToList()[1] - (Vector2)transform.position).normalized;
                }
            }
            
            transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
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
