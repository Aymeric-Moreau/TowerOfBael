using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le comportement d'un projectile tiré par un ennemi

public class Tear_Ennemie : MonoBehaviour
{
    // La vitesse de déplacement du projectile
    public float vitesse = 10f;

    // La direction dans laquelle le projectile va se déplacer
    public Vector2 direction;

    // Dégâts que ce projectile inflige au joueur 
    [SerializeField] private float tear_damage;

    void Update()
    {
        // Déplace le projectile dans la direction spécifiée à la vitesse donnée
        transform.position += (Vector3)(direction * vitesse * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si le projectile touche un objet avec le tag "Mur" 
        if (other.CompareTag("Mur"))
        {
            //Détruit le projectile
            Destroy(gameObject);
            return;
        }

        // Si le projectile touche un objet avec le tag "Obstacle" 
        if (other.CompareTag("Obstacle"))
        {
            //Détruit le projectile
            Destroy(gameObject);
            return;
        }

        // Si le projectile touche un objet avec le tag  "Player"
        if (other.CompareTag("Player"))
        {
            //  Récupère le script Health_Systeme du joueur
            var player_health = other.GetComponent<Health_Systeme>();

            if (player_health != null)
            {
                //Applique les dégâts au joueur 
                player_health.TakeDamge(tear_damage);
            }

            // Détruit le projectile après avoir infligé les dégâts
            Destroy(gameObject);
        }
    }
}
