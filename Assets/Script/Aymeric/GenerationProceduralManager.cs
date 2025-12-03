using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerationProceduralManager : MonoBehaviour
{

    public Boolean[,] map = new Boolean[9, 9]; //(map = new Room[width, height];)
    public 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        map[4,4] = true;
        
        foreach (var item in GetAllCoBetweenMinMaxCo( map, new Vector2(1,2), new Vector2(5, 6)))
        {
            Debug.Log(item);
        }
        Vector2 BossCo = GetRandomCoInListRange(new List<List<Vector2>>
{
    GetAllCoBetweenMinMaxCo(map, new Vector2(0, 0), new Vector2(1, 7)),
    GetAllCoBetweenMinMaxCo(map, new Vector2(2, 0), new Vector2(6, 1)),
    GetAllCoBetweenMinMaxCo(map, new Vector2(7, 0), new Vector2(8, 8)),
    GetAllCoBetweenMinMaxCo(map, new Vector2(2, 7), new Vector2(6, 8))
});
        map[(int)BossCo.x, (int)BossCo.y] = true;
        for (int i = 0; i < 9; i++)
        {
            Debug.Log(
    "( " + i + ",0 ):" + map[i, 0] + " . " +
    "( " + i + ",1 ):" + map[i, 1] + " . " +
    "( " + i + ",2 ):" + map[i, 2] + " . " +
    "( " + i + ",3 ):" + map[i, 3] + " . " +
    "( " + i + ",4 ):" + map[i, 4] + " . " +
    "( " + i + ",5 ):" + map[i, 5] + " . " +
    "( " + i + ",6 ):" + map[i, 6] + " . " +
    "( " + i + ",7 ):" + map[i, 7] + " . " +
    "( " + i + ",8 ):" + map[i, 8]
);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // récupére la vleur de tout les voisin autour de la corrdonée donner
    Boolean[] GetAllVoisinsValue(Boolean[,] map , Vector2 co)
    {
        Boolean[] result = new Boolean[7];
        result[0] = map[(int)(co.x - 1), (int)(co.y)];
        result[1] = map[(int)(co.x - 1), (int)(co.y+1)];
        result[2] = map[(int)(co.x), (int)(co.y+1)];
        result[3] = map[(int)(co.x + 1), (int)(co.y + 1)];
        result[4] = map[(int)(co.x + 1), (int)(co.y)];
        result[5] = map[(int)(co.x + 1), (int)(co.y-1)];
        result[6] = map[(int)(co.x ), (int)(co.y -1)];
        result[7] = map[(int)(co.x - 1), (int)(co.y-1)];
            return result;
    }

    List<Vector2> GetAllCoBetweenMinMaxCo(Boolean[,] map , Vector2 coMin , Vector2 coMax)
    {
        List<Vector2> result = new List<Vector2>();
        for (int XI = (int)coMin.x; XI <= coMax.x; XI++)
        {
            for (int YI = (int)coMin.y; YI <= coMax.y; YI++)
            {
                if (!result.Contains(new Vector2(XI, YI)))
                {
                    result.Add(new Vector2(XI, YI));
                }
                
            }
        }
        return result;
    }

    Vector2 GetRandomCoInListRange(List<List<Vector2>> listeCo)
    {
        Vector2 result = new Vector2();
        List<Vector2> fusion = new List<Vector2>();

        foreach (var subList in listeCo)
        {
            fusion.AddRange(subList);
        }

        result = fusion[ UnityEngine.Random.Range(0, fusion.Count)];

        return result;
    }

}
