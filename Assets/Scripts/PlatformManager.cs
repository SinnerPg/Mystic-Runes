using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private Animator animator;
    public BoxCollider Respawn;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void checkAnimator()
    {
        if(!animator.GetBool("running"))
        {
            animator.enabled = false;
        }
    }

    public void enableRespawn()
    {
        Respawn.enabled = true;
    }
}
