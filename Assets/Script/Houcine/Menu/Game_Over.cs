using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over : MonoBehaviour
{
    // Référence au panel complet de Game Over (texte + boutons)
    public GameObject gameOverScreen; 

    private void Start()
    {
        // On cache le panel au début
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    // Appelée quand le joueur meurt
    public void ShowGameOver()
    {
        // Active le panel Game Over pour l'afficher
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
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
