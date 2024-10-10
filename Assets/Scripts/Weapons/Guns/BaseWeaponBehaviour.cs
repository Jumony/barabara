using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class that all Weapons derive from. Contains logic for
 * shooting, controls, reload, etc.
 * It also handles initializing values according to the correct PlayerWeaponType
 * 
 * "Abstract" means that other classes can derive from this. Its essentially
 * a template
 */
public abstract class BaseWeaponBehaviour : MonoBehaviour
{
    public Transform shootOrigin;
    protected ObjectPooler objectPooler; // Protected means that only subclasses can use this variable
    public PlayerWeaponType weaponType;

    protected float fireRate;
    protected float nextFireTime = 0f;

    /*
     *  This function will be used as long as the subclass does not override their own function
     *  In other words, this is the default function behaviour unless otherwise defined by the subclass
     */
    protected virtual void Start()
    {
        fireRate = weaponType.fireRate;
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }

    protected virtual void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !GameObject.Find("InventoryManager").GetComponent<RadialMenu>().menuEnabled)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    /*
     * An abstract function means that the subclass MUST define their own
     * function behaviour for this function
     */
    public abstract void Shoot();

    /*
     * This has no special access modifier because we want reload to be the same across ALL weapons
     * 
     * Unless otherwise is desired, this should remain without an access modifier
     */
    public void Reload()
    {
        return;
    }

    public virtual PlayerWeaponType GetWeaponType()
    {
        return weaponType;
    }
}
