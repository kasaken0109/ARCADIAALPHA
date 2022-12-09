using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveActionCaller : MonoBehaviour
{
    public static PassiveActionCaller Instance
    {
        get
        {
            if (_instance) return _instance;
            var instance = FindObjectOfType<PassiveActionCaller>();
            if (instance)
            {
                _instance = instance;
            }
            else
            {
                var caller = new GameObject("Caller");
                _instance = caller.AddComponent<PassiveActionCaller>();
                _instance.OnAwake();
            }
            return _instance;
        }
    }

    static PassiveActionCaller _instance;

    PlayerMoveController playerControll;
    PlayerAttackController playerAttack;
    PlayerManager playerManager;
    BulletFire bulletFire;
    void OnAwake()
    {
        playerControll = FindObjectOfType<PlayerMoveController>();
        playerAttack = FindObjectOfType<PlayerAttackController>();
        playerManager = FindObjectOfType<PlayerManager>();
        bulletFire = FindObjectOfType<BulletFire>();
    }


    public void PlayerHeal(float healAmount)
    {
        playerManager.Heal(Mathf.CeilToInt(healAmount) * -1);
    }

    public void PlayerAttackUp(float attackupRate, float time)
    {
        playerAttack.ChangeAttackPower(attackupRate, time);
    }

    public void PlayerDefenceUp(float defenceupRate, float time)
    {
        playerManager.ChangeDefenceValue(defenceupRate, time);
    }
    public void InvisibleTimeUp(float extentionRate, float time)
    {
        playerManager.ChangeInvisibleTime(extentionRate, time);
    }

    public void ReduceEnergy(float reduceRate)
    {
        bulletFire.ChangeBulletEnergy(reduceRate);
    }

    public void ReduceCoolDown(float reduceRate)
    {
        bulletFire.ChangeCoolDown(reduceRate);
    }

    public void AddBulletDamage(float addRate)
    {
        bulletFire.ChangeBulletDamage(addRate);
    }

    public void AddStun(GameObject target)
    {
        target.GetComponentInParent<IStun>().StunChecker(1);
    }
}
