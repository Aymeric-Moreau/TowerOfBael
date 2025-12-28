using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemPoolSOS items;
    
    // Start is called before the first frame update
    void Start()
    {
         

        GameObject item = items.GetRandomLoot();
        Instantiate(item,transform.position,Quaternion.identity);

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
