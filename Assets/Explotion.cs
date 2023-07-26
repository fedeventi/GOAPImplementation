using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explotion : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        _damage = 45;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetDamage
    {
        get { return _damage; }
        set { value = _damage; }
    }
    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
