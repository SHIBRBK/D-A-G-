using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimationEvent : MonoBehaviour
{
    //ジャンプ用ステート
    public enum JumpState
    {
        State0,
        State1,
        State2,
        State3,
        State4,
        State5,
    }

    private PlayerAction player;
    [SerializeField]
    private GameObject playerPos;

    [SerializeField]
    private CinemachineImpulseSource cineCamera;

    bool stepR, stepL = false;

    //波動拳
    [SerializeField]
    GameObject hadoken;
    [SerializeField]
    Hadoken hadokenMove;
    bool hadoFlag = false;
    [SerializeField]
    private GameObject hadokenPosR;
    [SerializeField]
    private GameObject hadokenPosL;
    [SerializeField]
    private string HadoHitPlayer;
    [SerializeField]
    private string originalLayer;

    //アニメーションの移動
    private float tatsumakiMove = 25.0f;
    private float syoryuMove = 10.0f;
    private float syoryuMoveY = 50f;
    private bool tatsumakiMoveFlag = false;
    private bool syoryuMoveFlag = false;
    private bool sakotsuwariMove = false;

    private float hitMove = 15.0f;
    private bool hitFlag = false;

    //硬直時間
    private bool rigorFlag = false;
    private float rigorTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>();
    }

    private void Update()
    {
        if (tatsumakiMoveFlag)
        {
            float offset = player.anim.GetBool("Mirror") ? -tatsumakiMove : tatsumakiMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
        else if(sakotsuwariMove)
        {
            float offset = player.anim.GetBool("Mirror") ? -tatsumakiMove : tatsumakiMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
        else if(syoryuMoveFlag)
        {
            float offset = player.anim.GetBool("Mirror") ? -syoryuMove : syoryuMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            moveDirection.y = syoryuMoveY;
            player.MoveDirection = moveDirection;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            player.animState.SetAnimTrue("Huttobi");
        }

        if(hitFlag)
        {
            gameObject.layer = LayerMask.NameToLayer("HitNow"); 
            float offset = player.anim.GetBool("Mirror") ? hitMove : -hitMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }

        //硬直時間
        if(!rigorFlag)
        {
            rigorTime = 0.0f;
        }
    }

    //アニメーションイベント
    //波動拳生成

    void Hadoken()
    {
        if(!hadoFlag)
        {
            StartCoroutine(HadoFlag());
            Vector3 direction = player.anim.GetBool("Mirror") ? Vector3.left : Vector3.right;
            hadokenMove.Initialize(direction, HadoHitPlayer);
            Instantiate(hadoken, player.anim.GetBool("Mirror") ? hadokenPosL.transform.position : hadokenPosR.transform.position, Quaternion.identity);
        }
    }

    //波動拳を複数個生成させないため
    IEnumerator HadoFlag()
    {
        hadoFlag = true;
        yield return new WaitForSeconds(0.5f);
        hadoFlag = false;
    }

    //波動拳アニメーション終了
    void HadoFinish()
    {
        player.animState.SetAnimFalse("Hadoken");
        //左右入力確認
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void HasyogekiFinish()
    {
        player.animState.SetAnimFalse("Hasyogeki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void SyoryuMove()
    {
        syoryuMoveFlag = true;
    }

    void SyoryuMoveFalse()
    {
        syoryuMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.MoveDirection = moveDirection;
    }

    void SyoryuFinish()
    {
        player.animState.SetAnimFalse("Syoryuken");
        //左右入力確認
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void TatsumakiMove()
    {
        tatsumakiMoveFlag = true;
    }

    void TatsumakiMoveFalse()
    {
        tatsumakiMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.controller.Move(moveDirection);
    }

    void TatsumakiFalse()
    {
        player.animState.SetAnimFalse("Tatsumaki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    //攻撃アニメーション終了
    void NPFinish()
    {
        player.animState.SetAnimFalse("N_Weak");
        player.animState.SetAnimFalse("N_Mid");
        player.animState.SetAnimFalse("N_Stro");
        //左右入力確認
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void NAFinish()
    {
        player.animState.SetAnimFalse("NA_Mid");
        player.DetectPadHorizontalInput();
    }

    void CKFinish()
    {
        player.animState.SetAnimFalse("C_Weak");
        player.animState.SetAnimFalse("C_Mid");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    //踏み込む動作があるため、少し前へ動く
    void AshibaraiMove()
    {
        float offset = player.anim.GetBool("Mirror") ? -2f : 2f;
        Vector3 moveDirection = new Vector3(offset, 0, 0);
        player.controller.Move(moveDirection);
    }

    //足払い終了
    void AshibaraiFalse()
    {
        player.animState.SetAnimFalse("Ashibarai");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    //鎖骨割り移動
    void SakotsuwariMove()
    {
        sakotsuwariMove = true;
    }

    //鎖骨割り終了
    void SakotsuwariFalse()
    {
        player.animState.SetAnimFalse("Sakotsuwari");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void CameraImpulse()
    {
        cineCamera.GenerateImpulse();
    }

    //前ステップ終了
    void StepRF()
    {
        player.animState.SetAnimFalse("StepR");
        StartCoroutine(StepFalse());
    }

    //前ステップ移動終了
    void StepRMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    //後ろステップ終了
    void StepLF()
    {
        player.animState.SetAnimFalse("StepL");
        StartCoroutine(StepFalse());
    }

    //後ろステップ移動終了
    void StepLMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }


    //すべての方向のジャンプアニメーション開始
    void JStart()
    {
        //ジャンプ開始→ジャンプ中(垂直、前、後ろ)
        if (player.IsJumpVertical)
        {
            player.animState.SetJumpState((int)JumpState.State2);
        }
        else if (player.IsJumpFront)
        {
            player.animState.SetJumpState((int)JumpState.State3);
        }
        else if (player.IsJumpBack)
        {
            player.animState.SetJumpState((int)JumpState.State4);
        }
    }

    //ジャンプアニメーション終了
    void JFinish()
    {
        player.animState.SetJumpState((int)JumpState.State0);
        player.animState.SetJump(false);
        player.IsJumpVertical = false;
        player.IsJumpFront = false;
        player.IsJumpBack = false;
        player.playerInput.JumpFlag = false;
        StartCoroutine(AttackFalse());
        player.DetectPadHorizontalInput();
    }

    void JumpFF()
    {
        player.animState.ResetAnim();
    }

    //しゃがみ開始アニメーション開始
    void CStart()
    {
        player.animState.SetAnimTrue("Crouching");
    }

    //しゃがみ開始アニメーション終了
    void CStartFalse()
    {
        player.animState.SetAnimFalse("CStart");
    }

    void HitRigorTime()
    {
        player.animState.SetAnimTrue("Cancel");
        rigorFlag = true;
    }

    void HitLayer()
    {
        hitFlag = true;
    }

    void HitMoveFalse()
    {
        hitFlag = false;
    }

    void HitFalse()
    {
        player.animState.SetAnimFalse("SWeakHit");
        player.animState.SetAnimFalse("SMidHit");
        player.animState.SetAnimFalse("SStrongHit");
        player.animState.SetAnimFalse("Huttobi");
        gameObject.layer = LayerMask.NameToLayer(originalLayer);
        player.DetectPadHorizontalInput();
    }

    //キャンセルフレーム
    void Cancel()
    {
        player.animState.SetAnimTrue("Cancel");
        StartCoroutine(AttackFalse());
        stepL = false;
        stepR = false;
    }

    void CancelFalse()
    {
        player.animState.SetAnimFalse("Cancel");
    }

    public bool StepR
    {
        get { return stepR; }
        set { stepR = value; }
    }

    public bool StepL
    {
        get { return stepL; }
        set { stepL = value; }
    }

    //攻撃終了に遅延
    public IEnumerator AttackFalse()
    {
        yield return 0.1f;
        player.playerInput.AttackFlag = false;
    }

    public IEnumerator StepFalse()
    {
        yield return 0.1f;
        //左右入力確認
        player.DetectPadHorizontalInput();
        stepL = false;
        stepR = false;
    }

    public void CallCancel()
    {
        Cancel();
    }
}
