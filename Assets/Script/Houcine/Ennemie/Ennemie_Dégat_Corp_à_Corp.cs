using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemie_Dégat_Corp_à_Corp : MonoBehaviour
{
    //Montant de dégâts infligé par l'ennemi
    [SerializeField]
    private float damage_amount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        //  Vérifie si l'objet en collision possède le script Déplacement_Character
        //  Cela permet de savoir si c'est le joueur
        if (collision.gameObject.GetComponent<Déplacement_Character>())
        {
            //  Récupère le script Health_Systeme du joueur
            var health_Systeme = collision.gameObject.GetComponent<Health_Systeme>();

            //Applique les dégâts au joueur
            health_Systeme.TakeDamge(damage_amount);

        }
    }
}

