using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Ce script sert de "wrapper" pour gérer l'invincibilité du joueur après un coup.
//  Il ne contient pas la logique de la coroutine elle-même, mais appelle le script Invincible_Lenght.
public class Character_Damge_Inviincible : MonoBehaviour
{
    // Durée pendant laquelle le joueur sera invincible après un coup.
    [SerializeField]
    private float Invincible_Durée;

    //Référence au script Invincible_Lenght qui contient la coroutine réelle.
    private Invincible_Lenght invincible_lenght;

    private void Awake()
    {
        //On récupère le script Invincible_Lenght attaché au même GameObject.
        invincible_lenght = GetComponent<Invincible_Lenght>();
    }

    public void startInvincible()
    {
        // On appelle la fonction StartInvincible du script Invincible_Lenght
        // et on lui passe la durée configurée.
        invincible_lenght.StartInvincible (Invincible_Durée);
    }

}
