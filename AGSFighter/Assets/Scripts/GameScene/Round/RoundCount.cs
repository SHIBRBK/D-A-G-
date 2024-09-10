using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCount : MonoBehaviour
{
    [SerializeField] public Text currnetRoundNumber;
    // Start is called before the first frame update
    void Start()
    {
        currnetRoundNumber.text = RoundManager.Instance.currentRound.ToString();
    }

    public void RoundText()
    {
        currnetRoundNumber.text = RoundManager.Instance.currentRound.ToString();
    }

}
