using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static Collectible;

[Serializable]
public struct IndexMinMax
{
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
}
[Serializable]
public struct obstacleValue
{
    public GameObject prefab;
    public Direction[] directionsNeed;
}



public class GenerationProceduralManager : MonoBehaviour
{
    // array a 2 dimension permettant de stocker la dispositon de l'atage sous forme theorique a l'aide de la classe Room
    public Room[,] map = new Room[9,9];


    public Dictionary<IndexGrid, GameObject> DictInstanciateRooms = new Dictionary<IndexGrid, GameObject>();

    // stocke tout les obstacle possible a mettre sur le salle
    public GameObject[] obstacles;

    // double array liant un type de salle et un prefab
    public TypeSalle[] typeSalleParIndex;
    public GameObject[] prefabSalle;

    //public obstacleValue[] obstacleValues;

    // array de coordonée délimitant la zonne où peut spawn la salle de boss
    public IndexMinMax[] zoneSalleBoss;


    public GameObject wall;

    // écart entre le spawn des prefab de salle
    public float ecartEntreSallX = 50;

    public float ecartEntreSallY = 40;

   
    // salle en plus qui spawneront aprés la constrcution du chemin et du spawn de la salle item
    public int nbrSalleEnPlus = 0;

    

    IndexGrid SpawnCo;

    IndexGrid BossCo;

    public GameObject player;

    private static GenerationProceduralManager instance;

    //void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Debug.LogError("GenerationProceduralManager dupliqué — destruction");
    //        Destroy(gameObject);
    //        return;
    //    }
    //    instance = this;
    //}

    void Start()
    {
        // Fil d'éxécution construisant le niveau pas a pas
        initialization();
        placingSpecialRoom();
        placingPathwayRoom();
        instanciationRoomInScene();

    }

    
    void initialization()
    {
        InitializeMapDefault();
        //UnityEngine.Random.state = new UnityEngine.Random.State();
        if (GameManager.instance.seed != "" && !GameManager.instance.seed.IsUnityNull())
        {
            UnityEngine.Random.InitState(GameManager.instance.seed.GetHashCode());
            //int hash = seedSTR.GetHashCode();
        }
        
    }

    // place la room spawn et boss
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

    // place le chemin de room entre boss et spawn ainsi que la salle item (qui doit n'avoir qu'un seule voisin)
    void placingPathwayRoom()
    {
        List<IndexGrid> pathCo = GetCoPathBetweenTwoCoo(SpawnCo, BossCo);
        SetRoomInAllCoList(pathCo);

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
        int maxTry = 50;
        int tryCount = 0;

        while (nbrSallecreer < nbrSalleEnPlus && tryCount < maxTry)
        {
            tryCount++;

            Room combatRoom = GetRandomRoomInListRange(neighbors);
            if (!combatRoom.IsActive())
            {
                combatRoom.SetActive(true);
                combatRoom.SetSalleType(TypeSalle.Combat);
                nbrSallecreer++;
            }
        }

        if (tryCount >= maxTry)
        {
            Debug.LogWarning("Impossible de placer toutes les salles combat");
        }
    }

