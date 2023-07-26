using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AttackState<T> : State<T> 
{
    float _damage;
    float _attackCadence;    
    float _currentTime;
    BoxQuery boxQuery;
    Enemy _owner;


    public AttackState(float cadence,Enemy enemy)
    {        
        _attackCadence = cadence;        
        _owner = enemy;
        _damage = _owner.damageMelee;
        boxQuery = _owner.boxQuery;
        
    }
    public override void OnEnter()
    {
        _currentTime = 0;
        _owner.Notify("Punch");
    }
    public override void OnUpdate()
    {
        if (_owner.canAttack)
        {
            var player = Attack();
            if (player != null)
            {
                player.Damaged(_damage);
                _owner.canAttack = false;
            }
        }
        if (_currentTime < _attackCadence)        
            _currentTime += Time.deltaTime;        
        else
            Transition();
        
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    public override void OnSleep()
    {
        base.OnSleep();
    }
    //IA2-P2
    public PlayerModel Attack()
    {
        return boxQuery.Query().OfType<PlayerModel>().Where(player => player.Life > 0).FirstOrDefault(); //IA2-P3
    }

    void Transition()
    {  
        _currentTime = 0;
        _owner.TransitionSt("Walk");
        
    }
}
