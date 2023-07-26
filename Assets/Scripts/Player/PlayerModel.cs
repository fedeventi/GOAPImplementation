using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.XR.WSA.Input;

public class PlayerModel : MonoBehaviour, IObservable, IGridEntity
{
    private float speed = 10f;
    private float life = 100;

    private Camera mainCamera;
    private Rigidbody _rb;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    public float damage;
    public float decreaseDamge;
    public float minDamage;
    private bool isShooting;
    private bool canShot;
    public bool isDeadth=false;
    public float timeBetweenShots;
    private float shotCounter;
    public GameObject flameThrower;
    List<IObserver> _Observers = new List<IObserver>();
    public ConeQuery coneQuery;
    int acummulatedPoints;
    public int currentLife;
    public Acid acid;
    public event Action<IGridEntity> OnMove;

    private void Awake()
    {
        currentLife = (int)life;
        _rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();        
    }

    private void Update()
    {
        
        
        
    }
    private void LateUpdate()
    {
       
    }


    public Rigidbody RigidBody
    {
        get
        {
            return _rb;
        }
    }
    public Camera Camera
    {
        get
        {
            return mainCamera;
        }
    }
    public Vector3 MoveInput
    {
        get
        {
            return moveInput;
        }
        set
        {
            moveInput = value;
        }
    }
    public Vector3 MoveVelocity
    {
        get
        {
            return moveVelocity;
        }
        set
        {
            moveVelocity = value;
        }
    }   
    public float Life
    {
        get { return life; }
        set { life = value; }
    }
    public float Speed
    {
        get
        {
            return speed;
        }
    }
    public bool IsShooting
    {
        get
        {
            return isShooting;
        }
        set
        {
            isShooting=value;
        }
    }
    public bool CanShot
    {
        get
        {
            return canShot;
        }
        set
        {
            canShot = value;
        }
    }
    public float TimeBetweenShots
    {
        get
        {
            return timeBetweenShots;
        }
        set
        {
            timeBetweenShots = value;
        }
    }
    public float ShotCounter
    {
        get
        {
            return shotCounter;
        }
        set
        {
            shotCounter = value;
        }
    }

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public void Move(float ver,float hor)
    {
        if (ver != 0 || hor != 0)
        {
            Notify("Run");
        }
        else
            Notify("Idle");
        _rb.velocity = new Vector3(-ver * speed, _rb.velocity.y , hor * speed);
    }

    public void Shoot()
    {
        var _enemies = Attack();
        
        if (isDeadth == false && Time.timeScale > 0)
        {
            if (IsShooting)
            {
                
                flameThrower.SetActive(true);
                Notify("Shoot");
                ShotCounter -= Time.deltaTime;
                if (ShotCounter <= 0)
                {
                    acummulatedPoints = Points();
                    Notify("AddPoints");
                    CanShot = true;
                    float damageAccum=damage;
                    foreach (var enemie in _enemies)
                    {
                        
                        enemie.Damaged(damageAccum);
                        damageAccum = damageAccum > minDamage ? damageAccum -= decreaseDamge : minDamage;
                    }
                    ShotCounter = TimeBetweenShots;
                }
            }
            else
            {
                ShotCounter = TimeBetweenShots;
                Notify("StopShooting");
                flameThrower.SetActive(false);
            }
        }
    }
    public void Deadth()
    {
        if (currentLife <= 0)
        {
            isDeadth = true;
            Notify("Deadth");
        }
    }
    public void Damaged(float damage)
    {  
        currentLife -= (int)damage;
        Notify("Damaged");
    }


    //IA2-P2 //Hasta la linea 227
    private IEnumerable<Enemy> Attack() 
    {
        return coneQuery.Query().OfType<Enemy>() 
                                .Where(x => x.GetLife > 0)
                                .OrderBy(x => x.DistanceFromPlayer).Take(5); //IA2-P3 
    }

    int Points()
    {
        int points = coneQuery.Query().OfType<Enemy>() 
                              .Where(x => x.GetLife > 0)
                              .Select(x => (int)(x.GetLife))
                              .Aggregate(0, (accum, current) => accum + (current+(int)(current*(currentLife/Life)))); //IA2-P3
        return points;
    }
    public int GetPoints
    {
        get { return acummulatedPoints; }
    }
    public void Notify(string eventName)
    {
        foreach (var item in _Observers)
        {
            item.OnNotify(eventName);
        }
    }
    public void LookAt(Vector3 position)
    {
        if(!isDeadth)
            transform.LookAt(position);
    }
    public void SubEvent(IObserver obs)
    {
        _Observers.Add(obs);
    }

    public void UnSubEvent(IObserver obs)
    {
        _Observers.Remove(obs);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>())
        {
            var projectile = other.GetComponent<Projectile>();
            Damaged(projectile.GetDamage);
        }
    }
}