    // Attribue les vlauer néçésaire a chaque room puis instantiate tout les prefab des room
    void instanciationRoomInScene()
    {
        
        SetAllValueOfAllRoom();
        SpawnAllRoomInScene();
        SetAllValueOfAllPrefabRoom();
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

    // Récupère les 8 Room voisins autour d’une case (si hors limite → null)
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
    // récupére la room voisin de la coordonée donner en focntion de la direction voulue
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

    // permet de fusionner des liste de IndexGrid en une seule liste
    List<IndexGrid> FusionneListe(List<List<IndexGrid>> listeCo)
    {
        List<IndexGrid> fusion = new List<IndexGrid>();

        foreach (var subList in listeCo)
        { fusion.AddRange(subList); }
        
        return fusion;
    }

    // renvoie une room aléatoire a partir d'une liste de room
    Room GetRandomRoomInListRange(List<Room> listeRoom)
    {
       
        return listeRoom[UnityEngine.Random.Range(0, listeRoom.Count)];
    }
    // renvoie une room aléatoire a partir d'une liste de co
    Room GetRandomRoomInListRange(List<IndexGrid> listeRoom)
    {
        IndexGrid coAleatoire = listeRoom[UnityEngine.Random.Range(0, listeRoom.Count)];
        return map[coAleatoire.x, coAleatoire.y];
        //return listeRoom[UnityEngine.Random.Range(0, listeRoom.Count)];
    }


    // on récupére un chemin de coordoné entre 2 co
    List<IndexGrid> GetCoPathBetweenTwoCoo(IndexGrid coMin, IndexGrid coMax)
    {
        List<IndexGrid> result = new List<IndexGrid>();
        int ecartX = coMax.x - coMin.x;
        int ecartY = coMax.y - coMin.y;



        List<IndexGrid> lignevertical = GetAllCoBetweenMinMaxCo(coMin, new IndexGrid(coMin.x + ecartX , coMin.y));
        List<IndexGrid> ligneveHorizontal = GetAllCoBetweenMinMaxCo(new IndexGrid(coMin.x + ecartX, coMin.y), new IndexGrid(coMin.x + ecartX , coMin.y + ecartY));


        result.AddRange(lignevertical);
        result.AddRange(ligneveHorizontal);


        return result;
    }
    // active les room de chaque coordonée en leur donnant leur type
    void SetRoomInAllCoList(IEnumerable<IndexGrid> coList, TypeSalle type = TypeSalle.Combat) // IEnumerable<IndexGrid> permet d'accpeter a la fois les listn et les array ainsi que d'autre...
    {
        foreach (var item in coList)
        {
            // Sécurité : hors grille
            if (item.x < 0 || item.y < 0 || item.x >= map.GetLength(0) || item.y >= map.GetLength(1))
            {
                Debug.LogWarning($"Coordonnée hors grille ignorée : {item.x}, {item.y}");
                continue;
            }

            Room room = map[item.x, item.y];

            if (!room.IsActive())
            {
                room.SetActive(true);
                room.SetSalleType(type);
            }
        }
    }

    // permet de récupérer toute les salle active avec un filtre de type possible
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
    // récupére toute les salle inactive qui ont une salle active comme voisin
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

    // récupére les salle inactive qui ont qu'un seule voisin
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

    // permet de spawn le prefab dde la room corespondante dans la scéne 
    void SpawnRoom(IndexGrid index, Room salle ,TypeSalle type)
    {
        if (DictInstanciateRooms.ContainsKey(index))
            return;
        
        if (type != TypeSalle.Bloquer)
        {
            GameObject room = prefabSalle[Array.IndexOf(typeSalleParIndex, type)];


            Vector3 posRoom = new Vector3(index.x * room.transform.localScale.x * 1.4f, index.y * room.transform.localScale.y * 1.5f, 0);
            GameObject roomIntstance = Instantiate(room, posRoom, Quaternion.identity);
            RoomManager RMScript = roomIntstance.GetComponent<RoomManager>();
            SpawnObstacle(type, posRoom, salle, RMScript);
            DictInstanciateRooms.Add(index, roomIntstance);
            
        }


    }


    // spawn un obstacle aléatoire au coordonée de la salle en focntion de quelle porte ouverte la salle ai
    void SpawnObstacle(TypeSalle type, Vector3 posRoom, Room salle, RoomManager scriptPrefab)
    {
       

        if (type != TypeSalle.Combat)
            return;

        //évite les boucle infinie
        const int MAX_TRY = 20;
        int tryCount = 0;
        
        while (tryCount < MAX_TRY)
        {
            tryCount++;

            bool isInvalid = false;

            GameObject obstacleChoisis = obstacles[UnityEngine.Random.Range(0, obstacles.Length)];
            obstacleManager obstacleScript = obstacleChoisis.GetComponent<obstacleManager>();

            // vérifie si la salle a des chemin ouvert qui serais bloquer par l'obstacle
            foreach (var item in obstacleScript.cheminBloquer)
            {
                if (salle.Voisins.ContainsKey(item))
                {
                    isInvalid = true;
                    break;
                }
            }

            if (!isInvalid)
            {
                Instantiate(obstacleChoisis, posRoom, Quaternion.identity);

                SpawnEnnemis(obstacleScript, scriptPrefab, posRoom);
                return;
            }
        }

        Debug.LogWarning("Aucun obstacle valide trouvé pour cette salle");
    }
    // spawn un ennemis aléatoire piocher dans les enseble d'ennemis rentrer dans l'ensenble d'obstacle
    void SpawnEnnemis(obstacleManager obstacleScript, RoomManager scriptPrefab, Vector3 posRoom)
    {
        GameObject ennemis = Instantiate(obstacleScript.ensenbleDEnnemisPossible[UnityEngine.Random.Range(0, obstacleScript.ensenbleDEnnemisPossible.Length)], posRoom, Quaternion.identity);

        SetPlayerInAI(ennemis);
        SetRoomOwnerInAi(ennemis, scriptPrefab);
        ennemis.SetActive(false);
        scriptPrefab.ennemis = ennemis;
    }
    // donne a l'ia le script de la salle ou il spawn
    void SetRoomOwnerInAi(GameObject ennemis,RoomManager roomScript)
    {
        Ennemie_Health[] ennemie_Health = ennemis.GetComponentsInChildren<Ennemie_Health>();
        if (ennemie_Health.Length > 0)
        {
            foreach (Ennemie_Health ennemi in ennemie_Health)
            {
                ennemi.RoomOwner = roomScript;
            }
        }
    }
    // donne a chaque script de l'ennemis le player ( pas opti il faudrait que je repense gloabalement cette partie)
    void SetPlayerInAI(GameObject ennemis)
    {
        // Shooter
        Enemy_Shooter[] shooters = ennemis.GetComponentsInChildren<Enemy_Shooter>();
        if (shooters.Length > 0)
        {
            foreach (Enemy_Shooter shooter in shooters)
            {
                //shooter.player = player.transform;
                shooter.player = GameObject.FindWithTag("Player").transform;
            }
        }

        // Fuyard
        AIEnnemisFuyeur[] fuyards = ennemis.GetComponentsInChildren<AIEnnemisFuyeur>();
        if (fuyards.Length > 0)
        {
            foreach (AIEnnemisFuyeur fuyard in fuyards)
            {
                fuyard.target = GameObject.FindWithTag("Player").transform;
            }
        }

        // Suiveur
        AIEnnemisSuiveur[] suiveurs = ennemis.GetComponentsInChildren<AIEnnemisSuiveur>();
        if (suiveurs.Length > 0)
        {
            foreach (AIEnnemisSuiveur suiveur in suiveurs)
            {
                suiveur.target = GameObject.FindWithTag("Player").transform;
            }
        }
    }
    // parcours tout les room active pour placer leur prefab
    void SpawnAllRoomInScene()
    {
        
        foreach (var item in GetAllActivedRoom())
        {
            
            SpawnRoom(item.IndexInMap,item,item.GetSalleType());
        }
        
    }

    // enregistre les voisin de chaque room dans leur script
    void SetAllValueOfAllRoom()
    {
        Debug.Log("set valeur port");
       List<Room> lesRooms = GetAllActivedRoom();
        foreach (var item in lesRooms)
        {
            foreach (Direction i in Enum.GetValues(typeof(Direction)))
            {
                //Debug.Log("for each set valeur port");
                //item.SetNeighbor(i, getVoisin(item.IndexInMap, i));

                Room voisin = getVoisin(item.IndexInMap, i);

                if (voisin != null && voisin.IsActive())
                {
                    item.SetNeighbor(i, voisin);
                }
            }
            if (item.GetSalleType() == TypeSalle.Boss)
            {
                foreach (var item1 in item.Voisins)
                {
                    Debug.Log("boss vosiin  = " +item1.Key);
                }
            }
        }
    }

    // parcours chaque prefab pour activer ou non leur porte en fonction de leur voisin
    void SetAllValueOfAllPrefabRoom()
    {
        RoomManager roomM;
        foreach (var item in DictInstanciateRooms)
        {
            roomM = item.Value.GetComponent<RoomManager>();
            Door[] lesPorte = item.Value.GetComponentsInChildren<Door>();
            for (int i = 0; i < lesPorte.Length; i++)
            {
                Door porte = lesPorte[i];
                Room voisin = getVoisin(item.Key, porte.direction);

                if (voisin != null &&
                    DictInstanciateRooms.TryGetValue(voisin.IndexInMap, out GameObject cible))
                {
                    porte.roomCible = cible;
                    roomM.wallsDoor[i].SetActive(false);
                }
                else
                {
                    porte.gameObject.SetActive(false);
                    roomM.wallsDoor[i].SetActive(true);
                }
            }
        }
    }

    // mélange un array
    public GameObject[] RandomizeArray(GameObject[] array)
    {
        GameObject[] result = new GameObject[array.Length];
        
        List<int> arrayOfIndex = new List<int>();
        int indexChoisis;
        for (int i = 0; i < array.Length; i++)
        {
            arrayOfIndex.Add(i);
        }

        for (int i = 0; i < array.Length ; i++)
        {
            
            indexChoisis = arrayOfIndex[UnityEngine.Random.Range(0, arrayOfIndex.Count)];
            //Debug.Log("array fo index : " + i + " . " );
            result[i] = array[indexChoisis];
            arrayOfIndex.RemoveAt(arrayOfIndex.IndexOf(indexChoisis));
        }

        return result;


    }

}
