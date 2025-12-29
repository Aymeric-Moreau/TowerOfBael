using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère le tir automatique d'un ennemi vers le joueur (IA Shooter)
public class Enemy_Shooter : MonoBehaviour
{
    public GameObject tearPrefab;     //Prefab du projectile que l'ennemi va tirer
    public float fireRate = 1.5f;    // Temps entre deux tirs successifs (en secondes) 
    public float bulletSpeed = 10f; // Vitesse des projectiles
    public Transform player; // Référence au joueur pour viser
    private Health_Systeme playerHealth; // Référence au script de santé du joueur

    private float fireTimer; // Timer pour contrôler le délai entre les tirs
    void Start()
    {
        // Si le joueur est assigné, récupère son composant Health_Systeme
        if (player != null)
            playerHealth = player.GetComponent<Health_Systeme>();
    }
    void Update()
    {
        // Si le joueur n'existe pas ou est mort, on arrête de tirer
        if (playerHealth == null || playerHealth.CurrentHealth <= 0)
            return;

        // Incrémente le timer avec le temps écoulé depuis la dernière frame
        fireTimer += Time.deltaTime;

        // Si le timer dépasse le fireRate, on tire un projectile
        if (fireTimer >= fireRate)
        {
            Shoot();        // Appelle la fonction de tir
            fireTimer = 0f; // Réinitialise le timer
        }
    }

    void Shoot()
    {
        // Vérifie que le prefab et le joueur existent
        if (tearPrefab == null || player == null) 
            return;

        // Instancie le projectile à la position de l'ennemi avec rotation nulle
        GameObject tear = Instantiate(tearPrefab, transform.position, Quaternion.identity);

        // Récupère le script du projectile ennemi
        Tear_Ennemie tearScript = tear.GetComponent<Tear_Ennemie>();

        if (tearScript != null)
        {
            // Calcule la direction vers le joueur et la normalise pour que la vitesse soit constante
            Vector2 dir = player.position - transform.position;
            tearScript.direction = dir.normalized;

            // Définit la vitesse du projectile
            tearScript.vitesse = bulletSpeed; 
        }
    }
}
