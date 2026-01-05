using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemPoolSOS", order = 1)]
public class ItemPoolSOS : ScriptableObject
{
    public loot[] pool;

    public GameObject GetRandomLoot()
    {
        GameObject result = null;
        int sommeProba = 0;
        int sommeGloablProba = 0;
        int rInt;

        foreach (var item in pool)
        {
            sommeGloablProba += item.pourcentageChance;
        }
        if (sommeGloablProba > 100)
        {
            rInt = UnityEngine.Random.Range(0, sommeGloablProba);
        }
        else
        {
            rInt = UnityEngine.Random.Range(0, 100);
        }

        foreach (var item in pool)
        {
            sommeProba += item.pourcentageChance;
            if (rInt < sommeProba)
            {
                result = item.itemGO;
                break;
            }
        }

        return result;
    }
}