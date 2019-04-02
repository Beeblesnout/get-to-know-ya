
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVibrate : MonoBehaviour
{
    public float amount;
    
    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(Random.Range(-amount, amount), Vector3.forward);
    }
}
