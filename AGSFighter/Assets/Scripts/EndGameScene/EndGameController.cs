using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    [SerializeField]
    private GameObject yuji;
    [SerializeField]
    private GameObject yuji2;
    [SerializeField]
    private GameObject yujiPos, yuji2Pos;
    public Text winner1Text, winner2Text;
    [SerializeField]
    private GameObject p1Button, p2Button, p1Text, p2Text;
    [SerializeField]
    private Button p1RematchButton, p2RematchButton;

    void Start()
    {
        RoundManager.Instance.ResetRoundCount();

        //èüóòÇµÇΩï˚óp
        if (GameResult.winnerMessage == "Player 1 Wins the Game!")
        {
            Instantiate(yuji, yujiPos.transform.position, yujiPos.transform.rotation);
            p1RematchButton.Select();
            p1Button.SetActive(true);
            p1Text.SetActive(true);
            if (winner1Text != null)
            {
                winner1Text.text = GameResult.winnerMessage;
            }
        }
        else
        {
            Instantiate(yuji2, yuji2Pos.transform.position, yuji2Pos.transform.rotation);
            p2RematchButton.Select();
            p2Button.SetActive(true);
            p2Text.SetActive(true);
            if (winner2Text != null)
            {
                winner2Text.text = GameResult.winnerMessage;
            }
        }

    }
}
