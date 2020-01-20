using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public float earlyHitOffset;
    public float goodHitOffset;
    public float lateHitOffset;
    public List<Note> notes;


    // Depending on the minigame, the designer (you) might want to have different effects, or no effects at all
    // for the Early and Late hits. That is why this is a parent script to the minigame scripts.

    public virtual void EarlyHit()
    {
        Debug.Log("Early hit");
    }

    public virtual void GoodHit()
    {
        HitDetection.instance.IncrementNextHit();
        Debug.Log("Good hit");
    }

    public virtual void LateHit()
    {
        Debug.Log("Late hit");
    }

    public virtual void BadHit(bool isWrong)
    {
        if (isWrong)
        {
            HitDetection.instance.IncrementNextHit();
        }
        Debug.Log("Bad hit");
    }
}
