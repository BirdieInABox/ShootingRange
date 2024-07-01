//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingManager : MonoBehaviour, IEventListener
{
    [SerializeField] //list of all weapons in order
    private WeaponList weapons;

    //currently equipped weapon
    private Weapon currentWeapon;

    //Current weapon's index
    private int weaponIndex;

    [SerializeField]
    private ParticleSystem explosionPrefab;

    [SerializeField] //Raycast vertical offset
    private float rayYOffset = -0.05f;

    [SerializeField] //Raycast distance
    private float shootingDistance = 50f;

    [SerializeField]
    private LayerMask rayMask;
    private Ray ray;

    private void Awake()
    {
        //Start with standard weapon
        currentWeapon = weapons.list[0];
    }

    private void Start()
    {
        //Adds this as listener to the event system
        EventManager.MainStatic.AddListener(this);
    }

    /// /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //If event received is of type "GameEnded"
        if (receivedEvent.Type == EventTypes.GameEnded)
        {
            //Interrupts all delays and reloads in progress
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// Called on Fire-Key has been pressed
    /// </summary>
    public void OnFire(InputValue value)
    {
        //If current weapon not empty and not locked
        if (currentWeapon.currentMagazine > 0 && !currentWeapon.onDelay)
        {
            EventManager.MainStatic.FireEvent(new EventData(EventTypes.BulletShot, weaponIndex));

            sendRay();
            FireWeapon();
            //Lock the current weapon
            StartCoroutine(HandleDelay());

            //Reduce current ammo by 1
            currentWeapon.currentMagazine--;
            //if weapon is empty, reload automatically
            if (!currentWeapon.reloading && currentWeapon.currentMagazine <= 0)
            {
                Debug.Log("Reloading");
                StartCoroutine(ReloadWeapon());
            }
        }
    }

    /// <summary>
    /// Handles the firing of the weapon
    /// </summary>
    private void FireWeapon()
    {
        //If the current weapon is a hitscan weapon
        if (currentWeapon.hitScan)
        {
            Debug.Log(currentWeapon.hitScan);
            HitscanShot();
        }
        else //if current weapon uses projectiles
        {
            ProjectileShot();
        }
    }

    private void HitscanShot()
    {
        RaycastHit hitInfo;
        //If raycast hit an object on chosen layer
        if (Physics.SphereCast(ray, 0.05f, out hitInfo, shootingDistance, rayMask))
        {
            //If hit object has component Enemy
            if (hitInfo.collider.GetComponentInParent<Enemy>() != null)
            {
                //Call hit method of Enemy

                hitInfo.collider.GetComponentInParent<Enemy>().Hit(currentWeapon.damage);
            }
            else if (hitInfo.collider.GetComponentInParent<StartButton>() != null)
            {
                hitInfo.collider.GetComponentInParent<StartButton>().Hit();
            }
            SpawnVFX(hitInfo);
        }
    }

    /// <summary>
    /// Spawns the explosion prefab as clone at position of raycast collision with target
    /// </summary>
    /// <param name="hitInfo">the raycast hit</param>
    private void SpawnVFX(RaycastHit hitInfo)
    {
        ParticleSystem explosion = Instantiate(
            explosionPrefab,
            hitInfo.point,
            Quaternion.Euler(Vector3.zero)
        );
    }

    /// <summary>
    /// Shoots projectile
    /// </summary>
    private void ProjectileShot()
    {
        //Angle is the point the camera is facing
        Quaternion projectileAngle = Quaternion.Euler(Camera.main.transform.forward);
        //origin is at the camera, modified by vertical offset also used for raycast
        Vector3 projectileOrigin = Camera.main.transform.position;
        projectileOrigin.y += rayYOffset;
        //Spawns projectile with given parameters
        Projectile projectile = Instantiate(
            currentWeapon.projectilePrefab,
            projectileOrigin,
            projectileAngle
        );
        //Sets the projectile's speed, damage and lifetime
        projectile.SetParams(
            currentWeapon.projectileSpeed,
            currentWeapon.damage,
            currentWeapon.projectileLifetime
        );
        //Tells the projectile to start flying
        projectile.Shoot();
    }

    /// <summary>
    ///  puts shots at a delay for the current weapon only
    /// </summary>
    /// <returns> Wait for the weapon's shots' delay </returns>
    IEnumerator HandleDelay()
    {
        Weapon _weapon = currentWeapon;
        //locks weapon
        _weapon.onDelay = true;
        //Wait for a number of seconds equal to 10/weaponSpeed
        //For example a weapon with a speed of 20 has a delay of 10/20 seconds, or 0.5 seconds.
        yield return new WaitForSeconds(10f / _weapon.weaponSpeed);
        //unlocks weapon
        _weapon.onDelay = false;
    }

    /// <summary>
    /// recharges gun to full ammo after delay
    /// </summary>
    /// <returns> wait for reload duration </returns>
    IEnumerator ReloadWeapon()
    {
        Weapon _weapon = currentWeapon;
        int _index = weaponIndex;
        _weapon.reloading = true;
        yield return new WaitForSeconds(_weapon.reloadTime);
        _weapon.reloading = false;
        EventManager.MainStatic.FireEvent(new EventData(EventTypes.Reloaded, _index));
        _weapon.currentMagazine = _weapon.magazineSize;
    }

    /// <summary>
    /// Send raycast
    /// </summary>
    private void sendRay()
    {
        //towards looked-at angle
        Vector3 rayAngle = Camera.main.transform.forward;
        //from camera-center
        Vector3 rayOrigin = Camera.main.transform.position;
        //if gun not on camera center, change origin of ray
        rayOrigin.y += rayYOffset;
        ray = new Ray(rayOrigin, rayAngle);
    }

    /// <summary>
    /// Called on buttons pressed
    /// </summary>
    public void OnGUI()
    {
        //if game isn't paused & a button has been pressed down
        if (
            (Time.timeScale != 0)
            && Event.current.isKey
            && (Event.current.type == EventType.KeyDown)
        )
        {
            //Get the input as string
            string keyCode = Event.current.keyCode.ToString();

            //Check for numbers 1 to x and switch weapon to weapon at the index = number pressed
            switch (keyCode)
            {
                case "Alpha1":
                    SwitchWeapon(0);
                    break;
                case "Alpha2":
                    SwitchWeapon(1);
                    break;
                case "Alpha3":
                    SwitchWeapon(2);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// equips the weapon at position index in the weaponList's array
    /// </summary>
    /// <param name="index"> the chosen index of the weapon to be equipped </param>
    private void SwitchWeapon(int index)
    {
        weaponIndex = index;
        currentWeapon = weapons.list[index];
        Debug.Log("Weapon equipped: " + currentWeapon.name);
        Debug.Log("Magazine: " + currentWeapon.currentMagazine);
        //if weapon is empty on equip, reload
        if (!currentWeapon.reloading && currentWeapon.currentMagazine <= 0)
        {
            Debug.Log("Reloading");
            StartCoroutine(ReloadWeapon());
        }
    }
}
