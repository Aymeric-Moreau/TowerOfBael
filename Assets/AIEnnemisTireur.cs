using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnnemisTireur : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;

 

    

    //  Variables pour le tir
    public GameObject projectilePrefab;
    public float cooldownTir = 0.3f;
    private float timerTir = 0f;

   


    void Update()
    {
        //if (target == null) return;

        //Vector3 direction = target.position - transform.position;
        //direction.Normalize();

        //transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        InvokeRepeating("Tirer",1,2);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke("Tirer");
    }
    //  Fonction pour tirer une balle
    void Tirer()
    {
        //Vector3 direction = target.position - transform.position;
        //  Si le cooldown n'est pas terminé, on ne peut pas tirer
        if (timerTir > 0f)
            return;

        // Instanciation du projectile à la position du joueur
        GameObject balle = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Debug.Log("tire ennemis");
        // On donne la direction à la balle
        balle.GetComponent<Tear_Character>().direction = target.position - transform.position;

        // Reset du timer de tir
        timerTir = cooldownTir;
    }

}

