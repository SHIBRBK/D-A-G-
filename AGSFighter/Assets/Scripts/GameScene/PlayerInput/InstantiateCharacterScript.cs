using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstantiateCharacterScript : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("参加");
        foreach (var device in playerInput.devices)
        {
            Debug.Log("操作デバイス" + device);
        }
    }
}