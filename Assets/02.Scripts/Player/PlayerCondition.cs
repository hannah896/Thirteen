using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    [SerializeField] private int attackDamage = 5;
    public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }       // 공격력
    [SerializeField] private int defense = 5;
    public int Defense { get { return defense; } set { defense = value; } }            // 방어력

    Condition hp { get { return uiCondition.hp; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition thirst { get { return uiCondition.thirst; } }

    public float noHungerHPDecay;
    public float noThirstHPDecay;

    public event Action onTakeDamage;

    public bool isDie;

    public bool godMode;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        thirst.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
            hp.Subtract(noHungerHPDecay * Time.deltaTime);

        if (thirst.curValue == 0f)
            hp.Subtract(noThirstHPDecay * Time.deltaTime);

        if (hp.curValue == 0f)
            Die();
    }

    public void Heal(float amount)
    {
        hp.Add(amount);
    }

    public void Eat(ItemData item)
    {
        if (item.itemType != ItemType.Consumable) return;

        foreach (Effect effect in item.effect)
        {
            switch (effect.consumableType)
            {
                case ConsumableType.Health:
                    hp.Add(effect.value);
                    break;
                case ConsumableType.Hunger:
                    hunger.Add(effect.value);
                    break;
                case ConsumableType.Thirsty:
                    thirst.Add(effect.value);
                    break;
                case ConsumableType.Stemina:
                    stamina.Add(effect.value);
                    break;
            }
        }
    }

    public void Die()
    {
        if (godMode) return;
        if (!isDie)
        {
            Debug.Log("Player Die");
            isDie = true;

            CharacterManager.Instance.Player.animController.DieAnimation();
        }
    }

    public void TakeDamage(int damage)
    {
        if(!isDie)
        {
            hp.Subtract(damage);
            onTakeDamage?.Invoke();
        }

    }

    public void SetAttackDamage(int value)
    {
        attackDamage += value;
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
            return false;

        stamina.Subtract(amount);
        return true;
    }
}
