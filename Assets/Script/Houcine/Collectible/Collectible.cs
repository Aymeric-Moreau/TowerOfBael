using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum ItemType { Health, NouvelleLarme, damageBonus, cooldownReduction , speedBonus } // Type d’item
    public ItemType itemType;
    public int healthAmount = 1;           // PV à ajouter si c’est de la vie
    public float damageBonus = 1f;         // Bonus dégâts (Tear + Brimstone)
    public float cooldownReduction = 0.1f; // Réduction cooldown larme normale
    public float speedBonus = 1f;          // Bonus vitesse déplacement




    public GameObject newProjectilePrefab; // Prefab à remplacer si c’est un upgrade
    [HideInInspector] public float NewCooldownProjectile; // Cooldown du laser (gérée depuis le player)

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            // On récupère le script Déplacement_Character du joueur qui a touché l'objet
            var player = other.GetComponent<Déplacement_Character>();
            if (player == null) return; // Si ce n'est pas le joueur, on quitte la fonction

            // On récupère le système de vie du joueur
            var health = player.GetComponent<Health_Systeme>();

            // SWITCH : permet de tester la valeur d'une variable et d'exécuter
            // différents blocs de code selon cette valeur.
            switch (itemType)
            {
                // CASE : correspond à un "cas" spécifique dans le switch. 
                // Si itemType == ItemType.Health, alors ce bloc s'exécute.
                case ItemType.Health:

                    // Si c'est un item de type santé et que le joueur a un système de vie
                    if (health != null) health.GetHealth(healthAmount); // On ajoute les points de vie
                    break;

                case ItemType.NouvelleLarme:
                    // Si le joueur existe et qu'on a un prefab de projectile à remplacer
                    if (player != null && newProjectilePrefab != null)
                    {
                        player.projectilePrefab = newProjectilePrefab; // On change le prefab de tir du joueur par celui du collectible

                        // On met à jour le cooldown du Brimstone directement sur le joueur
                        // Ce cooldown sera utilisé quand le joueur tirera avec le nouveau projectile
                        player.cooldownBrimstone = NewCooldownProjectile;
                    }
                    break;
            }

            // Détruit l’item après collecte
            Destroy(gameObject);
        }
    }
}
