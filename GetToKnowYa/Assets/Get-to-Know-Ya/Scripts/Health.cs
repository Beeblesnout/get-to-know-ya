using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoints;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoints <= 0)
        {
            OnDeath();
        }
    }

    public float Damage()
    {
        return 0.0f;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
