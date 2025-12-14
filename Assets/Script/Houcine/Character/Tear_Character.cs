 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le comportement d'un projectile du joueur
public class Tear_Character : MonoBehaviour
{
    // La vitesse de déplacement du projectile
    public float vitesse = 10f;

    // La direction dans laquelle la balle va se déplacer
    public Vector2 direction;

    [SerializeField]
    private float tear_damage;

    void Update()
    {
        // Déplace la balle dans la direction spécifiée à la vitesse donnée
        // Time.deltaTime permet d'assurer que le déplacement est indépendant du nombre de FPS
        transform.position += (Vector3)(direction * vitesse * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Mur"))
        {
            Destroy(gameObject);
            return;
        }

            // Si la balle touche un objet avec le tag "Mur" ou "Ennemie"
            if (other.CompareTag("Ennemie"))
        {
            //  Récupère le script Ennemie_Health de l'ennemie
            var ennemie_health = other.gameObject.GetComponent<Ennemie_Health>();

                if ( ennemie_health != null)
            {
                //Applique les dégâts a l'ennemie
                ennemie_health.TakeDamgeEnnemie(tear_damage);
            }
            
            // On détruit la balle pour qu'elle disparaisse
            Destroy(gameObject);
        }

    }
}

