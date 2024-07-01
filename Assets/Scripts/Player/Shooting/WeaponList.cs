//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponList", menuName = "WeaponList", order = 0)]
public class WeaponList : ScriptableObject
{
    //List of all equippable weapons
    public Weapon[] list;
}
