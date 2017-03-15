using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catmageddon/Tank")]
public class TankObject : Item
{
    [Tooltip("Tank Model to be used. Must have a 'SpawnPoint' and 'CatSpawnPoint' already")]
    public GameObject TankPrefab;
    public GameObject CatPrefab;
    [Tooltip("Side weapons are the SMG's while the Main weapons are special weapons that can be varied")]
    public Weapon SideWeapon, MainWeapon;
    public float Speed;
    public float Health;
    [Tooltip("Base is 0.5f to be used for the attack speed of the main weapon")]
    public float MainCoolDown = 0.5f;
    [Tooltip("FireMode for the different types of firetypes like Beam, Single Fire or Automatic")]
    public FireMode MainWeaponFireMode;
}
