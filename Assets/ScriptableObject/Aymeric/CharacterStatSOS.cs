
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterStatSOS", order = 1)]
public class CharacterStatSOS : ScriptableObject
{
    public int MaxHealth;
    public int CurrentHealt;
    public float vitesseDeplacementPersonnage;
    //  Variables pour le tir
    public GameObject projectilePrefab;
    public float cooldown;
    
    private float timerTir ;
    public float damage;
    



}