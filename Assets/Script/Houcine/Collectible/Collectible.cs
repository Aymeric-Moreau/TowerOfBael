using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum ItemType { Health, NouvelleLarme, damageBonus, cooldownReduction , speedBonus, MaxHealthUp } // Type d’item
    public ItemType itemType;
    public int SoinPV = 1;           // Redonne de la vie
    public float damageBonus = 1f;         // Bonus dégâts (Tear + Brimstone)
    public float cooldownReduction = 0.1f; // Réduction cooldown larme normale
    public float speedBonus = 1f;          // Bonus vitesse déplacement
    public int maxHealthIncrease = 1;      // Ajoute un nouveau coeur /demi-coeur supplémentaire




    public GameObject newProjectilePrefab; // Prefab à remplacer si c’est un upgrade
    [HideInInspector] public float NewCooldownProjectile; // Cooldown du laser (gérée depuis le player)

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            // On récupère le script Déplacement_Character du joueur qui a touché l'objet
            var player = other.GetComponent<Déplacement_Character>();
            if (player == null)
                return; // Si ce n'est pas le joueur, on quitte la fonction

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
                    if (health != null) 
                        health.GetHealth(SoinPV); // On ajoute les points de vie
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

                // Augmente les dégâts du joueur 
                case ItemType.damageBonus:
                    player.damageBrimstone += damageBonus;//  - Pour le tir laser (Brimstone)
                    player.damageTear+= damageBonus; //  - Pour les larmes normales (Tear)
                    break;


                case ItemType.cooldownReduction:

                    // Réduit le cooldown des tirs normaux
                    player.cooldownNormal -= cooldownReduction;
                    // Empêche de descendre sous 0.01 sec
                    player.cooldownNormal = Mathf.Max(0.1f, player.cooldownNormal);

                    // Réduit le cooldown du Brimstone (laser)
                    player.cooldownBrimstone -= cooldownReduction;
                    // Empêche de descendre sous 0.01 sec
                    player.cooldownBrimstone = Mathf.Max(0.1f, player.cooldownBrimstone);
                    break;


                // Augmente la vitesse de déplacement du joueur
                case ItemType.speedBonus:
                    player.vitesseDeplacementPersonnage += speedBonus;
                    break;


                // Augmente la vie maximale du joueur et met à jour l'UI des cœurs
                case ItemType.MaxHealthUp:
                    if (health != null)
                        health.AddMaxHealth(maxHealthIncrease);
                    break;



            }

            // Détruit l’item après collecte
            Destroy(gameObject);
        }
    }
}
