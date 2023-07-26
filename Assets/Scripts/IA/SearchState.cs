using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState <T> : State<T>
{
    float speed;
    Enemy _enemy;
    float distanceToAttack;
    Transform _player;
    int currentNode;
    List<Vector3> _waypoints=new List<Vector3>();
    
    // Start is called before the first frame update
    public SearchState(Enemy enemy,Transform player, float distancetoAttack)
    {
        _enemy = enemy;
        _player = player;
        _waypoints = enemy.waypoints;
        currentNode = enemy.currentNode;
        speed = enemy.speed;
        this.distanceToAttack = distancetoAttack;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _waypoints.Clear();
        AgentAStar.instance.seeker = _enemy.transform;
        AgentAStar.instance.target = _player;
        AgentAStar.instance.FindPath(_enemy.transform.position,_player.position);
        currentNode = 0;
        _enemy.Notify("Move");
        for (int i = 0; i < _enemy.grid.path.Count; i++)
        {
            _waypoints.Add(_enemy.grid.path[i].worldPosition);
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_enemy.InSight()) _enemy.TransitionSt("Walk");
        

        var _currentNode = _waypoints[currentNode];
            
            var direction = new Vector3(_currentNode.x, _enemy.transform.position.y, _currentNode.z) - _enemy.transform.position;

        _enemy.transform.position += direction.normalized * speed * Time.deltaTime;

        if (direction.magnitude <= .5f)
        {
            currentNode++;
            if (currentNode >= _waypoints.Count)
            {
                _waypoints.Clear();
                currentNode = 0;
                _enemy.TransitionSt("Search");
            }
        }

        _enemy.transform.forward = Vector3.Slerp(_enemy.transform.forward, direction.normalized, 0.2f);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
    }
    public override void OnSleep()
    {
        base.OnSleep();
        _enemy.Notify("Stand");
    }
}
