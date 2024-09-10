using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class ControllerManager : SingletonBehaviour<ControllerManager>
{
    // コントローラリスト: 追加順に登録
    private List<GameObject> m_controllers = new List<GameObject>();

    // コントローラ生存確認
    public bool IsController(int controller_id)
    {
        if (controller_id >= m_controllers.Count) { return false; }
        return true;
    }

    // コントローラ情報取得
    public ControllerBehaviour GetController(int controller_id)
    {
        return m_controllers[controller_id].GetComponent<ControllerBehaviour>();
    }

    // コントローラ追加処理
    public void OnPlayerJoined(PlayerInput player_input)
    {
        var controller_obj = player_input.gameObject;

        controller_obj.transform.parent = this.transform;
        m_controllers.Add(controller_obj);
    }
}