using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum ItemType { Health, Upgrade} // Type d’item
    public ItemType itemType;
    public int healthAmount = 1;           // PV à ajouter si c’est de la vie
    public GameObject newProjectilePrefab; // Prefab à remplacer si c’est un upgrade
    public float NewCooldownProjectile;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie que c’est le joueur
        var player = other.GetComponent<Déplacement_Character>();
        if (player != null)
        {
            // Récupère le système de vie du joueur
            var health = player.GetComponent<Health_Systeme>();

            if (itemType == ItemType.Health && health != null)
            {
                health.GetHealth(healthAmount); // ajoute les PV
            }
            else if (itemType == ItemType.Upgrade)
            {
                // Récupère le script de tir du joueur
                var playerShooter = player.GetComponent<Déplacement_Character>();
                if (playerShooter != null && newProjectilePrefab != null)
                {
                    playerShooter.projectilePrefab = newProjectilePrefab; // change le prefab des tirs
                    player.cooldownTir = NewCooldownProjectile; // change le cooldown du nouveau tir
                }
            }

            // Détruit l’item après collecte
            Destroy(gameObject);
        }
    }
}
