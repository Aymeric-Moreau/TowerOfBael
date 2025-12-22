using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooter : MonoBehaviour
{
    public GameObject tearPrefab;
    public float fireRate = 1.5f; // Temps entre deux tirs en secondes
    public float bulletSpeed = 10f; // Vitesse des projectiles
    public Transform player; // Référence au joueur pour viser
    private Health_Systeme playerHealth;

    private float fireTimer; // Timer pour contrôler le délai entre les tirs
    void Start()
    {
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
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        // Vérifie que le prefab et le joueur existent
        if (tearPrefab == null || player == null) 
            return;

        // Instancie le projectile à la position de l'ennemi avec rotation nulle
        GameObject tear = Instantiate(tearPrefab, transform.position, Quaternion.identity);
        Tear_Ennemie tearScript = tear.GetComponent<Tear_Ennemie>();

        if (tearScript != null)
        {
            // Calcule la direction vers le joueur et la normalise pour que la vitesse soit constante
            Vector2 dir = player.position - transform.position;
            tearScript.direction = dir.normalized;
            tearScript.vitesse = bulletSpeed; // Définit la vitesse du projectile
        }
    }
}
