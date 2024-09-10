using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float pushDistance = 1.0f;
    [SerializeField]
    private string pushTag;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(pushTag))
        {
            Vector3 pushDirection = other.transform.position - transform.position;
            pushDirection = pushDirection.normalized * pushDistance;
            controller.Move(pushDirection);
        }
    }

    void Update()
    {
        transform.position = playerTransform.position;
    }
}
