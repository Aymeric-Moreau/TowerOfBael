using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerationProceduralManager : MonoBehaviour
{
    public Room[,] map = new Room[9, 9];

    void Start()
    {
        InitializeMapDefault();

        Vector2 SpawnCo = new Vector2(4,4);
        // Centre
        map[(int)SpawnCo.x, (int)SpawnCo.y].SetActive(true);
        map[(int)SpawnCo.x, (int)SpawnCo.y].SetSalleType(TypeSalle.Vide);

        

        // Choix boss
        Vector2 BossCo = GetRandomCoInListRange(new List<List<Vector2>>
        {
            GetAllCoBetweenMinMaxCo(new Vector2(0, 0), new Vector2(1, 7)),
            GetAllCoBetweenMinMaxCo(new Vector2(2, 0), new Vector2(6, 1)),
            GetAllCoBetweenMinMaxCo(new Vector2(7, 0), new Vector2(8, 8)),
            GetAllCoBetweenMinMaxCo(new Vector2(2, 7), new Vector2(6, 8))
        });

        map[(int)BossCo.x, (int)BossCo.y].SetActive(true);
        map[(int)BossCo.x, (int)BossCo.y].SetSalleType(TypeSalle.Boss);

        List<Vector2> pathCo = GetCoPathBetweenTwoCoo(SpawnCo, BossCo);
        SetRoomInAllCoList(pathCo);


        // Debug map
        for (int i = 0; i < 9; i++)
        {
            Debug.Log(
                "( " + i + ",0 ):" + Format(i, 0) + " . " +
                "( " + i + ",1 ):" + Format(i, 1) + " . " +
                "( " + i + ",2 ):" + Format(i, 2) + " . " +
                "( " + i + ",3 ):" + Format(i, 3) + " . " +
                "( " + i + ",4 ):" + Format(i, 4) + " . " +
                "( " + i + ",5 ):" + Format(i, 5) + " . " +
                "( " + i + ",6 ):" + Format(i, 6) + " . " +
                "( " + i + ",7 ):" + Format(i, 7) + " . " +
                "( " + i + ",8 ):" + Format(i, 8)
            );
        }

        Debug.Log("co Spawn = " + SpawnCo + " co Boss = " + BossCo);
        foreach (var item in pathCo)
        {
            Debug.Log(item);
        }
    }

    string Format(int x, int y)
    {
        return map[x, y].GetSalleType() + " - " + map[x, y].IsActive();
    }

    // Initialise toutes les salles à Vide + inactif
    void InitializeMapDefault()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                map[x, y] = new Room(false, TypeSalle.Vide, new Vector2(x, y));
            }
        }
    }

    // Récupère les 8 voisins autour d’une case (si hors limite → null)
    Room[] GetAllVoisins(Vector2 co)
    {
        Room[] result = new Room[8];

        int cx = (int)co.x;
        int cy = (int)co.y;

        result[0] = GetRoom(cx - 1, cy);     // Gauche
        result[1] = GetRoom(cx - 1, cy + 1); // Haut-Gauche
        result[2] = GetRoom(cx, cy + 1);     // Haut
        result[3] = GetRoom(cx + 1, cy + 1); // Haut-Droite
        result[4] = GetRoom(cx + 1, cy);     // Droite
        result[5] = GetRoom(cx + 1, cy - 1); // Bas-Droite
        result[6] = GetRoom(cx, cy - 1);     // Bas
        result[7] = GetRoom(cx - 1, cy - 1); // Bas-Gauche

        return result;
    }

    // Retourne directement la Room si elle existe, sinon null
    Room GetRoom(int x, int y)
    {
        // Hors limite → pas de room → null
        if (x < 0 || y < 0 || x >= 9 || y >= 9)
            return null;

        return map[x, y];
    }

    // renvoie une liste des cooronée présente entre coMin et coMax
    List<Vector2> GetAllCoBetweenMinMaxCo(Vector2 coMin, Vector2 coMax)
    {
        float coMaxX = coMax.x;
        float coMaxY = coMax.y;
        float coMinX = coMin.x;
        float coMinY = coMin.y;
        List<Vector2> result = new List<Vector2>();
        if (coMin.x > coMax.x)
        {
            coMaxX = coMin.x;
            coMinX = coMax.x;
        }
        if (coMin.y > coMax.y) { coMaxY = coMin.y; coMinY = coMax.y; }

        Debug.Log("GetAllCoBetweenMinMaxCo = coMin = " + coMin + " coMax = " + coMax);
        // tant que co index x est plus petit qye comax.x 
        for (int XI = (int)coMinX; XI <= coMaxX; XI++)
        {
            for (int YI = (int)coMinY; YI <= coMaxY; YI++)
            {
                result.Add(new Vector2(XI, YI));
            }
        }
        return result;
    }
    // renvoie une co aléatoire a partir d'une liste de liste de co 
    Vector2 GetRandomCoInListRange(List<List<Vector2>> listeCo)
    {
        List<Vector2> fusion = new List<Vector2>();

        foreach (var subList in listeCo)
            fusion.AddRange(subList);

        return fusion[UnityEngine.Random.Range(0, fusion.Count)];
    } 

    // co min = 4,4 coMax = 8,5
    // 8,5 - 4,4 = 4,1
    // 4 +4 = 8
    // 4+1 = 5
    // on récupére les case entre 4,4 et 8,4 ensuite entre 8,4 et 8,5
   List<Vector2> GetCoPathBetweenTwoCoo(Vector2 coMin, Vector2 coMax)
    {
        List<Vector2> result = new List<Vector2>();
        float ecartX = coMax.x - coMin.x;
        float ecartY = coMax.y - coMin.y;

        Debug.Log("comin = " + coMin +  ", comax = " + coMax + ", ecart = " + ecartX + "," + ecartY);

        List<Vector2> lignevertical = GetAllCoBetweenMinMaxCo(coMin, new Vector2(coMin.x + ecartX , coMin.y));
        List<Vector2> ligneveHorizontal = GetAllCoBetweenMinMaxCo(new Vector2(coMin.x + ecartX, coMin.y), new Vector2(coMin.x + ecartX , coMin.y + ecartY));

        Debug.Log("lignevertical = " + lignevertical.IsUnityNull() + "," + lignevertical.Count);
        Debug.Log("ligneveHorizontal = " + ligneveHorizontal.IsUnityNull() + "," + ligneveHorizontal.Count);
        //Debug.Log("ligneveHorizontal = " + ligneveHorizontal.ToString());

        result.AddRange(lignevertical);
        result.AddRange(ligneveHorizontal);


        return result;
    }

    void SetRoomInAllCoList(List<Vector2> coList)
    {
        foreach (var item in coList)
        {
            if(!map[(int)item.x, (int)item.y].IsActive())
            {
                map[(int)item.x, (int)item.y].SetActive(true);
                map[(int)item.x, (int)item.y].SetSalleType(TypeSalle.Combat);
            }
            
        }
    }

}
