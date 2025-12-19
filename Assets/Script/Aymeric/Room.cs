using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeSalle
{
    Vide,
    Combat,
    Item,
    Boss,
    Spawn,
    Bloquer

}

[System.Serializable]
public struct IndexGrid
{
    public int x;
   public int y;

    public IndexGrid(int v1, int v2) : this()
    {
        x = v1;
        y = v2;
    }
}

public class Room
{
    public IndexGrid IndexInMap;
    public Vector2 CoordonerInScene;

    bool active;
    TypeSalle type;
    public bool acceptePlusieurVoisin;
    public bool VoisinHaut;
    public bool VoisinBas;
    public bool VoisinGauche;
    public bool VoisinDroite;
    public Dictionary<Direction, Room> Voisins = new();

    public Room(bool active, TypeSalle typeSalle, IndexGrid IMap)
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
