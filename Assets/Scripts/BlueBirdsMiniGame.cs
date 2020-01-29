using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBirdsMiniGame : MiniGame
{
    // This minigame is broken and does not work properly yet.

    public GameObject captain;
    public GameObject bird;
    private Animator captainAnimator;
    private Animator birdAnimator;

    private void Start()
    {
        captainAnimator = captain.GetComponent<Animator>();
        birdAnimator = bird.GetComponent<Animator>();
        bird.GetComponent<SynchedAnimation>().mainAnimation = "Idle";
        captain.GetComponent<SynchedAnimation>().mainAnimation = "Captain";
    }

    public override void EarlyHit(bool isLetGo)
    {
        if (!isLetGo)
        {
            base.EarlyHit(isLetGo);
            BadHit(true, isLetGo);
        }
    }

    public override void GoodHit(bool isLetGo)
    {
        base.GoodHit(isLetGo);
        captainAnimator.Play("Captain Praise");
        switch (InputDetection.instance.currentInputState)
        {
            case InputState.HOLD:
                // Squat.
                birdAnimator.Play("Squat");
                break;
            case InputState.FLICK:
                // Stretch.
                birdAnimator.Play("Stretch");
                break;
            case InputState.TAP:
                // Peck.
                birdAnimator.Play("Peck");
                break;
        }
    }

    public override void LateHit(bool isLetGo)
    {
        if (!isLetGo)
        {
            base.LateHit(isLetGo);
            BadHit(true, isLetGo);
        }
    }

    public override void BadHit(bool isWrong, bool isLetGo)
    {
        if (!isLetGo)
        {
            base.BadHit(isWrong, isLetGo);
            captainAnimator.Play("Captain Anger");
            switch (InputDetection.instance.currentInputState)
            {
                case InputState.HOLD:
                    // Squat.
                    birdAnimator.Play("Squat Miss");
                    break;
                case InputState.FLICK:
                    // Stretch.
                    birdAnimator.Play("Stretch Miss");
                    break;
                case InputState.TAP:
                    // Peck.
                    birdAnimator.Play("Peck Miss");
                    break;
            }
        }
    }
}
