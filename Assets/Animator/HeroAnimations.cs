using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HeroAnimations : MonoBehaviour
{
    private Animator animator;

    public enum HState
    {
        None = 0,
        Idle = 1,
        Run = 2,
        Attack = 3,
        Die = 4,
        Gethit = 5,
    }

    public HState state = HState.Idle;

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
        if (state == HState.Attack)
        {
            return;
        }
        SetAllFalse();
        animator.SetBool("idle", true);
        state = HState.Idle;
    }

    public void PlayRun()
    {
        if (state == HState.Attack)
        {
            return;
        }
        SetAllFalse();
        animator.SetBool("run", true);
        state = HState.Run;
    }

    public void PlayAttack1()
    {
        SetAllFalse();
        animator.SetBool("attack1", true);
        state = HState.Attack;
    }

    public void PlayDie()
    {
        SetAllFalse();
        animator.SetBool("die", true);
        state = HState.Die;
    }

    public void PlayGethit()
    {
        SetAllFalse();
        animator.SetBool("gethit", true);
        state = HState.Gethit;
    }

    public void Attack01End()
    {
        state = HState.None;
        PlayIdle();
    }

    public void GethitEnd()
    {
        state = HState.None;
        PlayIdle();
    }
}
