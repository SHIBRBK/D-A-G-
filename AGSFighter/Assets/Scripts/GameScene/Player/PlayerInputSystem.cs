using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputSystem : MonoBehaviour
{
    //プレイヤー
    private PlayerAction player;
    private PlayerHit pHit;
    private PlayerAnimationEvent playerAnim;
    private AnimationState animState;
    private PlayerGuard playerGuard;
    private PlayerGuardPose playerGuardPose;

    //動きの量
    private Vector3 moveDirection;
    private float speed;

    //ステップ時の移動量
    [SerializeField]
    private float step = 10.0f;

    //入力確認
    private float rv = 0.0f;
    private float lv = 0.0f;
    private float uv = 0.0f;
    private float ulv = 0.0f;
    private float urv = 0.0f;
    private float dv = 0.0f;

    //ジャンプ
    private bool isJump = false;
    private float jumpSpeed = 50.0f;
    private float jumpSpeedX = 10.0f;

    //技の状態
    [SerializeField]
    private bool attackFlag = false;

    //ジャンプの状態
    [SerializeField]
    private bool jumpFlag = false;

    //ガード
    [SerializeField]
    float guard = 0.0f;
    [SerializeField]
    private float guardMove = 0.0f;
    public bool guardSFlag = false;
    public bool guardSPFlag = false;
    public bool guardCFlag = false;
    public bool guardCPFlag = false;
    [SerializeField]
    private bool assistFlag = false;
    private const float RecoilTime = 0.3f;
    public bool AttackFlag
    {
        get { return attackFlag; }
        set { attackFlag = value; }
    }
    public bool IsJump
    {
        get { return isJump; }
        set { isJump = value; }
    }

    public bool JumpFlag
    {
        get { return jumpFlag; }
        set { jumpFlag = value; }
    }

    public bool GuardSFlag
    {
        get { return guardSFlag; }
    }

    public bool GuardCFlag
    {
        get { return guardCFlag; }
    }

    //右方向の入力の受け渡し
    public float RV()
    {
        return rv;
    }

    //左方向の入力の受け渡し
    public float LV()
    {
        return lv;
    }

    //上方向の入力の受け渡し
    public float UV()
    {
        return uv;
    }

    //上方向の入力の受け渡し
    public float URV()
    {
        return urv;
    }

    //上方向の入力の受け渡し
    public float ULV()
    {
        return ulv;
    }

    //下方向の入力の受け渡し
    public float DV()
    {
        return dv;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>();
        pHit = GetComponent<PlayerHit>();
        playerAnim = GetComponent<PlayerAnimationEvent>();
        animState = GetComponent<AnimationState>();
        playerGuard = this.GetComponentInChildren<PlayerGuard>();
        playerGuardPose = this.GetComponentInChildren<PlayerGuardPose>();
        speed = player.GetSpeed();
    }

    private void FixedUpdate()
    {
        //ステップ
        if (playerAnim.StepL)
        {
            Vector3 moveDirection = new Vector3(-step, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
            player.animState.SetAnimFalse("BackMove");
            player.animState.SetAnimFalse("FrontMove");

            if (!player.anim.GetBool("Mirror"))
            {
                player.animState.SetAnimTrue("StepL");
            }
            else
            {
                player.animState.SetAnimTrue("StepR");
            }
        }
        else if (playerAnim.StepR)
        {
            Vector3 moveDirection = new Vector3(step, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
            player.animState.SetAnimFalse("FrontMove");
            player.animState.SetAnimFalse("BackMove");

            if (!player.anim.GetBool("Mirror"))
            {
                player.animState.SetAnimTrue("StepR");
            }
            else
            {
                player.animState.SetAnimTrue("StepL");
            }
        }

        //ガード処理
        if (guardSFlag || guardCFlag)
        {
            guard++;
        }

        if (pHit.Damage.Item3 != AttackLevel.NullLevel)
        {
            if ((!player.anim.GetBool("Mirror") && lv == 1) || (player.anim.GetBool("Mirror") && rv == 1))
            {
                if (!player.IsCrouching && playerGuard.isGuard && !guardSFlag)
                {
                    ExecuteGuard("StandingGuard");
                    playerGuard.InstantiateGuardEffect();
                    guardSFlag = true;

                    if (lv == 1)
                    {
                        StartCoroutine(Recoil(2f));
                    }
                    else if (rv == 1)
                    {
                        StartCoroutine(Recoil(-2f));
                    }
                }
                else if (!player.IsCrouching && playerGuardPose.isGuard && !guardSPFlag)
                {
                    ExecuteGuard("StandingGuard");
                    guardSPFlag = true;

                    if (lv == 1)
                    {
                        StartCoroutine(Recoil(2f));
                    }
                    else if (rv == 1)
                    {
                        StartCoroutine(Recoil(-2f));
                    }
                }

                if (player.IsCrouching && playerGuard.isGuard && !guardCFlag
                    && (pHit.Damage.Item3 == AttackLevel.Low || pHit.Damage.Item3 == AttackLevel.High))
                {
                    ExecuteGuard("CrouchingGuard");
                    playerGuard.InstantiateGuardEffect();
                    guardCFlag = true;

                    if (lv == 1)
                    {
                        StartCoroutine(Recoil(2f));
                    }
                    else if (rv == 1)
                    {
                        StartCoroutine(Recoil(-2f));
                    }
                }
                else if (player.IsCrouching && playerGuardPose.isGuard && !guardCPFlag)
                {
                    ExecuteGuard("CrouchingGuard");
                    guardCPFlag = true;
                    if (lv == 1)
                    {
                        StartCoroutine(Recoil(2f));
                    }
                    else if (rv == 1)
                    {
                        StartCoroutine(Recoil(-2f));
                    }
                }
            }

            //ガードを解く
            if (!playerGuard.isGuard && !playerGuardPose.isGuard)
            {
                if (guard > 50f)
                {
                    guardSFlag = false;
                    guardSPFlag = false;
                    guardCFlag = false;
                    guardCPFlag = false;
                    playerAnim.CallCancel();
                    guard = 0.0f;
                    player.animState.SetAnimFalse("StandingGuard");
                    player.animState.SetAnimFalse("CrouchingGuard");
                    player.DetectPadHorizontalInput();
                }
            }
        }
    }

    //InputSystem
    //プレイヤー移動右方向
    public void OnMoveFront(InputAction.CallbackContext context)
    {
        // 移動方向を受け取る
        var v = context.ReadValue<float>();
        rv = v;

        if (CanMove())
        {
            //プレイヤー移動
            moveDirection = new Vector3(-v, 0.0f, 0.0f);
            moveDirection = transform.TransformDirection(moveDirection);

            animState.ResetAnim();

            if (!player.anim.GetBool("Mirror"))
            {
                moveDirection = moveDirection * speed;
                player.animState.SetAnimTrue("FrontMove");
            }
            else
            {
                moveDirection = moveDirection * speed / 1.5f;
                player.animState.SetAnimTrue("BackMove");
            }

            if (context.canceled)
            {
                ResetMove();
            }

            player.MoveDirection = moveDirection;
        }
    }

    //左方向
    public void OnMoveBack(InputAction.CallbackContext context)
    {
        // 移動方向を受け取る
        var v = context.ReadValue<float>();
        lv = v;

        if (CanMove())
        {
            //プレイヤー移動
            moveDirection = new Vector3(v, 0.0f, 0.0f);
            moveDirection = transform.TransformDirection(moveDirection);

            animState.ResetAnim();

            if (!player.anim.GetBool("Mirror"))
            {
                moveDirection = moveDirection * speed / 1.5f;
                player.animState.SetAnimTrue("BackMove");
            }
            else
            {
                moveDirection = moveDirection * speed;
                player.animState.SetAnimTrue("FrontMove");
            }

            if (context.canceled)
            {
                ResetMove();
            }

            player.MoveDirection = moveDirection;
        }
    }

    //ステップ
    public void OnStep(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !playerAnim.StepL && !playerAnim.StepR
            && !player.IsCrouching && player.anim.GetBool("Cancel"))
        {
            player.animState.ResetAnim();
            moveDirection.y = 0.0f;

            if (lv == 1)
            {
                playerAnim.StepL = true;
            }
            else if (rv == 1)
            {
                playerAnim.StepR = true;
            }
        }
    }

    //ジャンプ
    public void OnJump(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        uv = v;

        if (!context.started)
        {
            return;
        }

        if (CanJump())
        {
            jumpFlag = true;
            animState.ResetAnim();
            player.IsJumpVertical = true;
            StartCoroutine(Count());
            moveDirection.x = 0.0f;
            moveDirection.y = jumpSpeed;
            player.animState.SetJump(true);

            player.MoveDirection = moveDirection;
        }
    }

    //前ジャンプ
    public void OnJumpR(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        urv = v;
        if (!context.started)
        {
            return;
        }

        if (CanJump())
        {
            animState.ResetAnim();
            jumpFlag = true;
            if (!player.anim.GetBool("Mirror"))
            {
                player.IsJumpFront = true;
            }
            else
            {
                player.IsJumpBack = true;
            }
            StartCoroutine(Count());
            //ジャンプ時の移動量
            moveDirection.x = jumpSpeedX;
            moveDirection.y = jumpSpeed;
            player.animState.SetJump(true);

            player.MoveDirection = moveDirection;
        }
    }

    //後ろジャンプ
    public void OnJumpL(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        ulv = v;
        if (!context.started)
        {
            return;
        }

        if (CanJump())
        {
            animState.ResetAnim();
            jumpFlag = true;
            if (!player.anim.GetBool("Mirror"))
            {
                player.IsJumpBack = true;
            }
            else
            {
                player.IsJumpFront = true;
            }
            StartCoroutine(Count());
            //ジャンプ時の移動量
            moveDirection.x = -jumpSpeedX;
            moveDirection.y = jumpSpeed;
            player.animState.SetJump(true);

            player.MoveDirection = moveDirection;
        }
    }

    public bool CanJump()
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !player.IsCrouching && !playerAnim.StepL && !playerAnim.StepR 
            && player.anim.GetBool("JumpSF") == false && jumpFlag == false 
            && player.anim.GetBool("Cancel");
    }

    public void AssistButton(InputAction.CallbackContext context)
    {
        assistFlag = true;
        if(context.canceled)
        {
            assistFlag = false;
        }
    }

    //弱攻撃
    public void OnNWP(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        if (CanNormalAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("N_Weak");
        }
        else if(CanJumpAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteJumpAttack("JumpKick");
        }
        else if (CanNormalAttack(player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("C_Weak");
        }
        else if(CanNormalAttack(!player.IsCrouching) && assistFlag)
        {
            ExecuteCombo("NA_Weak");
        }
    }

    //中攻撃
    public void OnNMP(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (CanNormalAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("N_Mid");
        }
        else if (CanJumpAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteJumpAttack("JumpKick2");
        }
        else if (CanNormalAttack(player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("C_Mid");
        }
        else if(CanNormalAttack(!player.IsCrouching) && assistFlag)
        {
            ExecuteCombo("NA_Mid");
        }
    }

    //強攻撃
    public void OnNSP(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (CanNormalAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("N_Stro");
        }
        else if (CanJumpAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteJumpAttack("JumpPunch");
        }
        else if (CanNormalAttack(player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("Ashibarai");
        }
        else if (CanNormalAttack(!player.IsCrouching) && assistFlag)
        {
            ExecuteCombo("NA_Stro");
        }
    }

    public void OnSakotsuwari(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, !player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Sakotsuwari");
        }
    }

    public void OnMirrorSakotsuwari(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Sakotsuwari");
        }
    }

    //波動拳
    public void OnHadoken(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !player.IsCrouching && !attackFlag
            && player.anim.GetBool("Cancel"))
        {
            ExecuteAttack("Hadoken");
        }
    }

    //昇竜拳
    public void OnSyoryuken(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, !player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Syoryuken");
        }
    }

    public void OnMirrorSyoryuken(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching,player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Syoryuken");
        }
    }

    //竜巻旋風脚
    public void OnTatsumaki(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, !player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Tatsumaki");
        }
    }

    public void OnMirrorTatsumaki(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Tatsumaki");
        }
    }

    public void OnHasyogeki(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && player.IsCrouching && !attackFlag && player.anim.GetBool("Cancel"))
        {
            ExecuteAttack("Hasyogeki");
        }

    }

    //しゃがみ
    public void OnCrouching(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        dv = v;

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround())
        {
            if (context.started)
            {
                player.animState.SetAnimTrue("CStart");
                player.animState.SetAnimFalse("FrontMove");
                player.animState.SetAnimFalse("BackMove");
                player.IsCrouching = true;
                moveDirection = Vector3.zero;
                player.MoveDirection = moveDirection;
            }
            else if (context.canceled)
            {
                player.animState.SetAnimFalse("Crouching");
                player.animState.SetAnimFalse("CStart");
                player.IsCrouching = false;
                //左右入力確認
                player.DetectPadHorizontalInput();
            }
        }
    }

    private void ResetMove()
    {
        player.animState.SetAnimFalse("FrontMove");
        player.animState.SetAnimFalse("BackMove");
        player.animState.SetAnimFalse("StandingGuard");
        player.animState.SetAnimFalse("CrouchingGuard");
    }

    //攻撃時のアニメーションと移動処理
    private void ExecuteAttack(string attackName)
    {
        if(player.anim.GetBool(attackName))
        {
            player.animState.SetAnimFalse(attackName);
            StartCoroutine(DelayAnim(attackName));
            return;
        }

        animState.ResetAnim();

        player.animState.SetAnimTrue(attackName);
        attackFlag = true;
        moveDirection *= 0;
        player.MoveDirection = moveDirection;
    }

    private void ExecuteCombo(string attackName)
    {
        if (player.anim.GetBool(attackName))
        {
            StartCoroutine(DelayAnim(attackName));
            return;
        }

        animState.ResetAnim();

        player.animState.SetAnimTrigger(attackName);
        attackFlag = true;
        moveDirection *= 0;
        player.MoveDirection = moveDirection;
    }

    private void ExecuteGuard(string guardName)
    {
        animState.ResetAnim();

        player.animState.SetAnimTrue(guardName);
        player.MoveDirection = moveDirection;
    }

    private void ExecuteJumpAttack(string attackName)
    {
        player.animState.SetAnimTrue(attackName);
        attackFlag = true;
    }

    //非同期処理
    //ジャンプした瞬間にtrueにするとジャンプしなくなるので遅延させる
    public IEnumerator Count()
    {
        yield return new WaitForSeconds(0.5f);
        isJump = true;
    }

    //ガード後のヒットバック
    IEnumerator Recoil(float speed)
    {
        //後退速度
        float recoilSpeed = speed;
        float timeElapsed = 0.0f;

        while (timeElapsed < RecoilTime)
        {
            float currentSpeed = recoilSpeed * Mathf.Sqrt(1 - Mathf.Pow(timeElapsed / RecoilTime, 2));
            moveDirection -= new Vector3(currentSpeed * Time.deltaTime, 0, 0);
            timeElapsed += Time.deltaTime;
            player.MoveDirection = moveDirection;
            yield return null;
        }

        playerGuard.IsGuard = false;
        playerGuardPose.IsGuard = false;
        player.DetectPadHorizontalInput();
    }

    IEnumerator DelayAnim(string attackName)
    {
        yield return new WaitForSeconds(0.001f);

        player.animState.SetAnimTrue(attackName);
        attackFlag = true;
        moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    //bool関数
    private bool CanMove()
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !player.IsCrouching && !playerAnim.StepL && !playerAnim.StepR
            && player.anim.GetBool("Cancel") && player.anim.GetInteger("Jump") == 0
            && !player.anim.GetBool("JumpSF") && !playerGuard.isGuard;
    }

    private bool CanNormalAttack(bool isCrouching)
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && isCrouching && !attackFlag && player.anim.GetBool("Cancel");
    }

    private bool CanJumpAttack(bool isCrouching)
    {
        return player.State == PlayerAction.MyState.Game && player.anim.GetBool("JumpSF") == true
            && isCrouching && !attackFlag && player.anim.GetBool("Cancel");
    }

    private bool MirrorAnimation(bool isCrouching, bool mirror)
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && isCrouching && !attackFlag && mirror && player.anim.GetBool("Cancel");
    }
}
