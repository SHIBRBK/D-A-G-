using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

// X:0, Y:1, B:2, A:3

public class ControllerBehaviour : MonoBehaviour
{
    public enum ButtonState : int
    {
        NONE = 0x00,
        PRESS = 0x01,
        HOLD = 0x02,
        RELEASE = 0x03,
    }

    private Vector2 m_cross_key_value = Vector2.zero;
    private ButtonState[] m_key_state = new ButtonState[4];

    private bool[] m_current_normal_button_value = new bool[4];
    private bool[] m_previous_normal_button_value = new bool[4];

    // 十字キー
    public Vector2 CrossKeyValue
    {
        get { return m_cross_key_value; }
    }
    // 4ボタン
    public ButtonState NormalButtonX { get { return m_key_state[0]; } }
    public ButtonState NormalButtonY { get { return m_key_state[1]; } }
    public ButtonState NormalButtonB { get { return m_key_state[2]; } }
    public ButtonState NormalButtonA { get { return m_key_state[3]; } }

    private void Update()
    {
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        // ボタン状態の更新
        for (int i = 0; i < 4; ++i)
        {
            m_key_state[i] = CheckButtonState(m_previous_normal_button_value[i], m_current_normal_button_value[i]);
            m_previous_normal_button_value[i] = m_current_normal_button_value[i];
        }
    }

    // キー状態を取得
    private ButtonState CheckButtonState(bool previous, bool current)
    {
        if (!previous && current) { return ButtonState.PRESS; }
        if (previous && current) { return ButtonState.HOLD; }
        if (previous && !current) { return ButtonState.RELEASE; }

        return ButtonState.NONE;
    }

    // イベントハンドラ
    // 十字キー
    public void OnCrossKey(InputAction.CallbackContext context)
    {
        m_cross_key_value = context.ReadValue<Vector2>();
    }
    // 4ボタン
    public void OnNormalButtonX(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint) { m_current_normal_button_value[0] = true; }
        else { m_current_normal_button_value[0] = false; }
    }
    public void OnNormalButtonY(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint) { m_current_normal_button_value[1] = true; }
        else { m_current_normal_button_value[1] = false; }
    }
    public void OnNormalButtonB(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint) { m_current_normal_button_value[2] = true; }
        else { m_current_normal_button_value[2] = false; }
    }
    public void OnNormalButtonA(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint) { m_current_normal_button_value[3] = true; }
        else { m_current_normal_button_value[3] = false; }
    }

}