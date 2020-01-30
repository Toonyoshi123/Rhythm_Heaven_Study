using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchedAnimation : MonoBehaviour
{
    public Animator animator;
    public AnimatorStateInfo animatorStateInfo;
    public int currentState;
    public float beatAmplifier;

    // This should be your idle bopping animation.
    public string mainAnimation = "";

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // This loop will make sure the idle bop is always in sync with the music. You can turn it off if wanted.
    // The loop also makes sure the speed of the animator stays the same in any animation.
    void Update()
    {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        currentState = animatorStateInfo.fullPathHash;
        if (animatorStateInfo.IsName(mainAnimation))
        {
            animator.Play(currentState, -1, (Conductor.instance.loopPositionInAnalog));
            animator.speed = 0;
        }
        else
        {
            animator.speed = (1f / Conductor.instance.secPerBeat) / beatAmplifier;
        }
    }
}
