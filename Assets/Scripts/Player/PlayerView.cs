using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerView : MonoBehaviour, IObserver
{
    Animator _anim;
    PlayerModel _playerModel;
    public Image healthBar;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerModel= GetComponent<PlayerModel>();
        var observer = GetComponent<IObservable>();
        if (observer!=null)
        {
            observer.SubEvent(this);
        }
    }
    public void OnNotify(string eventName)
    {
        if (eventName == "Shoot")        
            Shoot();        
        if (eventName == "Idle")        
            Idle();
        if (eventName == "Run")
            Run();
        if (eventName == "Deadth")
            Deadth();
        if (eventName == "Damaged")
            Damaged();
        if (eventName == "StopShooting")
            StopShooting();
    }
    private void Deadth()
    {
        _anim.SetBool("IsDeadth", true);
    }
    public void Run()
    {
        _anim.SetBool("Running", true);
    }
    public void Idle()
    {
        _anim.SetBool("Running", false);
    }
    public void Shoot()
    {
        _anim.SetBool("ShotFlame", true);       
    }
    public void Damaged()
    {
        healthBar.fillAmount = _playerModel.currentLife / _playerModel.Life;
    }
    public void LoseScene()
    {
        GameManager.Instance.Lose();
    }
    public void StopShooting()
    {
        _anim.SetBool("ShotFlame", false);
    }
}
