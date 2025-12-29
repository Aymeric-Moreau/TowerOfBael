using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ce script gère l'écran de Game Over et les boutons liés (Restart, Menu)
public class Victory : MonoBehaviour
{
    // Référence au panel complet de Game Over (texte + boutons)
    public GameObject victoireScreen; 

    private void Start()
    {
        // On cache le panel au début
        if (victoireScreen != null)
            victoireScreen.SetActive(false);
    }

    // Appelée quand le joueur meurt
    public void ShowGameOver()
    {
        // Active le panel Game Over pour l'afficher
        if (victoireScreen != null)
            victoireScreen.SetActive(true);
    }

    // Bouton Restart
    public void RestartGame()
    {
        // Recharge la scène actuelle pour recommencer le niveau
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    // Bouton MainMenu
    public void GoToMainMenu()
    {
        // Charge la scène du menu principal
        SceneManager.LoadScene("Main_Men_Game"); 
    }
}
