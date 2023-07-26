using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class NormalEnemy : Enemy //IA2-P1
{
    // Start is called before the first frame update
    
    SeekState<string> seek;
    AttackState<string> attack;
    SearchState<string> search;
    DeathState<string> death;
    Rigidbody _rb;

    

    private void Awake()
    { 
        grid = FindObjectOfType<Grid>();
     
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        base.Start();
        Transitions();
        _stateMachine = new MyStateMachine<string>(attack);
    }

    public override void Update()
    {
        base.Update();
        _stateMachine.OnUpdate();
    }
    private void FixedUpdate()
    {
        _stateMachine.OnFixedUpdate();
    }
    protected override void Transitions()
    {
        base.Transitions();
        seek = new SeekState<string>(player, speed, this, distanceToAttack);
        search = new SearchState<string>(this, player, distanceToAttack);
        attack = new AttackState<string>(cadence, this);
        death = new DeathState<string>(this);
        seek.SetTransition("Search", search);
        seek.SetTransition("Attack", attack);
        seek.SetTransition("Death", death);
        attack.SetTransition("Walk", seek);
        attack.SetTransition("Search", search);
        attack.SetTransition("Death", death);
        search.SetTransition("Walk", seek);
        search.SetTransition("Attack", attack);
        search.SetTransition("Search", search);
        search.SetTransition("Death", death);
        death.SetTransition("Walk", seek);
    }


    private void OnDrawGizmos()
    {
        base.OnDrawGizmos();
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
