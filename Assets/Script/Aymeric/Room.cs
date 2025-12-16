using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeSalle
{
    Vide,
    Combat,
    Item,
    Boss,
    Spawn

}

public class Room
{
    public Vector2 IndexInMap;
    public Vector2 CoordonerInScene;

    bool active;
    TypeSalle type;
    public bool acceptePlusieurVoisin;
    public bool VoisinHaut;
    public bool VoisinBas;
    public bool VoisinGauche;
    public bool VoisinDroite;
    public Dictionary<Direction, Room> Voisins = new();

    public Room(bool active, TypeSalle typeSalle, Vector2 IMap)
    {
        this.active = active;
        this.type = typeSalle;
        this.IndexInMap = IMap;
    }

    public void SetActive(bool value) => active = value;
    public bool IsActive() => active;

    public void SetSalleType(TypeSalle t) => type = t;
    public TypeSalle GetSalleType() => type;

    public void SetCoordonerInScene(Vector2 co) => CoordonerInScene = co;
    public Vector2 GetCoordonerInScene() => CoordonerInScene;

    public bool HasNeighbor(Direction dir)
    {
        return Voisins.ContainsKey(dir);
    }

    public Room GetNeighbor(Direction dir)
    {
        Voisins.TryGetValue(dir, out Room neighbor);
        return neighbor;
    }

    public void SetNeighbor(Direction dir, Room room)
    {
        Voisins[dir] = room;
    }
}
