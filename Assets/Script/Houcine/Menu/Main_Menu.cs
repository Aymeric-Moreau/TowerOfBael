using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    // Fonction reliée au bouton "Play"
    public void PlayGame()
    {
        // Charge la scène de jeu
        SceneManager.LoadScene("Houcine_Character");
    }

    // Fonction reliée au bouton "Quit"
    public void QuitGame()
    {
        // Quitte l'application. Fonctionne uniquement dans le build final.
        // Dans l'éditeur Unity, cela n'aura aucun effet.
        Application.Quit();
    }

}
