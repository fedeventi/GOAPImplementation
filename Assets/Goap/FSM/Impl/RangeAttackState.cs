using System;
using UnityEngine;
using FSM;
using System.Collections.Generic;
using System.Collections;

public class RangeAttackState : MonoBaseState {
    public override event Action OnNeedsReplan;

    Enemy _owner;
    PlayerModel player;
    public GameObject bullet;
    public float attackRate;
    bool _hasShoot;
    float currentTime;
    public GameObject knife;
    public GameObject pistol;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {

        base.Enter(from, transitionParameters);
        player = FindObjectOfType<PlayerModel>();
        currentTime = 0;
        GetComponent<Boss>().Notify("moving_Off");
        GetComponent<Boss>().hasBullet = false;
        GetComponent<Boss>().Notify("Reload"); 
        _hasShoot = false;
        pistol.SetActive(true);
        knife.SetActive(false);
    }
    public override void UpdateLoop() {

        Debug.Log("ESTOY EN SHOOT");

        currentTime += Time.deltaTime;
        
        if (currentTime > attackRate)
        {
            if(!_hasShoot)
            {
                transform.LookAt(player.transform.position); 
                 GetComponent<Boss>().Notify("Shoot");
                _hasShoot = true;           
            }
            if (currentTime > attackRate + 1)
            {
                
                pistol.SetActive(false);
                currentTime = 0;
                OnNeedsReplan?.Invoke();
            }
            GetComponent<Boss>().hasBullet = false;
        }
            
    }
    
    public void Shoot()
    {
        Instantiate(bullet, transform.position+Vector3.up*2,transform.rotation);
    }
    public override IState ProcessInput() {
            return this;
    }
}