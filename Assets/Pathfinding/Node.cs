using UnityEngine;


public class Node //IA2-P1
{
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public float hCost;
    public float gCost;
    public Node parent;
    public bool walkable,isPath;

    public Node(bool _walkable,bool _path, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        isPath = _path;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    
    public float FCost()
    {
        return gCost + hCost;
    }
    
}
