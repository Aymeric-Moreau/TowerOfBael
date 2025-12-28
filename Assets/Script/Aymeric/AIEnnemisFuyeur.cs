using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class AIEnnemisFuyeur : MonoBehaviour 
{
    public Transform target;
    public float speed = 1f;
    // Start is called before the first frame update

    private Animator anim;
    private bool IsMovingAnim;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (target == null) return;
        //if (target.position.x - transform.position.x < 7 && target.position.y - transform.position.y < 7)
        //{
        //    Vector3 direction = new Vector3(target.position.x - transform.position.x,target.position.y - transform.position.y,transform.position.z);
        //    direction.Normalize();

        //    //transform.position -= direction * speed * Time.deltaTime;
        //    rb.velocity = direction * speed;

        //    // L’ennemi bouge → Walk
        //    IsMovingAnim = true;
        //}
        //else
        //{
        //    // Ennemi trop loin → Idle
        //    IsMovingAnim = false;
        //}
        //// On applique au Animator
        //anim.SetBool("IsMoving", IsMovingAnim);


    }

    private void FixedUpdate()
    {
        if (target == null) return;
        if (target.position.x - transform.position.x < 7 && target.position.y - transform.position.y < 7)
        {
            Vector3 direction = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, transform.position.z);
            direction.Normalize();

            //transform.position -= direction * speed * Time.deltaTime;
            rb.velocity = - direction * speed;

            // L’ennemi bouge → Walk
            IsMovingAnim = true;
        }
        else
        {
            rb.velocity = Vector3.zero;
            // Ennemi trop loin → Idle
            IsMovingAnim = false;
        }
        // On applique au Animator
        anim.SetBool("IsMoving", IsMovingAnim);
    }
}
