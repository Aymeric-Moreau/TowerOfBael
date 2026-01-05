 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le comportement d'un projectile "larme par default" du joueur
public class Tear_Character : MonoBehaviour
{
    // La vitesse de déplacement du projectile
    public float vitesse = 10f; 
    public Vector2 direction; // Direction dans laquelle la larme par défault  se déplace
    [HideInInspector] public float tear_damage; // Dégâts de larme par défault (gérée depuis le player)


    void Start()
    {
        // Afficher la bonne orientaion de la larme par défault
        // On récupère le transform pour rotation
        Vector3 rotation = Vector3.zero;

        // Permet de déplacer la larme à vitesse constante dans toutes les directions
        direction = direction.normalized;


        // Tir vertical
        if (direction.y > 0 && direction.x == 0) rotation.z = 180;      // Haut
        else if (direction.y < 0 && direction.x == 0) rotation.z = 0; // Bas

        // Tir horizontal
        else if (direction.x > 0 && direction.y == 0) rotation.z = -270; // Droite
        else if (direction.x < 0 && direction.y == 0) rotation.z = -90;  // Gauche

        // Diagonales
        else if (direction.x > 0 && direction.y > 0) rotation.z = 135;   // Haut-droite
        else if (direction.x < 0 && direction.y > 0) rotation.z = 225;    // Haut-gauche
        else if (direction.x > 0 && direction.y < 0) rotation.z = 45;   // Bas-droite
        else if (direction.x < 0 && direction.y < 0) rotation.z = -45;   // Bas-gauche

        // Applique la rotation
        transform.rotation = Quaternion.Euler(rotation);
    }
    void Update()
    {
        // Déplace la larme dans la direction spécifiée à la vitesse donnée
        // Time.deltaTime permet d'assurer que le déplacement est indépendant du nombre de FPS
        transform.position += (Vector3)(direction * vitesse * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si la larme touche un objet avec le tag "Mur" 
        if (other.CompareTag("Mur"))
        {
            //Détruit La larme
            Destroy(gameObject);
            return;
        }

        // Si la larme touche un objet avec le tag "Obstacle" 
        if (other.CompareTag("Obstacle"))
        {
            //Détruit La larme
            Destroy(gameObject);
            return;
        }
        // Si la larme touche un objet avec le tag  "Ennemie"
        if (other.CompareTag("Ennemie"))
        {
            //  Récupère le script Ennemie_Health de l'ennemie
            var ennemie_health = other.gameObject.GetComponent<Ennemie_Health>();

                if ( ennemie_health != null)
            {
                //Applique les dégâts a l'ennemie
                ennemie_health.TakeDamgeEnnemie(tear_damage);
            }
            
            // On détruit la larme pour qu'elle disparaisse
            Destroy(gameObject);
        }

    }
}

