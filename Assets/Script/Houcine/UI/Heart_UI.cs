using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // nécessaire pour utiliser Image et Sprite

public class Heart_UI : MonoBehaviour
{
    // Tableau d’images UI représentant les cœurs (de gauche à droite)
    public Image[] hearts;

    // Sprites possibles pour un cœur
    public Sprite heartFull; // cœur plein
    public Sprite heartHalf; // demi-cœur
    public Sprite heartEmpty; // cœur vide

    // Nombre de points de vie par cœur (2 = plein, 1 = moitié)
    public int hpPerHeart = 2;

    // Fonction appelée pour mettre à jour l’affichage des cœurs
    // currentHP = vie actuelle du joueur
    public void UpdateHearts(int currentHP)
    {
        // On parcourt chaque cœur
        for (int i = 0; i < hearts.Length; i++)
        {
            // Calcul de la vie restante pour ce cœur précis
            int heartHP = Mathf.Clamp(currentHP - (i * hpPerHeart),0,hpPerHeart);

            // Si le cœur est plein
            if (heartHP >= hpPerHeart)
                hearts[i].sprite = heartFull;
                

            // Si le cœur est à moitié rempli
            else if (heartHP > 0)
                hearts[i].sprite = heartHalf;

            // Si le cœur est vide
            else
                hearts[i].sprite = heartEmpty;

        }
    }
}
