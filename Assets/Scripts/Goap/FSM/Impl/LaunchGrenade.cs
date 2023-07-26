using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LaunchGrenade : MonoBaseState
{
    public override event Action OnNeedsReplan;
    Enemy _owner;
    PlayerModel player;
    public Grenade grenade;
    ObjPool<Grenade> pool;
    public float attackRate;
    float currentTime;

    private void Awake()
    {
        player = FindObjectOfType<PlayerModel>();
       // pool = new ObjPool<Grenade>(Factory, Grenade.TurnOnStatic, Grenade.TurnOffStatic, 1, true);
        
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        
        currentTime = 0;
    }
    public override IState ProcessInput()
    {
        return this;
    }

    public override void UpdateLoop()
    {
        if (currentTime < attackRate)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            GetComponent<Boss>().Notify("Throw");
        }
        
    }

    
    public void ThrowGrenade()
    {
        //pool.GetObj().SetOwner(this).SetPlayer(player.transform);
        Instantiate(grenade, transform.position + new Vector3(0, 2, 0), Quaternion.identity).SetOwner(this).SetPlayer(player.transform);
        currentTime = 0;
        OnNeedsReplan?.Invoke();
        
    }
    public void Recycle(Grenade grenade)
    {
        pool.Recycle(grenade);
    }
    public Grenade Factory()
    {
        return Instantiate(grenade, transform.position, Quaternion.identity).SetOwner(this).SetPlayer(player.transform);
    }
}
