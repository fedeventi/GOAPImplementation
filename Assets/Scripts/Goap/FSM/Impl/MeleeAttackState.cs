using System;
using System.Linq;
using FSM;
using UnityEngine;
using System.Collections.Generic;

public class MeleeAttackState : MonoBaseState {
    
    public override event Action OnNeedsReplan;
    Enemy _enemy;
    public float attackRate;
    public float _damage;
    public GameObject knife;
    public float maxDistance = 2f;
    BoxQuery boxQuery;

    private PlayerModel _player;

    private float _lastAttackTime;


    private void Awake() {
        _enemy = GetComponent<Enemy>();
        boxQuery = _enemy.boxQuery;
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _lastAttackTime = attackRate;
        knife.SetActive(true);
        _player = _enemy.player.GetComponent<PlayerModel>();
        GetComponent<Boss>().Notify("moving_Off");
    }
    
    public override void UpdateLoop()
    {
        Debug.Log("ESTOY EN MELEE");

        _lastAttackTime -= Time.deltaTime;
        //Debug.Log(_lastAttackTime);
        if (_lastAttackTime<= 0) 
        {
            GetComponent<Boss>().Notify("Stab");
            _lastAttackTime = attackRate;
        }


        
        
        float Distance = (_player.transform.position - transform.position).magnitude;

        if (Distance > maxDistance)
        {
            //knife.SetActive(false);
            OnNeedsReplan?.Invoke();
        }
    }
    public PlayerModel Attack()
    {
        return boxQuery.Query().OfType<PlayerModel>().Where(player => player.Life > 0).FirstOrDefault(); //IA2-P3
    }

    public override IState ProcessInput() {
        return this;
    }
    public void AttackAnim()
    {
        var player = Attack();
        if (player != null)
        {
            player.Damaged(_damage);
            OnNeedsReplan?.Invoke();
        }
    }
    
}