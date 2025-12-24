using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le comportement d'un projectile spécial : le laser Brimstone
public class Brimstone_Character : MonoBehaviour
{
    public float duration = 1f; // Durée du laser
    [HideInInspector] public float damage;   // Dégâts du laser (gérée depuis le player)
    public Vector2 direction;  // Direction dans laquelle le laser se déplace

    void Start()
    {


        // Rotation du laser en fonction de sa direction
        if (direction.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);  // droite
        else if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 180); // gauche

        // Si la direction est verticale
        if (direction.y > 0)
            transform.rotation = Quaternion.Euler(0, 0, 90); // haut
        else if (direction.y < 0)
            transform.rotation = Quaternion.Euler(0, 0, -90); // bas

        // On détruit le laser après sa durée
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {// Si le laser touche un objet avec le tag  "Ennemie"
        if (other.CompareTag("Ennemie"))
        {
            //  Récupère le script Ennemie_Health de l'ennemie
            var ennemie_health = other.gameObject.GetComponent<Ennemie_Health>();

            if (ennemie_health != null)
            {
                //Applique les dégâts a l'ennemie
                ennemie_health.TakeDamgeEnnemie(damage);
            }
        }
}
}
