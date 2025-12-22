using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // nécessaire pour utiliser Image et Sprite

public class Heart_UI : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab Image
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;
    public int hpPerHeart = 2;

    private GameObject[] hearts; // Tableau qui stocke les références aux cœurs instanciés

    // Crée tous les cœurs selon la vie max
    public void CreateHearts(int maxHP)
    {
        // Si des cœurs ont déjà été créés, on les détruit pour repartir à zéro
        if (hearts != null)
        {
            foreach (var h in hearts)
                Destroy(h);
        }

        // Calcul du nombre total de cœurs nécessaires
        int heartCount = Mathf.CeilToInt((float)maxHP / hpPerHeart);
        hearts = new GameObject[heartCount];

        // Instancie chaque cœur
        for (int i = 0; i < heartCount; i++)
        {
            GameObject go = Instantiate(heartPrefab, transform);// Instancie le prefab comme enfant du manager
            RectTransform rt = go.GetComponent<RectTransform>(); // Récupère le RectTransform pour positionner le cœur
            rt.anchoredPosition = new Vector2(i * 75, 0); // Décale chaque cœur horizontalement pour qu’ils ne se chevauchent pa
            rt.localScale = Vector3.one; // Assure que l’échelle du cœur est normale
            hearts[i] = go;
        }
    }

    // Met à jour les cœurs selon la vie actuelle
    public void UpdateHearts(int currentHP)
    {
        if (hearts == null) return; // Si aucun cœur n’a été créé, on ne fait rien

        // Parcourt tous les cœurs et met à jour leur sprite
        for (int i = 0; i < hearts.Length; i++)
        {
            // Calcule combien de points de vie reste pour ce cœur précis
            int heartHP = Mathf.Clamp(currentHP - (i * hpPerHeart), 0, hpPerHeart);
            Image img = hearts[i].GetComponent<Image>();

            if (heartHP >= hpPerHeart)
                img.sprite = heartFull;
            else if (heartHP > 0)
                img.sprite = heartHalf;
            else
                img.sprite = heartEmpty;
        }
    }
}
