using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinDuJeu : MonoBehaviour
{
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Victory victoire = FindObjectOfType<Victory>();

            // Vérifie que le script existe
            if (victoire != null)
                // Affiche l'écran de Game Over
                victoire.ShowGameOver();

        }
    }

}
