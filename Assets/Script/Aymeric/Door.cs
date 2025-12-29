using System;
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
    public IndexGrid indexRoom;
    public GameObject roomCible;
    public Direction direction;
    public Transform spawnPoint;
    public GameObject wall;

    // dictionnaire des direction opposé de chaque direction de porte
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
    // enléve le le mur
    public void desactiveWall()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        wall.SetActive(false);
    }
    // active le mur
    public void activeWall()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        wall.SetActive(true);
    }

    // quand il vas rentrer dans le trigger sa vas tp le joueur dans la room cible a point de tp qui corespond a la porte de direction opposé a celle pris 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && wall.activeSelf == false)
        {
            Debug.Log("player in porte");
            RoomManager RMScript = roomCible.GetComponent<RoomManager>();
            directionCible.TryGetValue(direction, out Direction dirCible);
            
            Camera.main.transform.position = roomCible.transform.position + new Vector3(0,0,-20);
            collision.gameObject.transform.position = RMScript.GetDoor(dirCible).spawnPoint.position;
            RMScript.ennemis.SetActive(true);
            //collision.transform.position;
            //collision.transform.position;
        }
    }
}
