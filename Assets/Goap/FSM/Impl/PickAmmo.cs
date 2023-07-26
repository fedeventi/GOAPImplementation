using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class PickAmmo : MonoBaseState
{

    public GameObject _item;
    Boss agent;
    public override event Action OnNeedsReplan;
    float currentTime;
    Transform player;
    private void Awake()
    {
        agent = GetComponent<Boss>();

    }


    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        agent.item = agent.NearestAmmo(agent.transform);
        _item = agent.item;
        currentTime = 3;
        Debug.Log("busco item");
        player = agent.player;
        if(_item!=null)
            transform.LookAt(_item.transform);
        agent.Notify("moving_On");
    }

    public override void UpdateLoop()
    {



        if (_item == null) {
            Debug.Log("there are no items");
            OnNeedsReplan?.Invoke();
            return;
        }  

        var dir = (_item.transform.position - transform.position);

        if (dir.magnitude <= .5f)
        {
            agent.hasBullet = true;
            _item.transform.parent.GetComponent<AmmoSpawner>().Recycle(_item.GetComponent<Ammo>());
            agent.Notify("moving_On");
            OnNeedsReplan?.Invoke();
        }
        transform.position += dir.normalized * (10 * Time.deltaTime);

    }

    public override IState ProcessInput()
    {
        if(agent.hasBullet)
        if (_item!=null && Transitions.ContainsKey("OnRangeAttackState"))
        {
                agent.Notify("moving_Off");
                return Transitions["OnRangeAttackState"];
        }
        if (Vector3.Distance(player.position, transform.position) < DistItem)
        {
            if (Transitions.ContainsKey("OnChaseState"))
            {
                Debug.LogError("entre");
                return Transitions["OnChaseState"];
            }
        }
       
       
        return this;
    }
    public float DistItem
    {
        get { 
                if(_item==null)
                    return int.MaxValue;
            return Vector3.Distance(_item.transform.position,transform.position);
        }
    }
}
