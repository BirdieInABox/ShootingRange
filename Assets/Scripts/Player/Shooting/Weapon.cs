//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public bool hitScan; //Is this weapon a hitscan weapon?
    public Projectile projectilePrefab; //the projectile, only needed when hitscan = false
    public int magazineSize; //max ammo

    [HideInInspector] //current ammo
    public int currentMagazine;
    public float reloadTime; //time needed to reload in seconds

    [Range(1f, 100f)] //clamp weaponSpeed value between 1 and 100
    //This is later calculated via: 10/weaponSpeed
    public float weaponSpeed = 10;

    //The weapon damage
    public float damage;

    //[HideInInspector] //Show in inspector for play in editor, as onDelay does not reset in editor
    public bool onDelay = false;

    //[HideInInspector] //Show in inspector for play in editor, as onDelay does not reset in editor
    public bool reloading = false;

    //projectile parameters, only needed when hitscan = false
    public float projectileSpeed;
    public float projectileLifetime;
}
