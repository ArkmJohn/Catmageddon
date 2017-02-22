using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catma/Tank")]
public class Tank : ScriptableObject
{
    public GameObject TankPrefab;
    public GameObject CatPrefab;
    public Projectile Weapon1, Weapon2;
    public float speed;
    public float health;
    public float SecondaryCoolDown = 0.5f;
    public FireMode myFireMode;
}
