using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IPooleable<Grenade>
{

    Vector3 _dir;
    float currentLife = 0;
    float lifeDistance = 10;
    LaunchGrenade _owner;
    public GameObject particleSystem;
    public Rigidbody _rb;
    
    Transform _player;
    public float force;
    public Grenade SetPlayer(Transform player)
    {
        _player = player;
        return this;
    }
    public Grenade SetOwner(LaunchGrenade enemy)
    {
        _owner = enemy;
        return this;
    }
    private void Start()
    {
        
       
        _dir = _player.transform.position - transform.position;
        GetComponent<Rigidbody>().AddForce(new Vector3(_dir.x, 0, _dir.z) + Vector3.up * _dir.magnitude, ForceMode.Impulse); 
    }
    private void Update()
    {
        currentLife += Time.deltaTime;
        if (currentLife > lifeDistance)
        {
            //_owner.Recycle(this);
            Destroy(gameObject);
        }
       // _rb.velocity = new Vector3(_dir.x, _rb.velocity.y / 1.6f, _dir.z);
        
    }
    public void instantiateParticle()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);
    }
   

    public static void TurnOnStatic(Grenade obj)
    {
        obj.TurnOn(obj);
    }

    public static void TurnOffStatic(Grenade obj)
    {
        obj.TurnOff(obj);
    }

    public void TurnOn(Grenade obj)
    {
        obj.currentLife = 0;
        obj.transform.position = obj._owner.transform.position + new Vector3(0, 2, 0);
        _dir = _player.transform.position - transform.position;
        obj.GetComponent<Rigidbody>().AddForce(new Vector3(_dir.x, 0, _dir.z)+Vector3.up*3*_dir.magnitude, ForceMode.Impulse);
        obj.gameObject.SetActive(true);

    }

    public void TurnOff(Grenade obj)
    {
        obj.gameObject.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            instantiateParticle();
            //_owner.Recycle(this);
            Destroy(gameObject);
        }
    }

    
}
