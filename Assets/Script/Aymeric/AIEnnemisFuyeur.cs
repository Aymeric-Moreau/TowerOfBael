using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class AIEnnemisFuyeur : MonoBehaviour 
{
    public Transform target;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (target.position.x - transform.position.x < 7 && target.position.y - transform.position.y < 7)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position -= direction * speed * Time.deltaTime;
        }
        

        
    }
}
