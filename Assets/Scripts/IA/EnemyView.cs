using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour , IObserver
{

    Animator _anim;
    Enemy _enemy;
    public Material dieMaterial;
    Renderer _mat;
    Material _tempMat;
    public Image healthBar;
    
    private void Awake()
    {
        _mat = GetComponentInChildren<Renderer>();
        _tempMat = _mat.material;
            
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        var observable = GetComponent<IObservable>();
        if(observable!=null)
            observable.SubEvent(this);

        
    }

    public Material GetMaterial
    {
        get
        {
            return _tempMat;
        }
    }


    public void Update()
    {
        UpdateHealth();
        if(_enemy.GetLife <= 0) _mat.material = dieMaterial;
    }

    public void OnNotify(string eventName)
    {
        if (eventName=="Punch")
        {
            OnAttack();
        }
        if (eventName == "Move")
        {
            OnMove(true);
        }
        if (eventName == "Stand")
        {
            OnMove(false);
        }
        if (eventName == "Damaged")
        {
            OnDamage();
        }
        if (eventName == "Death")
        {
            OnDeath(true);
        }
        if (eventName == "Spit")
        {
            OnSpit();
        }
    }
    public void OnDamage()
    {
        StartCoroutine(DamagedCR());
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
    IEnumerator DamagedCR()
    {
       
        _mat.material = dieMaterial;
        yield return new WaitForSeconds(1);
        _mat.material = _tempMat;
    }
    public void OnAttack()
    {
        _anim.SetTrigger("Punch");
    }
    public void OnMove(bool isMoving)
    {
        _anim.SetBool("Move", isMoving);
    }
    public void OnDeath(bool isDeath)
    {
        _anim.SetBool("Death", isDeath);
    }
    public void OnSpit()
    {
        _anim.SetTrigger("Spit");
    }
}
