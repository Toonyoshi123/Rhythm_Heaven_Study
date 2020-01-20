using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
    // This can be changed to beats, but that requires a whole lot of calculations on the developer's end.
    public float timeToHitAt = 0f;

    // In all caps: either FLICK TAP HOLD or LETGO.
    public string typeOfHit = "";

    // Add this option to both this and next note if this one starts the held note.
    // The HitDetection makes a distinction between the states.
    public bool continuous = false;
}
