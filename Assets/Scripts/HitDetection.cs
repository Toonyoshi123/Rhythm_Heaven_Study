using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public static HitDetection instance;

    /// <summary>
    /// List of Inputs the player needs to make.
    /// </summary>
    public List<Note> notes;
    private int currentNoteInt = 0;

    private float nextPerfectHit = 0f;
    private float earlyHitOffset;
    private float goodHitOffset;
    private float lateHitOffset;

    /// <summary>
    /// The minigame currently being played.
    /// </summary>
    public MiniGame currentMiniGame;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Setup.
        notes = currentMiniGame.notes;
        earlyHitOffset = currentMiniGame.earlyHitOffset;
        goodHitOffset = currentMiniGame.goodHitOffset;
        lateHitOffset = currentMiniGame.lateHitOffset;
        nextPerfectHit = notes[currentNoteInt].timeToHitAt;
    }

    private void Update()
    {
        // When the player misses the note entirely.
        if ((Conductor.instance.songPosition > nextPerfectHit + goodHitOffset + lateHitOffset))
        {
            currentMiniGame.BadHit(true,false);
        }
    }

    /// <summary>
    /// Updates variables for next hit to be detected.
    /// </summary>
    public void IncrementNextHit()
    {
        if (notes.Count - 1 > currentNoteInt)
        {
            Debug.Log("Incrementing.");
            currentNoteInt++;
            nextPerfectHit = notes[currentNoteInt].timeToHitAt;
        }
        else
        {
            this.enabled = false;
        }
    }

    /// <summary>
    /// Based on the type of input, calls the right function for the minigame to perform.
    /// This is in essence the evaluator.
    /// </summary>
    /// <param name="typeOfHit"></param>
    public void CheckHit(string typeOfHit)
    {
        // Early.
        if (Conductor.instance.songPosition >= nextPerfectHit - goodHitOffset - earlyHitOffset && 
            Conductor.instance.songPosition < nextPerfectHit - goodHitOffset)
        {
            if (typeOfHit == "LETGO")
            {
                currentMiniGame.EarlyHit(true);
            }
            else if (typeOfHit == notes[currentNoteInt].typeOfHit)
            {
                currentMiniGame.EarlyHit(false);
            }
            else
            {
                currentMiniGame.BadHit(true, false);
            }
        }
        // Good.
        else if (Conductor.instance.songPosition >= nextPerfectHit - goodHitOffset && 
            Conductor.instance.songPosition < nextPerfectHit + goodHitOffset)
        {
            if (typeOfHit == "LETGO")
            {
                currentMiniGame.GoodHit(true);
            }
            else if (typeOfHit == notes[currentNoteInt].typeOfHit)
            {
                currentMiniGame.GoodHit(false);
            }
            else
            {
                currentMiniGame.BadHit(true, false);
            }
        }
        // Late.
        else if (Conductor.instance.songPosition >= nextPerfectHit + goodHitOffset &&
            Conductor.instance.songPosition < nextPerfectHit + goodHitOffset + lateHitOffset)
        {
            if (typeOfHit == "LETGO")
            {
                currentMiniGame.LateHit(true);
            }
            else if (typeOfHit == notes[currentNoteInt].typeOfHit)
            {
                currentMiniGame.LateHit(false);
            }
            else
            {
                currentMiniGame.BadHit(true, false);
            }
        }
        // Bad or Miss.
        else
        {
            if (currentNoteInt > 0 && notes[currentNoteInt].continuous && notes[currentNoteInt - 1].continuous && notes[currentNoteInt - 1].typeOfHit == "HOLD")
            {
                if (typeOfHit == "LETGO")
                {
                    currentMiniGame.BadHit(true,true);
                }
                else
                {
                    currentMiniGame.BadHit(true,false);
                }
            }
            else
            {
                // Miss.
                if (typeOfHit == "LETGO")
                {
                    currentMiniGame.BadHit(false, true);
                }
                else
                {
                    currentMiniGame.BadHit(false, false);
                }
            }
        }
    }
}
