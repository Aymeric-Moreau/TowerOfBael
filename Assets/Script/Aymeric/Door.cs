using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum Direction
{
    gauche,droite,haut,bas


}

public class Door : MonoBehaviour
{
    public Vector2 indexRoom;
    public GameObject roomCible;
    public Direction direction;
    public Transform spawnPoint;

    public static readonly Dictionary<Direction, Direction> directionCible = new Dictionary<Direction, Direction>{
    { Direction.droite, Direction.gauche },
    { Direction.gauche, Direction.droite },
    { Direction.haut, Direction.bas },
    { Direction.bas, Direction.haut } };


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
            Debug.Log("player in porte");
            //collision.transform.position;
            //collision.transform.position;
        }
    }
}
