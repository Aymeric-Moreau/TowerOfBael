using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // IMPORTANT pour TextMeshPro

// Ce script gère les boutons du menu principal (Play, Quit)
public class Main_Menu : MonoBehaviour
{

    public string seed;
    // Fonction reliée au bouton "Play"
    public void PlayGame()
    {
        // Charge la scène de jeu par son nom
        SceneManager.LoadScene("GameScene");
    }

    // Fonction reliée au bouton "Quit"
    public void QuitGame()
    {
        // Quitte l'application. Fonctionne uniquement dans le build final.
        // Dans l'éditeur Unity, cela n'aura aucun effet.
        Application.Quit();
    }

    public void setSeed(string seed)
    {
        GameManager.instance.seed = seed;
        //GameManager.instance.seed
    }

}
