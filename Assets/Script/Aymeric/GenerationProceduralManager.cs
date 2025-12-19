using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
[Serializable]
public struct IndexMinMax
{
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
}

public class GenerationProceduralManager : MonoBehaviour
{
    
    public Room[,] map = new Room[9,9];
    public Dictionary<IndexGrid, GameObject> DictInstanciateRooms = new Dictionary<IndexGrid, GameObject>();
    public GameObject[] obstacles;
    
    public TypeSalle[] typeSalleParIndex;
    public GameObject[] prefabSalle;
    public float ecartEntreSallX = 50;
    public float ecartEntreSallY = 40;
    public int seed = 0;
    public int nbrSalleEnPlus = 0;
    public IndexMinMax[] zoneSalleBoss;

    IndexGrid SpawnCo;
    IndexGrid BossCo;


    void Start()
    {
        initialization();
        placingSpecialRoom();
        placingPathwayRoom();
        instanciationRoomInScene();

    }

    void initialization()
    {
        InitializeMapDefault();
        //UnityEngine.Random.state = new UnityEngine.Random.State();
        if (seed !=0)
        {
            UnityEngine.Random.InitState(seed);
        }
        
    }

    void placingSpecialRoom()
    {
        SpawnCo = new IndexGrid(4, 4);
        // Centre
        map[SpawnCo.x, SpawnCo.y].SetActive(true);
        map[SpawnCo.x, SpawnCo.y].SetSalleType(TypeSalle.Spawn);

        List<List<IndexGrid>> bossZoneListe = new List<List<IndexGrid>>();
        foreach (var item in zoneSalleBoss)
        {
            bossZoneListe.Add(GetAllCoBetweenMinMaxCo(new IndexGrid(item.minX, item.minY), new IndexGrid(item.maxX, item.maxY)));
        }

        List<IndexGrid> listeCo = FusionneListe(bossZoneListe);
        // Choix boss
        Room BossRoom = GetRandomRoomInListRange(listeCo);
      

        BossCo = BossRoom.IndexInMap;

        BossRoom.SetActive(true);
        BossRoom.SetSalleType(TypeSalle.Boss);
    }

    void placingPathwayRoom()
    {
        List<IndexGrid> pathCo = GetCoPathBetweenTwoCoo(SpawnCo, BossCo);
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

        // GetEmptyCoordinatesWithOneNeighbor()
        //Debug.Log(" nombre de case possible pour item = " + GetEmptyRoomWithOneNeighbor().Count);
        Room itemRoom = GetRandomRoomInListRange(GetEmptyRoomWithOneNeighbor());

        itemRoom.SetActive(true);
        itemRoom.SetSalleType(TypeSalle.Item);
        itemRoom.acceptePlusieurVoisin = false;
        SetRoomInAllCoList(GetAllCloseVoisinsCo(itemRoom.IndexInMap),TypeSalle.Bloquer);



        // ajout de salle combat a des position aléatoire tout en étant a coté d'une salle déja existante
        List<Room> neighbors = GetEmptyRoomWithNeighbor(TypeSalle.Combat, TypeSalle.Spawn);
        int nbrSallecreer = 0;
        while (nbrSallecreer != nbrSalleEnPlus)
        {
            Room combatRoom = GetRandomRoomInListRange(neighbors);
            if (!combatRoom.IsActive())
            {
                combatRoom.SetActive(true);
                combatRoom.SetSalleType(TypeSalle.Combat);
                nbrSallecreer++;
            }

            
        }




    }

