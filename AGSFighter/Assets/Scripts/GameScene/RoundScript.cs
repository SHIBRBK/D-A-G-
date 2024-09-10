using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundScript : MonoBehaviour
{
    void Update()
    {
        // Y²‚ğ’†S‚É‚P•b‚É‚Â‚«|12“x‰ñ“]‚·‚é
        transform.Rotate(new Vector3(0, -5) * Time.deltaTime);
    }
}
