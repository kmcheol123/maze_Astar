//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[SerializeField]
//public class MapLocation
//{

//    public int x;
//    public int z;

//    //public MapLocation(int _x, int _z)
//    //{
//    //    x = _x;
//    //    z = _z;
//    //}

//}
//public class Maze : MonoBehaviour
//{
//    private static System.Random rng = new System.Random();
//    public List<MapLocation> directions = new List<MapLocation>();
//    public int width = 30;
//    public int depth = 30;
//    public byte[,] map;     // 네트워크 겜 할떄나 byte 씀
//    public int scale = 6;
//    void Start()
//    {
//        InitialiseMap();
//        Generate();
//        DrawMap();
//    }
//    void InitialiseMap()
//    {
//        map = new byte[width, depth];
//        for(int z = 0; z < depth; z++)
//        {
//            for(int x = 0; x < width; x++)
//            {
//                map[x, z] = 1;  //1은 벽, 0은 통로
//            }
//        }
//    }
//    public virtual void Generate()
//    {
//        //for (int z = 0; z < depth; z++)
//        //{
//        //    for (int x = 0; x < width; x++)
//        //    {
//        //        if(Random.Range(0, 100) < 50)
//        //        {
//        //            map[x, z] = 0;      // 1= wall, 0 = corridor
//        //        }
//        //    }
//        //}
//    }
//    void DrawMap()
//    {
//        for(int z =0; z < depth; z++)
//        {
//            for(int x = 0;x < width; x++)
//            {
//                if (map[x, z] == 1)
//                {
//                    Vector3 pos = new Vector3(x * scale, 0, z * scale); 
//                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);   // unity에서 오브젝트를 생성할떄
//                    wall.transform.localScale = new Vector3(scale, scale, scale);       
//                    wall.transform.position = pos;
//                }
//            }
//        }
//    }
//    public int CountSquareNeighbours(int x, int z)      // 4방면으로 복도 검색
//    {
//        int count = 0;
//        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
//        if(map[x - 1, z] == 0) count++;
//        if(map[x + 1, z] == 0) count++;
//        if(map[x, z + 1] == 0) count++;
//        if(map[x, z - 1] == 0) count++;
//        return count;

//    }
//    void SetMapLocation()
//    {
//        // 오른쪽
//        MapLocation location0 = new MapLocation();
//        location0.x = 1;
//        location0.z = 0;
//        directions.Add(location0);
//        // 위
//        MapLocation location1 = new MapLocation();
//        location1.x = 0;
//        location1.z = 1;
//        directions.Add(location1);
//        // 왼쪽
//        MapLocation location2 = new MapLocation();
//        location2.x = -1;
//        location2.z = 0;
//        directions.Add(location2);
//        // 아래
//        MapLocation location3 = new MapLocation();
//        location3.x = 0;
//        location3.z = -1;
//        directions.Add(location3);
//    }
//    public void Shuffle(List<MapLocation> mapLocations)
//    {
//        int n = mapLocations.Count;
//        while(n > 1)
//        {
//            n--;
//            int k = rng.Next(n + 1);
//            MapLocation value = mapLocations[k];
//            mapLocations[k] = mapLocations[n];
//            mapLocations[k] = value;
//        }
//    }
//    void GenerateList(int x, int z)
//    {
//        List<MapLocation> mapDatas = new List<MapLocation>();
//        map[x, z] = 0;
//        MapLocation mapData = new MapLocation();
//        mapData.x = x;
//        mapData.z = z;
//        mapDatas.Add(mapData);

//        while(mapDatas.Count > 0)
//        {
//            MapLocation current = mapDatas[0];
//            Shuffle(directions);
//            bool moved = false;

//            foreach(MapLocation dir in directions)
//            {
//                int changeX = current.x + dir.x;
//                int changeZ = current.z + dir.z;

//                if(!(CountSquareNeighbours(changeX, changeZ) >= 2 || map[changeX, changeZ] == 0))
//                {
//                    map[changeX, changeZ] = 0;
//                    MapLocation tempData = new MapLocation();
//                    tempData.x = changeX;
//                    tempData.z = changeZ;
//                    mapDatas.Insert(0,tempData);
//                    moved = true;
//                    break;
//                }
//            }
//            if (!moved)
//            {
//                mapDatas.RemoveAt(0);
//            }
//        }
//    }
//    void Update()
//    {
        
//    }
//}
