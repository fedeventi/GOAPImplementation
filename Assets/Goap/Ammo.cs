using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour , IPooleable<Ammo>
{
    
    public Ammo SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }
    public static void TurnOnStatic(Ammo obj)
    {
        obj.TurnOn(obj);
    }
    public static void TurnOffStatic(Ammo obj)
    {
        obj.TurnOff(obj);
    }
    public void TurnOff(Ammo obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void TurnOn(Ammo obj)
    {
        obj.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
