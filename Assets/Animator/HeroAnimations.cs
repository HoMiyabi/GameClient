using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HeroAnimations : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void SetAllFalse()
    {
        animator.SetBool("idle", false);
        animator.SetBool("run", false);
        animator.SetBool("attack1", false);
        animator.SetBool("die", false);
        animator.SetBool("gethit", false);
    }

    public void PlayIdle()
    {
        SetAllFalse();
        animator.SetBool("idle", true);
    }

    public void PlayRun()
    {
        SetAllFalse();
        animator.SetBool("run", true);
    }

    public void PlayAttack1()
    {
        SetAllFalse();
        animator.SetBool("attack1", true);
    }

    public void PlayDie()
    {
        SetAllFalse();
        animator.SetBool("die", true);
    }

    public void PlayGethit()
    {
        SetAllFalse();
        animator.SetBool("gethit", true);
    }

    public void Attack01End()
    {
        PlayIdle();
    }

    public void GethitEnd()
    {
        PlayIdle();
    }
}
