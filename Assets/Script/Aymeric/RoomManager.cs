using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    Vector2 Index;
    Vector2 CoordonerInScene;
    // stock les porte active comme inactif de la porte
    public Door[] portes;
    public GameObject[] wallsDoor;
    public GameObject ennemis;
    public int nbrEnnemis;
    //public Dictionary<Direction, Door> lesPortes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Door[] scriptsDoors = GetComponentsInChildren<Door>();
        //foreach (var item in scriptsDoors)
        //{
        //    lesPortes.Add(item.direction, item);
        //    Debug.Log(item.name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Door GetDoor(Direction dir)
    {
        Door door = null;
        foreach (var item in portes)
        {
            
            if (item.direction == dir)
            {
                door = item;
                break;
            }
        }

        return door;

    }

    public void IncreaseNbrEnnemis()
    {
        nbrEnnemis++;
        if (nbrEnnemis >= 0)
        {
            // ferme les accés
            foreach (var porte in portes)
            {
                if (porte.isActiveAndEnabled)
                {
                    porte.activeWall();
                }
            }
        }
    }

    public void DecreaseNbrEnnemis()
    {
        nbrEnnemis--;
        if (nbrEnnemis <= 0) {
            // ouvre les accés
            foreach (var porte in portes)
            {
                if (porte.isActiveAndEnabled)
                {
                    porte.desactiveWall();
                }
            }
        }
    }
}
