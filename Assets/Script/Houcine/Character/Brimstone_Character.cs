using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le comportement d'un projectile spécial : le laser Brimstone
public class Brimstone_Character : MonoBehaviour
{
    public float duration = 1f; // Durée du laser
    [HideInInspector] public float damage;   // Dégâts du laser (gérée depuis le player)
    public Vector2 direction;  // Direction dans laquelle le laser se déplace

    public float offsetDistanceGauche = 0.5f; // Décalage du laser Gauche par rapport au joueur
    public float offsetDistanceDroite = 0.5f; // Décalage du laser Droit par rapport au joueur
    public float offsetDistanceHaut = 0.5f; // Décalage du laser Haut par rapport au joueur
    public float offsetDistanceBas = 0.5f; // Décalage du laser Bas par rapport au joueur

    void Start()
    {
        // Normalise la direction
        direction = direction.normalized;

        // Détermine le décalage du laser pour qu'il commence à l'extérieur du joueur
        Vector3 offset = Vector3.zero;

        if (direction.y > 0.1f) offset = Vector3.up * offsetDistanceHaut;       // Haut
        else if (direction.y < -0.1f) offset = Vector3.down * offsetDistanceBas; // Bas
        else if (direction.x > 0.1f) offset = Vector3.right * offsetDistanceDroite; // Droite
        else if (direction.x < -0.1f) offset = Vector3.left * offsetDistanceGauche; // Gauche

        // Applique le décalage pour que le laser commence à l'extérieur du joueur
        transform.position += offset;

        // Calcul de la rotation pour que le laser pointe correctement
        Vector3 rotation = Vector3.zero;

        if (direction.y > 0.1f) rotation.z = 180;       // Haut
        else if (direction.y < -0.1f) rotation.z = 0;   // Bas
        else if (direction.x > 0.1f) rotation.z = 90;  // Droite
        else if (direction.x < -0.1f) rotation.z = -90;  // Gauche

        transform.rotation = Quaternion.Euler(rotation);
        

        // On détruit le laser après sa durée
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
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
