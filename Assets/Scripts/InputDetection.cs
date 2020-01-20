using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Types of input.
/// </summary>
public enum InputState
{
    TAP,
    HOLD,
    FLICK,
    LETGO
}

public class InputDetection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // This script reads the player input and determines the type of input.
    // However, the hold variable needs almost too long to be useful. Needs improvement.

    public static InputDetection instance;

    private Conductor conductor;
    private InputFeedback circle;
    public GameObject cursorObject;

    public InputState currentInputState = InputState.LETGO;

    private bool isPointerDown = false;
    private float elapsedTimePress = 0f;

    public float tapTimeTreshold;

    private float currentSpeed;
    public float speedTreshold;
    Vector3 startPositionThisFrame;
    Vector3 positionLastFrame = new Vector3();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        conductor = Conductor.instance;
        circle = FindObjectOfType<InputFeedback>();
    }

    private void Update()
    {
        // Calculations per frame held down.
        if (isPointerDown)
        {
            elapsedTimePress += Time.deltaTime;
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                var newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
                cursorObject.GetComponent<Rigidbody2D>().MovePosition(newPosition);
                circle.transform.position = newPosition;
                startPositionThisFrame = newPosition;
                currentSpeed = circle.GetComponent<Rigidbody2D>().velocity.magnitude;
            }
        }
        // Check whether or not the player is holding down the pointer.
        if ((elapsedTimePress > 0 && elapsedTimePress > tapTimeTreshold) && 
            currentSpeed < speedTreshold)
        {
            // Hold.
            Debug.Log("Hold.");
            InputStateChange(InputState.HOLD);
        }
    }

    private void LateUpdate()
    {
        positionLastFrame = startPositionThisFrame;
    }

    // Only called once, sets up variables for calculations.
    public void OnPointerDown(PointerEventData eventData)
    {
        currentSpeed = 0f;
        isPointerDown = true;
        cursorObject.SetActive(true);
        cursorObject.GetComponent<FixedJoint2D>().connectedBody = circle.GetComponent<Rigidbody2D>();
        circle.transform.position = cursorObject.transform.position;
        circle.BecomeVisible();
    }

    // Only called once
    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;

        // Calculate type of input.
        if (elapsedTimePress > 0 && 
            elapsedTimePress < tapTimeTreshold)
        {
            // Tap.
            Debug.Log("Tap.");
            InputStateChange(InputState.TAP);
        }
        else if ((elapsedTimePress > 0 && 
            elapsedTimePress > tapTimeTreshold) && 
            currentSpeed > speedTreshold)
        {
            // Flick.
            Debug.Log("Flick.");
            InputStateChange(InputState.FLICK);
        }
        else
        {
            // Let go.
            InputStateChange(InputState.LETGO);
        }

        // Reset variables.
        cursorObject.SetActive(false);
        elapsedTimePress = 0f;
    }

    /// <summary>
    /// Returns the current inputState back to letGo.
    /// </summary>
    private IEnumerator InputStateChangeBack()
    {
        yield return new WaitForFixedUpdate();
        InputStateChange(InputState.LETGO);
        yield break;
    }

    /// <summary>
    /// Changes the current state to the new state.
    /// </summary>
    /// <param name="newState"></param>
    private void InputStateChange(InputState newState)
    {
        currentInputState = newState;
        HitDetection.instance.CheckHit(currentInputState.ToString());
        if (newState != InputState.HOLD && newState != InputState.LETGO)
        {
            StartCoroutine("InputStateChangeBack");
        }
    }
}
