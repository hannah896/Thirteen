using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RunAnimation(bool isRun)
    {
        animator.SetBool("IsRun", isRun);
    }

    public void JumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    public void WalkAnimation(float dirMagnitude)
    {
        animator.SetFloat("InputDir", dirMagnitude);
    }

    // 무기를 들고 있지 않을 때 공격
    public void BasicAttack()
    {
        animator.SetTrigger("Attack");
    }

    // 무기를 들고 있을 때 공격
    public void WeaponAttack()
    {
        animator.SetTrigger("WeaponAttack");
    }

    // 곡괭이를 들고 있을 때 캐는 모션
    public void RockAttack()
    {
        animator.SetTrigger("RockAttack");
    }

    // 무기를 들고 있을 때 공격
    public void TreeAttack()
    {
        animator.SetTrigger("TreeAttack");
    }

    // 무기를 들고 있을 때 공격
    public void PlantAttack()
    {
        animator.SetTrigger("PlantAttack");
    }

    public void DieAnimation()
    {
        animator.SetTrigger("Die");
    }
}
