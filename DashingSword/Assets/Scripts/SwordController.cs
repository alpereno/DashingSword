using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public event System.Action OnAttackEnded;
    [SerializeField] private Sword startingSword;
    [SerializeField] private Transform swordHold;
    Sword equippedSword;
    float nextAttackTime;

    AnimatorManager animatorManager;

    void Start()
    {
        animatorManager = GetComponent<AnimatorManager>();

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

    public bool AttackWithAnimation(float msBetweenAttacks)
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + msBetweenAttacks / 1000;
            int randomAttackIndex = Random.Range(1, 4);
            float attackTime;

            // good for 1500 msBetweenAttack variable value
            // and this method should be return max 1.5 sec
            // attack animations are should be short cause each time 1 enemy will die (max 1.5sec - 1.2sec fine (2 variables are should be linked))

            if (randomAttackIndex == 1)
            {
                attackTime = animatorManager.PlayAttackAnimation("AttackOne");
                Invoke("DisableAttacking", attackTime);
                return true;
            }
            else if (randomAttackIndex == 2)
            {
                attackTime = animatorManager.PlayAttackAnimation("AttackTwo");
                Invoke("DisableAttacking", attackTime);
                return true;
            }
            else if (randomAttackIndex == 3)
            {
                attackTime = animatorManager.PlayAttackAnimation("AttackThree");
                Invoke("DisableAttacking", attackTime);
                return true;
            }
        }
        return false;
    }

    void ApplyDamage()
    {
        equippedSword.ApplyDamage();
    }

    private void DisableAttacking()
    {
        if (OnAttackEnded != null)
        {
            OnAttackEnded();
        }
    }
}
