using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIChange : MonoBehaviour
{
    //デバイス確認用
    [SerializeField] 
    private PlayerInput playerInput;
    //コントローラー用UI
    [SerializeField]
    private List<GameObject> tutoPadUI = new List<GameObject>();
    //キーボード用UI
    [SerializeField]
    private List<GameObject> tutoKeyUI = new List<GameObject>();

    private bool keyboardActive = false;

    private void Update()
    {
        // PlayerInputがnullであるか、有効なプレイヤーではない場合は処理しない
        if (playerInput == null || !playerInput.user.valid)
        {
            Debug.Log("アクティブなプレイヤーではありません");
            return;
        }

        // Keyboardがアクティブな場合はKeyboard UIを表示し、それ以外の場合はPad UIを表示
        foreach (var device in playerInput.devices)
        {
            if (device.name == "Keyboard")
            {
                foreach(var pad in tutoPadUI)
                {
                    pad.SetActive(false);
                }
                foreach (var key in tutoKeyUI)
                {
                    key.SetActive(true);
                }
                keyboardActive = true;
                // Keyboardが見つかったら即座にリターン
                return; 
            }
            else
            {
                keyboardActive = false;
            }
        }

        // Keyboardが見つからなかった場合はPad UIを表示
        if (!keyboardActive)
        {
            foreach (var pad in tutoPadUI)
            {
                pad.SetActive(true);
            }
            foreach (var key in tutoKeyUI)
            {
                key.SetActive(false);
            }
        }
    }
}
