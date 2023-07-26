using System;
using UnityEngine;
using FSM;
using System.Collections.Generic;

public class LookState : MonoBaseState {

    
    Enemy _enemy;
    float currentTime;
    public float TimeToStartLaunchGrenades;
    float _rotationSpeed=0.9f;
    Transform _player;

    private void Awake() {
        
        _enemy=GetComponent<Enemy>();
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        currentTime = TimeToStartLaunchGrenades;
        _player = _enemy.player;
        GetComponent<Boss>().Notify("look");

    }
    public override void UpdateLoop() {
        //TODO: patrullo
        currentTime -= Time.deltaTime;
        Vector3 dir = transform.forward;
        if (currentTime < 0)
        {
            Vector3 playerDir = new Vector3(_player.position.x - transform.position.x,
                                            0,
                                            _player.position.z - transform.position.z);
            transform.forward=Vector3.Slerp(dir, playerDir ,_rotationSpeed*Time.deltaTime);

        }
       
    }
    

    public override IState ProcessInput() {
        

        if (currentTime<-3 && Transitions.ContainsKey("OnChaseState"))
        {
            return Transitions["OnChaseState"];
        }
        if (Transitions.ContainsKey("OnPickAmmo"))
        {
            return Transitions["OnPickAmmo"];
        }
         
            return this;
    }
}
