using UnityEngine;
using UnityEngine.UI;

public class PlayerTransform : MonoBehaviour
{
   public Transform playerTransform;


    void Start()
    {
        playerTransform.position = transform.position;
        playerTransform.rotation = transform.rotation;
        playerTransform.localScale = transform.localScale;
    }

    public void ResetPosition()
    {
        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
        transform.localScale = playerTransform.localScale;
    }

    // ä˘ë∂ÇÃÉRÅ[Éh...
}
