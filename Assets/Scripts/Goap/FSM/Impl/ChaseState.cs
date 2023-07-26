using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class ChaseState : MonoBaseState {

    public override event Action OnNeedsReplan;

    public float speed = 4f;
    public float _radiusAvoidance;
    public float _weightAvoidance;
    public float rangeDistance = 5;
    public float meleeDistance = 1.5f;
    Boss _agent;
    public float radius;
    public float weight;
    public LayerMask obs;
    private PlayerModel _player;
    private GameObject _item;
    

    private void Awake()
    {
        _agent = GetComponent<Boss>();
        _player = FindObjectOfType<PlayerModel>();
        
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _agent.Notify("moving_On");
    }
    public override void UpdateLoop() {

        //Debug.Log("ESTOY EN CHASE");

        _item = _agent.item;
        var dir = (_player.transform.position - transform.position);
        transform.position += dir.normalized * speed  * Time.deltaTime + ObtacleAvoidDance();
        var forward = transform.forward;
        transform.forward = Vector3.Slerp(forward, dir, 2 * Time.deltaTime);



        if (_item != null)
        {

            float DistanceItem = (_item.transform.position - transform.position).magnitude;
        
            if (dir.magnitude > DistanceItem)
            {
                _agent.Notify("moving_Off");
                OnNeedsReplan?.Invoke();
            }
        }

        if (!_agent.InSight()) {
            //_agent.Notify("moving_Off");
            OnNeedsReplan?.Invoke();
        }
    }

    public override IState ProcessInput() {
        var Distance = (_player.transform.position - transform.position).magnitude;
        float DistanceItem=0;

        if (_item != null)
        {
            DistanceItem = (_item.transform.position - transform.position).magnitude;
            if (DistanceItem < Distance && Transitions.ContainsKey("OnPickItemState"))
            {
                GetComponent<Boss>().Notify("moving_Off");
                return Transitions["OnPickItemState"];
            }
        }

        if (Distance < meleeDistance && Transitions.ContainsKey("OnMeleeAttackState"))
        {
            GetComponent<Boss>().Notify("moving_Off");
            return Transitions["OnMeleeAttackState"];
        }

        return this;
    }

    Vector3 ObtacleAvoidDance()
    {
        Vector3 dir = Vector3.zero;



        Collider[] obstacles = Physics.OverlapSphere(transform.position, _radiusAvoidance, obs);
        if (obstacles.Length > 0)
        {
            float distance = Vector3.Distance(obstacles[0].transform.position, transform.position);
            int indexSave = 0;
            for (int i = 1; i < obstacles.Length; i++)
            {
                float currDistance = Vector3.Distance(obstacles[i].transform.position, transform.position);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    indexSave = i;
                }
            }
            Vector3 dirFromObs = (transform.position - obstacles[indexSave].transform.position).normalized *
                (_radiusAvoidance - Mathf.Clamp((distance), 0, _radiusAvoidance)) / _radiusAvoidance * _weightAvoidance;
            dir += dirFromObs;
        }

        return dir;
    }

}