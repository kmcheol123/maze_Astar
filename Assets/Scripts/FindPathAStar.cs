using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathMarker 
{ 
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation I, float g, float h, float f, GameObject marker, PathMarker p)
    {
        location = I;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        parent = p;
    }
    public override bool Equals(object obj)
    {
        if((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return location.Equals(((PathMarker)obj).location);
        }
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;
    bool done = false;

    GameObject Unit;
    bool isMove = false;
    List<Vector3> movePath = new List<Vector3>();

    void RemoveAllMarkers()     // 태그에 마커라고 부는 마커를 전부 삭제 
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }
    void BeginSearch()      // 맵 내무에서 시작 포인트와 종료 포인트를 만들어낸다.
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();
        // 통로를 검색 
        for (int z = 1; z < maze.depth - 1; z++)
        {
            for (int x = 1; x < maze.depth - 1; x++)
            {
                if (maze.map[x, z] != 1)
                {
                    locations.Add(new MapLocation(x, z));
                }
            }
        }
        // 통로를 순서를 지정 
        locations.Shuffle();
        // 리스트를 첫번째를 시작지로 
        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startLocation, Quaternion.identity), null);
        // 리스트를 두번째를 시작지로 
        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        // 처음 시작되는 노드(통로)를 오픈 리스트에 삽입 
        open.Add(startNode);
        lastPos = startNode;
    }
    void Search(PathMarker thisNode)
    {
        // 마지막 또는 시작
        if (thisNode.Equals(goalNode))
        {
            done = true;
            return;
        }
        foreach (MapLocation dir in maze.directions)
        {
            // 현재 위치 값의 방향을 더해서 근처 노드를 찾아준다
            MapLocation neighbor = dir + thisNode.location;
            // 그 노드가 벽이라면 다음으로 이동
            if (maze.map[neighbor.x, neighbor.z] == 1)
            {
                continue;
            }
            // 그 노드가 테두리라면 다음으동 이동
            if (neighbor.x < 1 || neighbor.x >= maze.width || neighbor.z < 1 || neighbor.z >= maze.depth)
            {
                continue;
            }
            // 그 노드가 이미 닫힌 목록에 들어가 있다면 다음으로 이동
            if (IsClosed(neighbor))
            {
                continue;
            }
            float G = Vector2.Distance(thisNode.location.ToVector(), neighbor.ToVector()) + thisNode.G;
            float H = Vector2.Distance(neighbor.ToVector(), goalNode.location.ToVector());
            float F = G + H;
            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbor.x * maze.scale, 0, neighbor.z * maze.scale), Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G : " + G.ToString("0.00");
            values[1].text = "H : " + H.ToString("0.00");
            values[2].text = "F : " + F.ToString("0.00");
            // 근처 노드가 이미 열린 목록에 포함되어 있는지 확인(되어 있다면 값을 갱신)
            if (!UpdateMarker(neighbor, G, H, F, thisNode)) ;
            {
                // 열린 목록에 포함되어 있지 않다면 열린 목로에 포함시킨다
                open.Add(new PathMarker(neighbor, G, H, F, pathBlock, thisNode));
            }
        }
        //열린 목록을 오름차순 정렬
        open = open.OrderBy(p => p.F).ToList<PathMarker>();
        // 열린 목록에 있는 노드 중 F가 가장 작은 값을 선정한다.
        PathMarker pm = (PathMarker)open.ElementAt(0);
        // 닫힌 목록으로 추가
        closed.Add(pm);
        // 닫힌 목록에 추가한 열린 목록 노드를 삭제
        open.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        lastPos = pm;
    }
    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }
    bool IsClosed(MapLocation marker)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(marker))
            {
                return true;
            }
        }
        return false;
    }
    void GetPath()
    {
        RemoveAllMarkers();
        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {
            Instantiate(pathP, new Vector3(begin.location.x * maze.scale, 0, begin.location.z * maze.scale), Quaternion.identity);
            begin = begin.parent;
        }
        Instantiate(pathP, new Vector3(startNode.location.x * maze.scale, 0, startNode.location.z * maze.scale), Quaternion.identity);

    }
    void SetMovePath()
    {
        PathMarker begin = lastPos;
        while (!startNode.Equals(begin) && begin != null)
        {
            movePath.Add(new Vector3(begin.location.x * maze.scale, 0, begin.location.z * maze.scale));
            begin = begin.parent;
        }
        movePath.Add(new Vector3(startNode.location.x * maze.scale, 0, startNode.location.z * maze.scale));
        movePath.Reverse();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }
        if (Input.GetKeyDown(KeyCode.C) && !done)
        {
            Search(lastPos);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GetPath();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetMovePath();
            Unit = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Unit.transform.position = movePath[0];
            isMove = true;
        }
        if (isMove)
        {
            if (movePath.Count > 0)
            {
                if (Vector3.Distance(Unit.transform.position, movePath[0]) > 0.01f)
                {
                    Unit.transform.position = Vector3.MoveTowards(Unit.transform.position, movePath[0], 1.0f * maze.scale * Time.deltaTime);
                }
                else
                {
                    Unit.transform.position = movePath[0];
                    movePath.RemoveAt(0);
                }
            }
            else
            {
                isMove = false;
            }
        }

    }
}

