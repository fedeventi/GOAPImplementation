using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyTest : MonoBehaviour
{

    public float speed;
    public Grid grid;
    public List<Vector3> waypoints = new List<Vector3>();
    public int currentNode = 0;

    public Transform playerPos;

    public bool kill;

    public void Aweke()
    {


    }

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    // esto seria el execute de la state machine
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {
            waypoints.Clear();
            currentNode = 0;
            kill = false;
            var pj = FindObjectOfType<PlayerModel>();
            playerPos = pj.gameObject.transform;
            AgentAStar.instance.seeker = transform;
            AgentAStar.instance.target = playerPos;
            AgentAStar.instance.FindPath(transform.position, FindObjectOfType<PlayerModel>().transform.position);
            currentNode = 0;

            Debug.Log(grid.path.Count);

            for (int i = 0; i < grid.path.Count; i++)
            {
                waypoints.Add(grid.path[i].worldPosition);
            }

            Debug.Log(playerPos.position);

        }


        if (playerPos != null && !kill)
        {
            var _currentNode = waypoints[currentNode];
            //var direction = _currentNode - transform.position;
            var direction = new Vector3(_currentNode.x, transform.position.y, _currentNode.z) - transform.position;

            transform.position += direction.normalized * speed * Time.deltaTime;
            if (direction.magnitude <= .1)
            {
                currentNode++;
                if (currentNode >= waypoints.Count)
                {
                    kill = true;
                    waypoints.Clear();
                    currentNode = 0;
                }
            }

            transform.forward = Vector3.Slerp(transform.forward, direction.normalized, 0.2f);

        }


    }


    private void OnDrawGizmos()
    {
        if (waypoints.Any())
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.DrawSphere(waypoints[i], .2f);
                if (i < waypoints.Count - 1)
                    Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(waypoints[currentNode], .3f);
        }


    }

}
