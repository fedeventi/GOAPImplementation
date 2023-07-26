using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossView : MonoBehaviour , IObserver
{
    Animator _anim;
    public Renderer mat;
    Material _tempMat;
    public Material dieMaterial;
    public Image healthBar;
    Enemy _enemy;
    // Start is called before the first frame update
    void Awake()
    {
        
        _tempMat = mat.material;
    }
    void Start()
    {
        _anim=GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        var observable = GetComponent<Boss>().GetComponent<IObservable>();
        if (observable != null)
            observable.SubEvent(this);
        
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateHealth();
        if (_enemy.GetLife <= 0) mat.material = dieMaterial;
    }
    public void OnNotify(string eventName)
    {
        if (eventName == "Stab")
        {
            OnStabing();
        }
        else if (eventName == "look")
        {
            OnLooking();
        }
        else if (eventName == "moving_On")
        {
            OnMoving(true);
        }
        else if (eventName == "moving_Off")
        {
            OnMoving(false);
        }
        else if (eventName == "Shoot")
        {
            OnShooting();
        }
        else if(eventName== "Reload")
        {
            OnReloading();
        }
        else if (eventName == "Throw")
        {
            OnGrenade();
        }
        else if (eventName == "Damaged")
        {
            OnDamage();
        }
        else if (eventName == "Death")
        {
            OnDeath();
        }
    }
    public void OnStabing()
    {
        _anim.SetTrigger("stab");
        
    }
    public void OnLooking()
    {
        _anim.SetTrigger("look");
    }
    public void OnMoving(bool b)
    {
        
        _anim.SetBool("moving", b);
    }
    public void OnShooting()
    {
        _anim.SetTrigger("Shoot");
        
    }
    public void OnReloading()
    {
        _anim.SetTrigger("Reload");
        
    }
    public void OnGrenade()
    {
        _anim.SetTrigger("Throw");
    }
    public void OnDamage()
    {
        StartCoroutine(DamagedCR());
    }
    IEnumerator DamagedCR()
    {

        mat.material = dieMaterial;
        yield return new WaitForSeconds(1);
        mat.material = _tempMat;
    }
    public void UpdateHealth()
    {
        if (_enemy.GetLife > 0)
        {
            healthBar.gameObject.transform.parent.gameObject.SetActive(true);
            healthBar.fillAmount = _enemy.GetLife / _enemy.lifeMax;
        }
        else
            healthBar.gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void OnDeath()
    {
        _anim.SetBool("Death",true);
    }
    public void DeathEvent()
    {
        GameManager.Instance.Win();
    }
}
