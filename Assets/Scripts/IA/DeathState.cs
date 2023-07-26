using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState<T> : State<T>
{
    Enemy _owner;
    Rigidbody _rb;
    Collider _collider;
    EnemyView view;

    float currentTime;
    float timeToOff = 5; 

    public DeathState(Enemy enemy)
    {
        _owner = enemy;
        _rb = enemy.GetComponent<Rigidbody>();
        _collider= enemy.GetComponent<Collider>();
        view = enemy.GetComponent<EnemyView>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _owner.GetComponentInChildren<Renderer>().material = view.dieMaterial;
        _owner.Notify("Death");
        _rb.isKinematic = true;
        _collider.isTrigger = true;      
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currentTime += Time.deltaTime; 
    }

    public override void OnSleep()
    {
        base.OnSleep();
        _owner.GetComponentInChildren<Renderer>().material = view.GetMaterial;
        _rb.isKinematic = false;
        _collider.isTrigger = false;
    }
}
