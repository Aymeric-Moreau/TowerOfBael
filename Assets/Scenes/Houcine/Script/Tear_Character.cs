using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear_Character : MonoBehaviour
{
    public float speedTear;
    public Vector2 direction = Vector2.up;
   
    void Start()
    {


        Destroy(gameObject, 5f);
        ;

    }



    void Update()
    {
        transform.position += (Vector3)(direction * speedTear * Time.deltaTime);


    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
