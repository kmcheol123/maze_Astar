using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        Generate(5, 5);
    }
    void Generate(int x, int z)
    {
        // 4방위 중 2방위 이상이 복도일경우  || 자신이 복도일 때를 추가
        if (ConutSquareNeighbours(x, z) >= 2 || map[x, z] == 0)
        {
            return;
        }
        map[x, z] = 0;

        directions.Shuffle();

        Generate(x + directions[0].x, z + directions[0].z); //6,5
        Generate(x + directions[1].x, z + directions[1].z); //5,6
        Generate(x + directions[2].x, z + directions[2].z); //4,5
        Generate(x + directions[3].x, z + directions[3].z); //5,4
    }

}
