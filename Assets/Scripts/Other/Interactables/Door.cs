using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 openPositionOffset; // The offset from the original position when the door is open
    [SerializeField] private float moveDuration = 2.0f; // Duration to open/close the door

    private bool isOpen = false;
    private Vector3 closedPosition; // The original position of the door (when closed)
    private Coroutine moveCoroutine; // Reference to the current moving coroutine

    private void Start()
    {
        // Initialize the closed position
        closedPosition = transform.position;
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            MoveDoor(closedPosition);
        }
        else
        {
            MoveDoor(closedPosition + openPositionOffset);
        }
        isOpen = !isOpen;
    }

    private void MoveDoor(Vector3 targetPosition)
    {
        if (moveCoroutine != null)
        {
            // If there's already a move coroutine running, stop it
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveDoorCoroutine(targetPosition));
    }

    private IEnumerator MoveDoorCoroutine(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        transform.position = targetPosition;
    }
}