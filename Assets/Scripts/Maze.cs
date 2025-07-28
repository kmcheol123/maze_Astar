using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class MapLocation
{

    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    public static MapLocation operator +(MapLocation a, MapLocation b) 
        => new MapLocation(a.x + b.x , a.z + b.z);          // '=>' 람다식임


    public override bool Equals(object obj)
    {
        if((obj == null)|| !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;  // class와 같은 오브젝트형을 형변환을 이룰때
        }
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>() { new MapLocation(1, 0), new MapLocation(0, 1), new MapLocation(-1, 0), new MapLocation(0, -1) };
    public int width = 30;
    public int depth = 30;
    public byte[,] map;     // 네트워크 겜 할떄나 byte 씀
    public int scale = 6;
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
    }
    void InitialiseMap()
    {
        map = new byte[width, depth];
        for(int z = 0; z < depth; z++)
        {
            for(int x = 0; x < width; x++)
            {
                map[x, z] = 1;  //1은 벽, 0은 통로
            }
        }
    }
    public virtual void Generate()
    {
        //for (int z = 0; z < depth; z++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        if(Random.Range(0, 100) < 50)
        //        {
        //            map[x, z] = 0;      // 1= wall, 0 = corridor
        //        }
        //    }
        //}
    }
    void DrawMap()
    {
        for(int z =0; z < depth; z++)
        {
            for(int x = 0;x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale); 
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);   // unity에서 오브젝트를 생성할떄
                    wall.transform.localScale = new Vector3(scale, scale, scale);       
                    wall.transform.position = pos;
                }
            }
        }
    }
    public int ConutSquareNeighbours(int x, int z)      // 4방면으로 복도 검색
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if(map[x - 1, z] == 0) count++;
        if(map[x + 1, z] == 0) count++;
        if(map[x, z + 1] == 0) count++;
        if(map[x, z - 1] == 0) count++;
        return count;

    }

    void Update()
    {
        
    }
}