    void instanciationRoomInScene()
    {
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
        SetAllValueOfAllRoom();
        SpawnAllRoomInScene();
        SetAllValueOfAllPrefabRoom();
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
                map[x, y] = new Room(false, TypeSalle.Vide, new IndexGrid(x, y));
            }
        }
    }

    // Récupère les 8 voisins autour d’une case (si hors limite → null)
    Room[] GetAllVoisins(IndexGrid co)
    {
        Room[] result = new Room[8];

        int cx = co.x;
        int cy = co.y;

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

    Room getVoisin(IndexGrid co, Direction dir)
    {
        Room result = null;

        int cx = co.x;
        int cy = co.y;


        switch (dir)
        {
            case Direction.gauche:
                
                result = GetRoom(cx - 1, cy);     // Gauche
                break;

            case Direction.droite:
                
                result = GetRoom(cx + 1, cy);     // Droite
                break;

            case Direction.haut:
                
                result = GetRoom(cx, cy + 1);     // Haut
                break;

            case Direction.bas:
                
                result = GetRoom(cx, cy - 1);     // Bas
                break;

            default:
                Debug.LogWarning("Direction inconnue");
                break;
        }

        return result;
    }

    // Récupère les 4 voisins les plus proche autour d’une case (si hors limite → null)
    /// <summary>
    /// Récupére les 4 voisin dans cette ordre gauche, haut,droite,bas
    /// </summary>
    /// <param name="co">Coordoné de la room</param>
    /// <returns>un array des voisin</returns>
    Room[] GetAllCloseVoisins(IndexGrid co)
    {
        Room[] result = new Room[4];

        int cx = co.x;
        int cy = co.y;

        result[0] = GetRoom(cx - 1, cy);     // Gauche
        result[1] = GetRoom(cx, cy + 1);     // Haut
        result[2] = GetRoom(cx + 1, cy);     // Droite
        result[3] = GetRoom(cx, cy - 1);     // Bas

        return result;
    }

    // Récupère les 4 voisins les plus proche autour d’une case (si hors limite → null)
    /// <summary>
    /// Récupére les coordoné des 4 voisin dans cette ordre gauche, haut,droite,bas
    /// </summary>
    /// <param name="co">Coordoné de la room</param>
    /// <returns>un array des voisin</returns>
    IndexGrid[] GetAllCloseVoisinsCo(IndexGrid co)
    {
        IndexGrid[] result = new IndexGrid[4];

        int cx = co.x;
        int cy = co.y;

        result[0] = new IndexGrid(cx - 1, cy);     // Gauche
        result[1] = new IndexGrid(cx, cy + 1);     // Haut
        result[2] = new IndexGrid(cx + 1, cy);     // Droite
        result[3] = new IndexGrid(cx, cy - 1);     // Bas

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

    // Retourne directement la Room si elle existe, sinon null
    Room GetRoom(IndexGrid co)
    {
        // Hors limite → pas de room → null
        if (co.x < 0 || co.y < 0 || co.x >= 9 || co.y >= 9)
            return null;

        return map[co.x, co.y];
    }

    // renvoie une liste des cooronée présente entre coMin et coMax
    List<IndexGrid> GetAllCoBetweenMinMaxCo(IndexGrid coMin, IndexGrid coMax)
    {
        int coMaxX = coMax.x;
        int coMaxY = coMax.y;
        int coMinX = coMin.x;
        int coMinY = coMin.y;
        List<IndexGrid> result = new List<IndexGrid>();
        // Min est plus grand que max cela inverse les valuer (genre si on vuet un chemin entre 4.4 et 0.1)
        if (coMin.x > coMax.x)
        {
            coMaxX = coMin.x;
            coMinX = coMax.x;
        }
        if (coMin.y > coMax.y) 
        {
            coMaxY = coMin.y;
            coMinY = coMax.y;
        }

        
        // récupére tout les coordonée entre les x et y des 2 coordonée
        for (int XI = coMinX; XI <= coMaxX; XI++)
        {
            for (int YI = coMinY; YI <= coMaxY; YI++)
            {
                result.Add(new IndexGrid(XI, YI));
            }
        }
        return result;
    }


    List<IndexGrid> FusionneListe(List<List<IndexGrid>> listeCo)
    {
        List<IndexGrid> fusion = new List<IndexGrid>();

        foreach (var subList in listeCo)
        { fusion.AddRange(subList); }
        
        return fusion;
    }

    // renvoie une room aléatoire a partir d'une liste
    Room GetRandomRoomInListRange(List<Room> listeRoom)
    {
       
        return listeRoom[UnityEngine.Random.Range(0, listeRoom.Count)];
    }
    // renvoie une co aléatoire a partir d'une liste
    Room GetRandomRoomInListRange(List<IndexGrid> listeRoom)
    {
        IndexGrid coAleatoire = listeRoom[UnityEngine.Random.Range(0, listeRoom.Count)];
        return map[coAleatoire.x, coAleatoire.y];
        //return listeRoom[UnityEngine.Random.Range(0, listeRoom.Count)];
    }

    // co min = 4,4 coMax = 8,5
    // 8,5 - 4,4 = 4,1
    // 4 +4 = 8
    // 4+1 = 5
    // on récupére les case entre 4,4 et 8,4 ensuite entre 8,4 et 8,5
    List<IndexGrid> GetCoPathBetweenTwoCoo(IndexGrid coMin, IndexGrid coMax)
    {
        List<IndexGrid> result = new List<IndexGrid>();
        int ecartX = coMax.x - coMin.x;
        int ecartY = coMax.y - coMin.y;

        //Debug.Log("comin = " + coMin +  ", comax = " + coMax + ", ecart = " + ecartX + "," + ecartY);

        List<IndexGrid> lignevertical = GetAllCoBetweenMinMaxCo(coMin, new IndexGrid(coMin.x + ecartX , coMin.y));
        List<IndexGrid> ligneveHorizontal = GetAllCoBetweenMinMaxCo(new IndexGrid(coMin.x + ecartX, coMin.y), new IndexGrid(coMin.x + ecartX , coMin.y + ecartY));

        //Debug.Log("lignevertical = " + lignevertical.IsUnityNull() + "," + lignevertical.Count);
        //Debug.Log("ligneveHorizontal = " + ligneveHorizontal.IsUnityNull() + "," + ligneveHorizontal.Count);
        //Debug.Log("ligneveHorizontal = " + ligneveHorizontal.ToString());

        result.AddRange(lignevertical);
        result.AddRange(ligneveHorizontal);


        return result;
    }

    void SetRoomInAllCoList(IEnumerable<IndexGrid> coList, TypeSalle type = TypeSalle.Combat) // IEnumerable<IndexGrid> permet d'accpeter a la fois les listn et les array ainsi que d'autre...
    {
        foreach (var item in coList)
        {
            if(!map[item.x, item.y].IsActive())
            {
                map[item.x, item.y].SetActive(true);
                map[item.x, item.y].SetSalleType(type);
            }
            
        }
    }

    List<Room> GetAllActivedRoom(params TypeSalle[] typeFiltre) // le parametre TypeFiltre n'est pas obligatoire
    {
        List < Room > result = new List<Room>();
        foreach (var item in map)
        {
            if (item.IsActive())
            {
                if (typeFiltre == null || typeFiltre.Length == 0 || typeFiltre.Contains(item.GetSalleType()) )
                {
                    result.Add(item);
                }
                
            }
        }

        return result;
    }
    List<Room> GetEmptyRoomWithNeighbor(params TypeSalle[] typeFiltre)
    {
        List<Room> allRooms = GetAllActivedRoom(typeFiltre);
        List<Room> allInactiveNeighbor = new List<Room>();
        
        Room[] Neighbors = new Room[8];
        

        // on parcours toutes les salle en récupérent leur voisin inactif
        foreach (var room in allRooms)
        {

            Neighbors = GetAllCloseVoisins(room.IndexInMap);

            foreach (var item in Neighbors)
            {
                if (item != null && !item.IsActive())
                {
                    allInactiveNeighbor.Add(item);
                }
            }
        }

        return allInactiveNeighbor;
    }


    List<Room> GetEmptyRoomWithOneNeighbor()
    {

        List<Room> result = new List<Room>();
        List<Room> allInactiveNeighbor = GetEmptyRoomWithNeighbor(TypeSalle.Combat, TypeSalle.Spawn);
        int nbrVoisinActive = 0;

        // vérifie si il a qu'un voisin
        foreach (var room in allInactiveNeighbor) {
            nbrVoisinActive = 0;
            
            // compte les voisin
            foreach (var item in GetAllCloseVoisins(room.IndexInMap))
            {
                if (item != null && item.IsActive())
                {
                    nbrVoisinActive++;
                }
            }
            // ajoute a la liste si il n'a qu'un voisin
            if (nbrVoisinActive == 1)
            {
                result.Add(room);
            }
        }
        
        return result;
    }

    void SpawnRoom(IndexGrid index, Room salle ,TypeSalle type)
    {
        Debug.Log(type.ToString());
        Debug.Log(Array.IndexOf(typeSalleParIndex, type));
        if (type != TypeSalle.Bloquer)
        {
            GameObject room = prefabSalle[Array.IndexOf(typeSalleParIndex, type)];


            Vector3 posRoom = new Vector3(index.x * room.transform.localScale.x * 1.4f, index.y * room.transform.localScale.y * 1.5f, 0);
            GameObject roomIntstance = Instantiate(room, posRoom, Quaternion.identity);

            SpawnObstacle(type, posRoom);
            DictInstanciateRooms.Add(index, roomIntstance);
            //Instantiate(prefabSalle[Array.IndexOf(typeSalleParIndex, type)],
            //    new Vector3(index.x * ecartEntreSallX, index.y * ecartEntreSallY, 0),
            //    Quaternion.identity);
        }


    }

    void SpawnObstacle(TypeSalle type , Vector3 posRoom) {
        if (type == TypeSalle.Combat)
        {
            Instantiate(obstacles[UnityEngine.Random.Range(0, obstacles.Length)], posRoom, Quaternion.identity);
        }
    }

    void SpawnAllRoomInScene()
    {
        
        foreach (var item in GetAllActivedRoom())
        {
            SpawnRoom(item.IndexInMap,item,item.GetSalleType());
        }
        SetAllValueOfAllPrefabRoom();
    }

    void SetAllValueOfAllRoom()
    {
       List<Room> lesRooms = GetAllActivedRoom();
        foreach (var item in lesRooms)
        {
            foreach (Direction i in Enum.GetValues(typeof(Direction)))
            {
                item.SetNeighbor(i, getVoisin(item.IndexInMap, i));
            }
        }
    }

    void SetAllValueOfAllPrefabRoom()
    {
        foreach (var item in DictInstanciateRooms)
        {
            Door[] lesPorte = item.Value.GetComponentsInChildren<Door>();
            foreach (var porte in lesPorte)
            {
                if (DictInstanciateRooms.TryGetValue(getVoisin(item.Key, porte.direction).IndexInMap, out GameObject cible)) // ages.TryGetValue("Alice", out int age)
                {
                    porte.roomCible = cible;// getVoisin(item.key, porte.direction).IndexInMap
                }
                else
                {
                    porte.gameObject.SetActive(false);
                }

            }
        }
    }

}





























// renvoie une co aléatoire a partir d'une liste de liste de co 
//Vector2 GetRandomCoInListRange(List<List<Vector2>> listeCo)
//{
//    List<Vector2> fusion = new List<Vector2>();

//    foreach (var subList in listeCo)
//    { fusion.AddRange(subList); }


//    return fusion[UnityEngine.Random.Range(0, fusion.Count)];
//}

//// renvoie une room aléatoire a partir d'une liste de liste de co 
//Room GetRandomRoomInListRange(List<List<Vector2>> listeCo)
//{
//    List<Vector2> fusion = new List<Vector2>();

//    foreach (var subList in listeCo)
//    { fusion.AddRange(subList); }

//    Vector2 coAleatoire = fusion[UnityEngine.Random.Range(0, fusion.Count)];
//    return map[coAleatoire.x , coAleatoire.y];
//}