using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère la durée d'invincibilité du joueur via une coroutine.
public class Invincible_Lenght : MonoBehaviour
{
    //  Référence au système de vie du joueur.
    private Health_Systeme health_Systeme;

    private void Awake()
    {
        // On récupère automatiquement le Health_Systeme attaché au même GameObject
        //  pour pouvoir modifier la propriété Invincible plus tard
        health_Systeme = GetComponent<Health_Systeme>();
    }

    // Fonction publique pour lancer l'invincibilité
    // On passe la durée d'invincibilité en paramètre
    public void StartInvincible(float Durée)
    {
        // StartCoroutine lance la coroutine qui gère l'invincibilité dans le temps
        StartCoroutine(InvincibleCoroutine(Durée));
    }

    //  Coroutine : fonction spéciale qui peut "suspendre" son exécution et la reprendre plus tard
    private IEnumerator InvincibleCoroutine(float Durée)
    {
        // Active l'invincibilité dans le Health_Systeme
        health_Systeme.Invincible = true;

        // "yield return" dit à Unity :
        // attends X secondes avant de continuer la suite du code
        yield return new WaitForSeconds(Durée);

        //Après la durée, désactive l'invincibilité
        health_Systeme.Invincible = false;
    }






}
