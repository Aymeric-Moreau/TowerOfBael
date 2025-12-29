using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // nécessaire pour utiliser Image et Sprite

// Ce script gère l'affichage de la vie du joueur sous forme de cœurs
public class Heart_UI : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab Image
    public Sprite heartFull; // cœur plein
    public Sprite heartHalf; // cœur à moitié rempli
    public Sprite heartEmpty; // cœur vide
    public int hpPerHeart = 2;  // Nombre de points de vie que représente un cœur complet

    private GameObject[] hearts; // Tableau qui stocke les références aux cœurs instanciés
    // Référence au panel complet de Game Over (texte + boutons)
    public GameObject HideUI;

    // Crée tous les cœurs selon la vie max

    private void Start()
    {
        // On cache le panel au début
        if (HideUI != null)
            HideUI.SetActive(true);
    }

    // Appelée quand le joueur meurt
    public void ShowGameOver()
    {
        // Active le panel Game Over pour l'afficher
        if (HideUI != null)
            HideUI.SetActive(false);
    }
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
            rt.anchoredPosition = new Vector2(i * 75, 0); // Décale chaque cœur horizontalement pour qu’ils ne se chevauchent pas
            rt.localScale = Vector3.one; // Assure que l’échelle du cœur est correcte
            hearts[i] = go;
        }
    }

    // Met à jour les cœurs selon la vie actuelle
    public void UpdateHearts(int currentHP)
    {
        Debug.Log("valeur de heart : " + hearts.IsUnityNull());
        if (hearts == null) return; // Si aucun cœur n’a été créé, on ne fait rien

        // Parcourt tous les cœurs et met à jour leur sprite
        for (int i = 0; i < hearts.Length; i++)
        {
            // Calcule combien de points de vie reste pour ce cœur précis
            int heartHP = Mathf.Clamp(currentHP - (i * hpPerHeart), 0, hpPerHeart);
            Debug.Log(heartHP + " point de vie qui rest au jouur il a été tocuhe");
            // Récupère l'Image du cœur
            Image img = hearts[i].GetComponent<Image>();

            // Change le sprite selon les points de vie restants
            if (heartHP >= hpPerHeart)
                img.sprite = heartFull;
            else if (heartHP > 0)
                img.sprite = heartHalf;
            else
                img.sprite = heartEmpty;
        }
    }
}
