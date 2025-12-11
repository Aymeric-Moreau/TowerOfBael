using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Déplacement_Character : MonoBehaviour
{
    private float deplacementAxeVertical;
    private float deplacementAxeHorizontal;
    public float vitesseDeplacement = 10f;
    private Rigidbody2D rb;
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
    // Update is called once per frame
    void Update()
    {
        deplacementAxeVertical = Input.GetAxis("Vertical");
        deplacementAxeHorizontal = Input.GetAxis("Horizontal");

    }

    void FixedUpdate()
    {
        deplacement();
    }
}
