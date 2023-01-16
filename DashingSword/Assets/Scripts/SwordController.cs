using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private Sword startingSword;
    [SerializeField] private Transform swordHold;
    Sword equippedSword;

    void Start()
    {
        if (startingSword != null)
        {
            equipSword(startingSword);
        }
    }

    public void equipSword(Sword swordToEquip)
    {
        if (equippedSword != null)
        {
            Destroy(equippedSword.gameObject);
        }

        equippedSword = Instantiate(swordToEquip, swordHold.position, swordHold.rotation);
        equippedSword.transform.parent = swordHold;
    }
}
