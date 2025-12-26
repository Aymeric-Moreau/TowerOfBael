using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script gère la vie (HP) d'un ennemi
public class Ennemie_Health : MonoBehaviour
{
    // La vie maximale de l'ennemi, réglable dans l'inspecteur
    [SerializeField]
    private float Maximum_Health_Ennemie;

    // La vie actuelle de notre ennemie
    [SerializeField]
    private float Current_Health_Ennemie;

    // Propriété qui retourne la vie restante en pourcentage (0 à 1)
    public float Remain_Health
    {
        get
        {
            return Current_Health_Ennemie / Maximum_Health_Ennemie;
        }
    }

    // Fonction qui détruit l'ennemi (appelée quand la vie atteint 0)
    public void Mort_Ennemie()
    {
        Destroy(this.gameObject);
    }


    // Fonction qui applique des dégâts à l'ennemi
    public void TakeDamgeEnnemie(float Damage)
    {
        // Si la vie est déjà à 0, on ne fait rien
        if (Current_Health_Ennemie == 0)
        {
            return;
        }

        // Décrémente la vie de notre ennemie
        Current_Health_Ennemie -= Damage;

        
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(Cligote( sprite));


        // Empêche que la vie devienne négative
        if (Current_Health_Ennemie < 0)
        {
            Current_Health_Ennemie = 0;
        }

        // Si la vie atteint 0, l'ennemie meurt
        if (Current_Health_Ennemie == 0)
        {
            Mort_Ennemie();

        }
    }

    IEnumerator Cligote(SpriteRenderer sprite)
    {
        Color baseColor = sprite.color;
        Debug.Log("degat coroutine start");
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sprite.color = baseColor;
        
    }
}



    
