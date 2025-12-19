using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    Vector2 Index;
    Vector2 CoordonerInScene;
    public Door[] portes;
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
}
