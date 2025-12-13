using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Déplacement_Character : MonoBehaviour
{
    //Variable Déplacement
    private float deplacementAxeVertical;
    private float deplacementAxeHorizontal;
    public float vitesseDeplacement = 10f;
    private Rigidbody2D rb;

    //Variable Tir
    public GameObject projectiles;
    public float cooldown = 0.5f;
    private Vector2 dir;
    [SerializeField] float next_tir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void deplacement()
    {
        Vector2 Vertical = Vector2.zero;
        Vector2 Horizontal = Vector2.zero;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
        {
           Vertical = Vector2.up  * vitesseDeplacement * Time.fixedDeltaTime;
        } else if (Input.GetKey(KeyCode.S))
        {
            Vertical = Vector2.down *
            vitesseDeplacement * Time.fixedDeltaTime;
        }

        // On calcule les deux axes correctement


        if (Input.GetKey(KeyCode.D))
        {
            Horizontal = Vector2.right * vitesseDeplacement * Time.fixedDeltaTime;
        } else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
            {
            Horizontal = Vector2.left * 
            vitesseDeplacement * Time.fixedDeltaTime;
        }


            // On additionne les deux mouvements,
            Vector2 mouvementTotal = Vertical + Horizontal;

        // Déplace le personnage
        rb.MovePosition(rb.position + mouvementTotal);

        rb.angularVelocity = 0f;

    }
    public void Tir()
    {
         dir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            dir += Vector2.up;
           
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir += Vector2.down;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector2.right;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir += Vector2.left;
        }

        if (dir != Vector2.zero && Time.time >= next_tir)

        {
            Debug.Log("Direction du tir : " + dir);

            // Instancie le projectile
            GameObject tir = Instantiate(projectiles, transform.position, Quaternion.identity);

            // Assigne la direction telle quelle 
            tir.GetComponent<Tear_Character>().direction = dir;

            // Reset cooldown
            next_tir = Time.time + cooldown;
        }


    }



    // Update is called once per frame
    void Update()
    {
        deplacementAxeVertical = Input.GetAxis("Vertical");
        deplacementAxeHorizontal = Input.GetAxis("Horizontal");

        Tir();

    }

    void FixedUpdate()
    {
        deplacement();
    }
}
