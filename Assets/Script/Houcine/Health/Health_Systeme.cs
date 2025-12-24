using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ce script gère la santé du joueur ainsi que le feedback visuel (changement de couleur) lorsqu'il subit des dégâts
public class Health_Systeme : MonoBehaviour
{
    // SpriteRenderer du joueur (sert à changer la couleur lors des dégâts)
    private SpriteRenderer sprite;

    // Couleur d'origine du sprite (avant de prendre des dégâts)
    private Color baseColor;

    // Référence vers la coroutine de dégâts en cours
    // Permet d'éviter plusieurs flashs en même temps
    private Coroutine damageCoroutine;

    // La vie maximale du personnage
    [SerializeField]
    private float Maximum_Health;

    // La vie actuelle du personnage
    [SerializeField]
    private float Current_Health;

    // Propriété publique pour lire la vie actuelle
    public float CurrentHealth
    {
        get { return Current_Health; }
    }
    void Start()
    {
        // Vérifie que la référence au Heart_UI n'est pas vide
        if (heartUI != null)
        {
            // Crée dynamiquement les cœurs dans le UI Manager
            // Le nombre de cœurs est calculé à partir de la vie maximale du joueur
            heartUI.CreateHearts((int)Maximum_Health);

            // Met à jour l'affichage des cœurs pour refléter la vie actuelle
            // Par exemple, certains cœurs peuvent être pleins, à moitié ou vides
            heartUI.UpdateHearts((int)Current_Health);
        }

        // Récupère le SpriteRenderer attaché au joueur
        sprite = GetComponent<SpriteRenderer>();

        // Vérifie que le SpriteRenderer existe
        if (sprite != null)
        {
            // Sauvegarde la couleur initiale du sprite
            baseColor = sprite.color;
        }

    }

    // Coroutine appelée quand le joueur prend des dégâts
    // Elle gère l'effet visuel de "hit" (flash rouge)
    IEnumerator DamageFlash()
    {
        Debug.Log("degat coroutine start");

        // Change la couleur du sprite en rouge
        sprite.color = Color.red;

        // Attend 0.15 seconde sans bloquer le jeu
        yield return new WaitForSeconds(0.15f);

        // Restaure la couleur d'origine du sprite
        sprite.color = baseColor;
    }


    // Référence au script wrapper qui gère l'invincibilité
    public Character_Damge_Inviincible invincibleHandler;

    public Heart_UI heartUI;


    // Propriété qui retourne la vie restante en pourcentage (0 à 1)
    public float Remain_Health
    {
        get
        {
            return Current_Health / Maximum_Health;
        }
    }

    // Propriété publique pour savoir si le personnage est invincible
    public bool Invincible { get; set; }

    // Fonction appelée quand le joueur meurt
    void Mort_Joueur()
    {
        Debug.Log("Le joueur est mort !");

        // Récupère le script de déplacement sur le joueur
        Déplacement_Character depl = GetComponent<Déplacement_Character>();
        if (depl != null)
        {
            depl.estVivant = false; // bloque déplacement et tir
        }

        // Désactive le collider pour que le joueur ne gêne plus les collisions
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Récupère le script Game Over dans la scène
        Game_Over game_over = FindObjectOfType<Game_Over>();

        // Vérifie que le script existe
        if (game_over != null)
            // Affiche l'écran de Game Over
            game_over.ShowGameOver();


    }

    // Fonction pour infliger des dégâts
    public void TakeDamge(float Damage)
    {

        // Si la vie est déjà à 0 ou si le personnage est invincible, on ne fait rien
        if (Current_Health == 0 || Invincible)
        {
            return;
        }
         if (Invincible)
        {
                return ;

        }

        // Décrémente la vie du personnage
        Current_Health -= Damage;

        // Empêche que la vie devienne négative
        if (Current_Health < 0)
        {
            Current_Health = 0;
        }

        // Vérifie que le SpriteRenderer existe
        if (sprite != null)
        {
            // Si une coroutine de dégâts est déjà en cours
            // on l'arrête pour éviter un empilement de flashs
            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);

            // Lance la nouvelle coroutine de flash rouge
            damageCoroutine = StartCoroutine(DamageFlash());
        }

        // Si la vie atteint 0, le joueur meurt
        if (Current_Health == 0)
        {
            Mort_Joueur();

        } else
        {    // Sinon, si un invincibleHandler est attaché et que le joueur n'est pas déjà invincible
            //  on déclenche l'invincibilité temporaire
            if (invincibleHandler != null && !Invincible)
                invincibleHandler.startInvincible();
        }
        // Met à jour l'affichage des cœurs dans l'UI en fonction de la vie actuelle du joueur
        heartUI.UpdateHearts((int)Current_Health);

    }//  Fonction pour soigner le personnage
    public void GetHealth(float Add_Health)
    {
        // Si le joueur est déjà à sa vie maximale, on ne fait rien
        if (Current_Health == Maximum_Health)
        {
            return;
        }

        // Ajoute la vie
        Current_Health += Add_Health;

        // Empêche que la vie dépasse la vie maximale
        if (Current_Health > Maximum_Health)
        {
            Current_Health = Maximum_Health; 
        }
        // Met à jour l'affichage des cœurs dans l'UI en fonction de la vie actuelle du joueur
        heartUI.UpdateHearts((int)Current_Health);
    }


}
