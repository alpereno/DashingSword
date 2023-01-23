using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private Sword startingSword;
    [SerializeField] private Transform swordHold;
    Sword equippedSword;
    float nextAttackTime;

    void Start()
    {
        if (startingSword != null)
        {
            EquipSword(startingSword);
        }
    }

    public void EquipSword(Sword swordToEquip)
    {
        if (equippedSword != null)
        {
            Destroy(equippedSword.gameObject);
        }

        equippedSword = Instantiate(swordToEquip, swordHold.position, swordHold.rotation);
        equippedSword.transform.parent = swordHold;
    }

    public bool Attack(float msBetweenAttacks)
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + msBetweenAttacks / 1000;
            print("Damage applied...");
            return true;
        }
        return false;
    }
}
