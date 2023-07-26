using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AgentAStar : MonoBehaviour //IA2-P1
{

    public Transform seeker, target;
    Grid grid;

    static AgentAStar _instance;
    public static AgentAStar instance { get { return _instance; } }


    void Awake()
    {
        _instance = this;
        grid = FindObjectOfType<Grid>();
    }


    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        List<Node> slopes = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
        slopes.Add(startNode);

        while (slopes.Any())
        {
            Node node = slopes[0];
            for (int i = 1; i < slopes.Count; i++)
            {
                if (slopes[i].FCost() < node.FCost() || slopes[i].FCost() == node.FCost())
                {
                    if (slopes[i].hCost < node.hCost)
                        node = slopes[i];
                }
            }

            slopes.Remove(node);
            visited.Add(node);

            if (node == targetNode)
            {
                ContructPath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || visited.Contains(neighbour))
                {
                    continue;
                }

                float newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !slopes.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!slopes.Contains(neighbour))
                        slopes.Add(neighbour);

                }
            }
        }
    }


    void ContructPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

    }

    float GetDistance(Node nodeA, Node nodeB)
    {
        int xDist = Mathf.Abs(Mathf.Abs(nodeA.gridX - nodeB.gridX));
        int yDist = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (xDist > yDist)
            return 14 * yDist + 10 * (xDist - yDist);
        return 14 * xDist + 10 * (yDist - xDist);
    }

    public AgentAStar SetTarget(Transform tgt)
    {
        target = tgt;
        return this;
    }

    public AgentAStar SetSeeker(Transform init)
    {
        seeker = init;
        return this;
    }


    void PathToClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (grid.path != null && grid.path.Count > 0)
            {
                grid.path.Clear();
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                FindPath(seeker.position, hit.point);
            }

        }
    }

}
