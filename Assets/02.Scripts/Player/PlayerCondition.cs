using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition hp { get { return uiCondition.hp; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition thirst { get { return uiCondition.thirst; } }

    public float noHungerHPDecay;
    public float noThirstHPDecay;

    public event Action onTakeDamage;

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

    public void Eat(float amount)
    {
        //if() 음식이라면
        hunger.Add(amount);

        //if() 물이라면
        thirst.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Player Die");
    }

    public void TakeDamage(int damage)
    {
        hp.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
            return false;

        stamina.Subtract(amount);
        return true;
    }
}
