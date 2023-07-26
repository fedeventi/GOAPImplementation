using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeekState<T> : State<T> 
{
    float _speed;
    Transform _player;
    Enemy _owner;
    Rigidbody _rb;
    Animator _anim;
    float distanceToAttack;


    public SeekState(Transform player,float speed, Enemy owner, float distanceToAttack)
    {
        _speed = speed;
        _player = player;
        _owner = owner;
        _rb = owner.GetComponent<Rigidbody>();
        _anim = owner.GetComponent<Animator>();
        this.distanceToAttack = distanceToAttack;
    }
    public override void OnEnter()
    {
        _owner.Notify("Move");
    }
    public override void OnUpdate()
    {
        if (!_owner.InSight()) _owner.TransitionSt("Search");
        if (Vector3.Distance(_player.position, _owner.transform.position) <=distanceToAttack && _owner.InSight()) _owner.TransitionSt("Attack");
        Vector3 dir = (new Vector3(_player.position.x,0,_player.position.z) - new Vector3(_owner.transform.position.x,0,_owner.transform.position.z)).normalized;
        _owner.transform.forward = Vector3.Slerp(_owner.transform.forward, dir, 5f * Time.deltaTime);
        _owner.transform.position += _owner.transform.forward * _speed * Time.deltaTime; 
        
    }
    public override void OnFixedUpdate()
    {
       // _rb.velocity = _owner.transform.forward;/*new Vector3(_owner.transform.forward.x * _speed,_rb.velocity.y, _owner.transform.forward.z * _speed);*/
    }
    public override void OnSleep()
    {
        _owner.Notify("Stand");

    }
}
