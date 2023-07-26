using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState<T> : State<T>
{
    
    Enemy _owner;
    HeavyEnemy _ownerHeavy;
    float _attackDuration;
    float _currentTime;
    public HeavyAttackState( Enemy enemy)
    {
        _ownerHeavy=enemy.GetComponent<HeavyEnemy>();
        _owner = enemy;
        _attackDuration = 2;
    }
    public override void OnEnter()
    {
        _currentTime = 0;
        _owner.Notify("Spit");
    }
    public override void OnUpdate()
    {
        if (_currentTime < _attackDuration)
        {
            _currentTime += Time.deltaTime;
            _owner.transform.LookAt(_owner.player);
        }
        else
            ThrowAcid();
        
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    public override void OnSleep()
    {
        base.OnSleep();
    }
    void ThrowAcid()
    {
        _currentTime = 0;
        //_ownerHeavy.ThrowAcid();
        _owner.TransitionSt("Walk");

    }

}
