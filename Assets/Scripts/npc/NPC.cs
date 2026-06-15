using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public Animator parentAnimator;

   
    //separazione animazione del NPC e UI WorldSpace
    public void ResetAllIdleBools()
    {
        animator.SetBool("Idle_Front", false);
        animator.SetBool("Idle_Right", false);
        animator.SetBool("Idle_Back", false);
    }

    public void ResetParentAnim()
    {
        parentAnimator.SetBool("IsTalk", false);
    }
}