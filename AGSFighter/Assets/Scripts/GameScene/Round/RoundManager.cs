using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    public GameObject player1;
    public GameObject player2;


    public int player1Wins = 0;
    public int player2Wins = 0;
    public int currentRound = 1;
    public int totalRounds = 3;
    [SerializeField] string selectedStage; // 選択されたステージ名を保存する変数

    public bool isRoundOver = false; // 追加

    [SerializeField]private RentalUIAnimation testUI;


    // CountDownスクリプトの参照
    [SerializeField] private CountDown countDown;


    [SerializeField] private WinStarsUI player1WinStarsUI;
    [SerializeField] private WinStarsUI player2WinStarsUI;

    private Dictionary<string, string> stageBGMMapping;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeStageBGMMapping();
    }

    private void Start()
    {
    
    }

    private void InitializeStageBGMMapping()
    {
        stageBGMMapping = new Dictionary<string, string>
        {
            { "SelectStage1", "トレーニングメニュー" },
            { "SelectStage2", "飛翔鉄塔" },
            { "SelectStage3", "第３鉄工所" },
            // 他のステージ-BGMのペアを追加
        };
    }

    public void Create()
    {
        player1 = GameObject.Find("yuji");
        player2 = GameObject.Find("yuji2P");
        // CountDownスクリプトの参照を取得
        countDown = FindObjectOfType<CountDown>();
        player1WinStarsUI =
            GameObject.Find(
                "HP&SP").transform.Find(
                "Player1WinStars").GetComponent<WinStarsUI>();
        player2WinStarsUI =
            GameObject.Find(
                "HP&SP").transform.Find(
                "Player2WinStars").GetComponent<WinStarsUI>();
    }

    public void Player1Wins()
    {
        if (isRoundOver) return; // 追加
        Debug.LogWarning("プレイヤー1の勝ち");
        
        isRoundOver = true; // 追加
        player1Wins++;
        UpdateWinStarsUI();
        CheckGameOver();
    }

    public void Player2Wins()
    {
        if (isRoundOver) return; // 追加
        Debug.LogWarning("プレイヤー2の勝ち");
        
        isRoundOver = true; // 追加
        player2Wins++;
        UpdateWinStarsUI();
        CheckGameOver();
    }

    public void PlayerDraw()
    {
        if (isRoundOver) return; // 追加]
        Debug.LogWarning("引き分け！");

        isRoundOver = true; // 追加
        player1Wins++;
        player2Wins++;
        UpdateWinStarsUI();
        CheckGameOver();
    }

    void CheckGameOver()
    {
        isRoundOver = false; // 追加
        Debug.Log("CheckGameOver called");
        if (player1Wins == player2Wins && 
            player1Wins
            >= totalRounds / 2 + 1)
        {
            // プレイヤー1が勝利
            Debug.Log("Draw Game");
            EndGame("Draw…");
        }
        else if (player1Wins >= totalRounds / 2 + 1)
        {
            // プレイヤー1が勝利
            Debug.Log("Player 1 Wins the Game!");
            EndGame("Player 1 Wins the Game!");
        }
        else if (player2Wins >= totalRounds / 2 + 1)
        {
            // プレイヤー2が勝利
            Debug.Log("Player 2 Wins the Game!");
            EndGame("Player 2 Wins the Game!");
        }
        else
        {
            // 次のラウンドへ
            currentRound++;
            if (currentRound > totalRounds)
            {
                // 一定の勝利数に達していない場合、勝ち星とSAゲージをリセットし、ラウンドをリスタート
                ResetScoresAndSA();
                StartCoroutine(EndRoundCoroutine());
            }
            else
            {
                StartCoroutine(StartRound());
            }
        }
    }

    private IEnumerator StartRound()
    {
        Debug.Log("StartRound called");

        yield return FadeController.Instance.StartCoroutine(FadeController.Instance.FadeOut());
        FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
       


        if (player1 != null && player2 != null)
        {

            player1.GetComponent<PlayerHit>().ResetPlayer();
            player2.GetComponent<PlayerHit>().ResetPlayer();

            testUI.Start();
            GameObject.Find("RoundCount").transform.GetComponent<RoundCount>().RoundText();
        }

        else
        {
            Debug.LogError("プレイヤーが見つかりません。");
        }

    }

    void ResetScoresAndSA()
    {
        player1Wins = 0;
        player2Wins = 0;
        Debug.Log("Scores and SA reset");

        // 他のリセット処理を追加する場合はここに記述
    }
   

    private IEnumerator EndRoundCoroutine()
    {

        yield return new WaitForSeconds(1f); // 少しの猶予を置く                                    // シーンリロード
        ReloadScene();
         yield return new WaitForSeconds(1f); // シーンリロード後の猶予
    }

    private void ReloadScene()
    {
        if (!string.IsNullOrEmpty(selectedStage))
        {
            SceneManager.LoadScene(selectedStage);
        }
        else
        {
            Debug.LogError("selectedStage変数にシーン名が設定されていません。");
        }
    }

    void EndGame(string winnerMessage)
    {
        Debug.Log("EndGame called with message: " + winnerMessage);
        GameResult.winnerMessage = winnerMessage;
        SceneManager.LoadScene("EndGameScene");
        SoundManager.Instance.StopBGM();
    }

    public void SetSelectedStage(string stageName)
    {
        selectedStage = stageName;
        PlayStageBGM(stageName);
    }

    private void PlayStageBGM(string stageName)
    {
        if (stageBGMMapping.TryGetValue(stageName, out string bgmName))
        {
            Debug.Log($"ステージ: {stageName} のBGM: {bgmName} を再生");
            SoundManager.Instance.PlayBGM(bgmName);
        }
        else
        {
            Debug.LogError($"ステージ: {stageName} に対応するBGMが見つかりません");
        }
    }

    public void NotifyPlayerDefeated(GameObject player)
    {
        if (isRoundOver) return; // 追加
        countDown.ResetCountDown();
        Debug.LogWarning("通知来たよー");
        Debug.LogWarning("こいつが殺られた: " + player.name);

        if (player == player1)
        {
            Debug.LogWarning(player.name + "の死亡を確認 (player1)");
            Player2Wins();
        }
        else if (player == player2)
        {
            Debug.LogWarning(player.name + "の死亡を確認 (player2)");
            Player1Wins();
        }
        else
        {
            Debug.LogError("参照が一致しません。");
            Debug.LogError("player" + player.GetInstanceID());
            Debug.LogError("player1" + player1.GetInstanceID());
            Debug.LogError("player2" + player2.GetInstanceID());
        }
    }

    public void TimeOut()
    {
        if (player1 != null && player2 != null)
        {
            if (player1.GetComponent<PlayerHit>().hpGauge._currentHP > player2.GetComponent<PlayerHit>().hpGauge._currentHP)
            {
                Player1Wins();
            }
            else if (player1.GetComponent<PlayerHit>().hpGauge._currentHP < player2.GetComponent<PlayerHit>().hpGauge._currentHP)
            {
                Player2Wins();
            }
            else
            {
                // 体力が同じ場合の処理（引き分け扱いにするなど）
                Debug.Log("Draw! Both players have the same health.");
                PlayerDraw();

            }
        }
        else
        {
            Debug.LogError("プレイヤーが見つかりません。");
        }
    }


    private void UpdateWinStarsUI()
    {
        player1WinStarsUI.UpdateWinStars(player1Wins);
        player2WinStarsUI.UpdateWinStars(player2Wins);
    }

    private void ResetWinStarsUI()
    {
        player1WinStarsUI.ResetWinStars();
        player2WinStarsUI.ResetWinStars();
    }

    public void ResetRoundCount()
    {
        player1Wins = 0;
        player2Wins = 0;
        currentRound = 1;
    }
}
