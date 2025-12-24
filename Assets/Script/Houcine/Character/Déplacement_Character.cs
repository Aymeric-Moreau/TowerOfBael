using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ce script gère le déplacement et les tir du personnage joueur
public class Déplacement_Character : MonoBehaviour
{

    // Vitesse de déplacement du joueur
    public float vitesseDeplacementPersonnage = 10f;

    // Référence au Rigidbody2D pour gérer le mouvement physique
    private Rigidbody2D rb;

    //  Variables pour le tir
    public GameObject projectilePrefab;
    public float cooldownNormal;     
    public float cooldownBrimstone;     
    private float timerTir = 0f;
    public float damageTear = 1f;
    public float damageBrimstone = 5f;

    //  Indique si le joueur est vivant ou mort
    public bool estVivant = true;




    // Start is called before the first frame update
    void Start()
    {
        // On récupère le Rigidbody2D attaché au joueur
        rb = GetComponent<Rigidbody2D>();
    }

    // Fonction pour gérer le déplacement du joueur
    public void deplacement()
    {
        // On initialise les vecteurs de déplacement vertical et horizontal
        Vector2 Vertical = Vector2.zero;
        Vector2 Horizontal = Vector2.zero;

        //  Déplacement vertical
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
        {
            Vertical = Vector2.up * vitesseDeplacementPersonnage * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vertical = Vector2.down *
            vitesseDeplacementPersonnage * Time.fixedDeltaTime;
        }

        // Déplacement horizontal
        if (Input.GetKey(KeyCode.D))
        {
            Horizontal = Vector2.right * vitesseDeplacementPersonnage * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
        {
            Horizontal = Vector2.left *
            vitesseDeplacementPersonnage * Time.fixedDeltaTime;
        }


        // Addition des vecteurs pour obtenir le déplacement total
        Vector2 mouvementTotal = Vertical + Horizontal;

        // Déplacement du personnage via Rigidbody2D
        rb.MovePosition(rb.position + mouvementTotal);

        // On réinitialise la rotation pour éviter tout spin physique
        rb.angularVelocity = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        // Si le joueur est mort, on ne fait rien
        if (!estVivant)
            return;

        //  Décrément du timer de tir
        timerTir = timerTir - Time.deltaTime;

        // Initialisation du vecteur de direction du tir
        Vector2 directionTir = Vector2.zero;

        // Détection des touches pour le tir et calcul de la direction

        if (Input.GetKey(KeyCode.UpArrow))
            directionTir = directionTir + Vector2.up;

        if (Input.GetKey(KeyCode.DownArrow))
            directionTir = directionTir + Vector2.down;

        if (Input.GetKey(KeyCode.RightArrow))
            directionTir = directionTir + Vector2.right;

        if (Input.GetKey(KeyCode.LeftArrow))
            directionTir = directionTir + Vector2.left;

        // Si une direction est pressée, on tire

        if (directionTir != Vector2.zero)
        {
            Tirer(directionTir);
        }
    }

    //  Fonction pour tirer une balle

    void Tirer(Vector2 dir)
    {
        // Si le timer de tir est encore actif, on ne peut pas tirer
        if (timerTir > 0f)
            return;

        // On instancie le projectile actuel (le prefab défini dans le player)
        GameObject balleTiree = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Par défaut, on initialise le cooldown à celui des larmes normales
        float cooldownActuel = cooldownNormal;

        // Récupération des scripts des projectiles
        Brimstone_Character laserScript = balleTiree.GetComponent<Brimstone_Character>();
        Tear_Character tearScript = balleTiree.GetComponent<Tear_Character>();


        if (laserScript != null)
        {
            // On assigne la direction de tir au laser
            laserScript.direction = dir;

            // On transmet les dégâts depuis le player vers le Brimstone
            // Le laser utilisera donc la valeur actuelle du player.damageBrimstone
            laserScript.damage = damageBrimstone;

            // On définit le cooldown actuel du tir à celui du Brimstone
            // Cela permettra au timer du player de gérer correctement le délai
            cooldownActuel = cooldownBrimstone;
        }
        else
        {
            // On assigne la direction de tir à la larme normale
            tearScript.direction = dir;

            // On transmet les dégâts depuis le player vers la larme
            // La larme normale utilisera donc la valeur actuelle du player.damageTear
            tearScript.tear_damage = damageTear;

            // On définit le cooldown actuel du tir à celui des larmes normales
            // Le timer du player va attendre ce délai avant de pouvoir tirer à nouveau
            cooldownActuel = cooldownNormal;
        }

        // Met à jour le timer
        timerTir = cooldownActuel;
    }
    void FixedUpdate()
    {
        // Si le joueur est mort, on ne bouge pas
        if (!estVivant)
            return;

        // Appel de la fonction de déplacement
        deplacement();
    }
}
