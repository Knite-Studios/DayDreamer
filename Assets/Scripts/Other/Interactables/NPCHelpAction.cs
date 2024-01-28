using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHelpAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private Vector3 moveDirection = new Vector3(0, -1, 0);
    [SerializeField] private float moveSpeed = 1f; // Speed of the move
    [SerializeField] private float destroyDelay = 2f; // Time before the GameObject is destroyed
    [SerializeField] private AudioClip fallingSound; // Sound effect to play while the GameObject is moving

    private AudioSource audioSource;
    private bool isActionTriggered = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on the object. Please add an AudioSource component.");
        }
    }

    public void ExecuteAction()
    {
        if (objectToMove == null)
        {
            Debug.LogError("Object to move is not assigned in the NPCHelpAction script.");
            return;
        }

        if (!isActionTriggered)
        {
            isActionTriggered = true;
            StartCoroutine(MoveAndDestroyObject());
        }
    }

    private IEnumerator MoveAndDestroyObject()
    {
        // Play the falling sound
        if (fallingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fallingSound);
        }

        // Move the object down
        float elapsedTime = 0;
        while (elapsedTime < destroyDelay)
        {
            objectToMove.transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the object after the delay
        Destroy(objectToMove);
        Destroy(gameObject);
    }
}
