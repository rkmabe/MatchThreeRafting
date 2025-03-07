using MatchThreePrototype;
using System;
using UnityEngine;


public class Cannonball : Projectile
{

    public static event Action<Projectile> OnProjectileReturn;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IWeaponTarget target = collision.gameObject.GetComponent<IWeaponTarget>();

        target.TakeDamage(1, collision.contacts[0]);

        OnProjectileReturn(this);
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
