using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Acid : Projectile,IPooleable<Acid>
{
   
    Vector3 _dir;
    float currentLife = 0;
    float lifeDistance = 10;
    HeavyEnemy _owner;
    public ParticleSystem particleSystem;
    public Rigidbody _rb;
    

   public Acid SetDirection(Vector3 dir)
    {
        _dir = dir;
        return this;
    }
    public Acid SetOwner(HeavyEnemy enemy) 
    {
        _owner = enemy;
        return this;
    } 
    void Start()
    {
        _damage = 12;

    }
    private void Update()
    {
        currentLife += Time.deltaTime;
        if (currentLife > lifeDistance)
        {
            _owner.Recycle(this);
        }
        _rb.velocity = new Vector3(_dir.x, _rb.velocity.y/1.6f, _dir.z);


    }
    public void instantiateParticle()
    {
        Instantiate(particleSystem,transform.position,Quaternion.identity);
    }
    public void BeShoot()
    {
        
        
        currentLife = 0;
        
    }

    public static  void TurnOnStatic(Acid obj)
    {
        //obj.transform.position = obj._owner.transform.position + new Vector3(0, 2, 0);
        //obj.BeShoot();
        //obj.gameObject.SetActive(true);
        obj.TurnOn(obj);
    }

    public static void TurnOffStatic(Acid obj)
    {
        //obj.gameObject.SetActive(false);
        obj.TurnOff(obj);
    }

    public  void TurnOn(Acid obj)
    {
        obj.transform.position = obj._owner.transform.position + new Vector3(0, 2, 0);
        obj.BeShoot();
        obj.gameObject.SetActive(true);
    }

    public  void TurnOff(Acid obj)
    {
        obj.gameObject.SetActive(false);
    }



    private  void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==8 || other.gameObject.layer == 9|| other.gameObject.layer ==10 )
        {
            instantiateParticle();
            _owner.Recycle(this);
        }
    }

   
}
