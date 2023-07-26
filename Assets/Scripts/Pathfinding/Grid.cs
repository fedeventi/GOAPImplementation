using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour //IA2-P1
{
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;
    public LayerMask obstaclesMask;
    float nodeDiameter;
    public int gridSizeX, gridSizeY;
    public bool gizmosNodeColl, gizmosAreaWalkeable, gizmosPath;
    [Range(-.2f, .5f)]
    public float offestWalls;
    bool isPath = false;
    int cant0Neighbours, cant1Neighbours, cant2Neighbours, cant3Neighbours, cant4Neighbours, cant5Neighbours, cant6Neighbours, cant7Neighbours, cant8Neighbours;

    public List<Node> path;

    public bool debugNeighbours;

    private void Awake()
    {
        Bake();

    }

    public void Bake()
    {
        cant0Neighbours = 0;
        cant1Neighbours = 0;
        cant2Neighbours = 0;
        cant3Neighbours = 0;
        cant4Neighbours = 0;
        cant5Neighbours = 0;
        cant6Neighbours = 0;
        cant7Neighbours = 0;
        cant8Neighbours = 0;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y);
        CreateGrid();

        if (debugNeighbours)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (GetNeighbours(grid[x, y]).Count == 0) cant0Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 1) cant1Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 2) cant2Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 3) cant3Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 4) cant4Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 5) cant5Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 6) cant6Neighbours++;
                    else if (GetNeighbours(grid[x, y]).Count == 7) cant7Neighbours++;
                    else cant8Neighbours++;

                    Debug.Log($"Nodo {grid[x, y].worldPosition} : {GetNeighbours(grid[x, y]).Count}");
                }
            }
            Debug.Log($"[{cant0Neighbours}] Nodos con 0 vesinos | [{cant1Neighbours}] Nodos con   1 vesinos | [{cant2Neighbours}] Nodos con   2 vesinos" +
                $" | [{cant3Neighbours}] Nodos con   3 vesinos | [{cant4Neighbours}] Nodos con 4 vesinos | [{cant5Neighbours}] Nodos con   5 vesinos " +
                $" | [{cant6Neighbours}] Nodos con   6 vesinos | [{cant7Neighbours}] Nodos con   7 vesinos | [{cant8Neighbours}] Nodos con   8 vesinos");
        }

    }


    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.forward * gridSizeY / 2 - Vector3.right * gridSizeX / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter) + Vector3.forward * (y * nodeDiameter);
                bool walkable = !(Physics.CheckBox(worldPoint, Vector3.one * (nodeRadius + offestWalls), Quaternion.identity, obstaclesMask));
                grid[x, y] = new Node(walkable, isPath, worldPoint, x, y);
            }
        }

    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }



    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (grid[checkX, checkY].walkable)
                        neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    void OnDrawGizmos()
    {

        if (path != null && path.Count > 0 && gizmosPath)
        {
            foreach (Node n in path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(n.worldPosition, new Vector3(1, 0.02f, 1) * (nodeDiameter));
            }
        }


        if (grid != null && gizmosAreaWalkeable || gizmosNodeColl)
        {
            foreach (Node n in grid)
            {
                if (n.walkable && gizmosAreaWalkeable)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(n.worldPosition, new Vector3(1, 0.02f, 1) * (nodeDiameter));
                }
                else if (gizmosAreaWalkeable)
                {
                    Gizmos.color = new Color(0, 0, 0, 0);
                    Gizmos.DrawCube(n.worldPosition, new Vector3(1, 0.02f, 1) * (nodeDiameter));
                }

                if (gizmosNodeColl)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeRadius + offestWalls));
                }
            }
        }

    }

}
