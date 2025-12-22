using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnnemisSuiveur : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;
    }
}

