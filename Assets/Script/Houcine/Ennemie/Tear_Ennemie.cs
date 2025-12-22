using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le comportement d'un projectile de l'ennemi

public class Tear_Ennemie : MonoBehaviour
{
    // La vitesse de déplacement du projectile
    public float vitesse = 10f;
    // La direction dans laquelle la balle va se déplacer
    public Vector2 direction;
    [SerializeField] private float tear_damage;

    void Update()
    {
        // Déplace la balle dans la direction spécifiée à la vitesse donnée
        transform.position += (Vector3)(direction * vitesse * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si la balle touche un objet avec le tag "Mur" 
        if (other.CompareTag("Mur"))
        {
            //Détruit La balle
            Destroy(gameObject);
            return;
        }

        // Si la balle touche un objet avec le tag "Obstacle" 
        if (other.CompareTag("Obstacle"))
        {
            //Détruit La balle
            Destroy(gameObject);
            return;
        }

        // Si la balle touche un objet avec le tag  "Player"
        if (other.CompareTag("Player"))
        {
            //  Récupère le script Health_Systeme du joueur
            var player_health = other.GetComponent<Health_Systeme>();

            if (player_health != null)
            {
                //Applique les dégâts au joueur 
                player_health.TakeDamge(tear_damage);
            }

            // On détruit la balle pour qu'elle disparaisse
            Destroy(gameObject);
        }
    }
}
