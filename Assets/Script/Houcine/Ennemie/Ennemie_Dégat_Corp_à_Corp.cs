using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère les dégâts de contact au corps à corps d'un ennemi
public class Ennemie_Dégat_Corp_à_Corp : MonoBehaviour
{
    //Montant de dégâts infligé par l'ennemi au contact physique
    [SerializeField]
    private float damage_amount;

    // Appelé chaque frame où l'ennemi reste en collision avec un autre objet
    private void OnCollisionStay2D(Collision2D collision)
    {
        //  Vérifie si l'objet en collision possède le script Déplacement_Character
        //  Cela permet de savoir si c'est le joueur
        if (collision.gameObject.GetComponent<Déplacement_Character>())
        {
            //  Récupère le script Health_Systeme du joueur pour gérer les PV
            var health_Systeme = collision.gameObject.GetComponent<Health_Systeme>();

            //Applique les dégâts au joueur
            health_Systeme.TakeDamge(damage_amount);

        }
    }
}

