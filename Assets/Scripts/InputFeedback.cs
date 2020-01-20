using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFeedback : MonoBehaviour
{
    private MeshRenderer spriteRenderer;
    private Rigidbody2D rig;

    // This script is a visualizer of the player input. 
    // It is attached to an object necessary to calculate the flick with.

    void Start()
    {
        spriteRenderer = GetComponent<MeshRenderer>();
        rig = GetComponent<Rigidbody2D>();
    }

    private void OnBecameInvisible()
    {
        spriteRenderer.enabled = false;
        rig.Sleep();
    }

    public void BecomeVisible()
    {
        spriteRenderer.enabled = true;
        rig.velocity = Vector2.zero;
        rig.WakeUp();
    }
}
