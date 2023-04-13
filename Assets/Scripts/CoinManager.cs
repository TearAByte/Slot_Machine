using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour{
    private ReelSpin ReelSpin;

    [SerializeField]
    private TextMeshProUGUI TotalCoins;

    [SerializeField]
    private TextMeshProUGUI TotalBets;

    [SerializeField]
    private TextMeshProUGUI TotalWinnings;

    private int totalCoins, totalBets, totalWinnings, betValue;

    // Start is called before the first frame update
    void Start(){
        ReelSpin = GameObject.FindGameObjectWithTag("ReelManager").GetComponent<ReelSpin>();

        //initializes the variables
        totalCoins = 4000;
        totalBets = 100;
        betValue = totalBets / 20;
        totalWinnings = 0;
    }

    // Update is called once per frame
    void Update(){
        //ensures that the values displayed on screen are updated
        totalBets = betValue * 20;
        TotalCoins.text = totalCoins.ToString();
        TotalBets.text = totalBets.ToString();
        TotalWinnings.text = totalWinnings.ToString();
    }

    public void PayBet(){
        //decreases the coins held with the amount of bets placed and then calls ReelSpin to spin the wheel, does not do anything when they player cannot pay for the spin
        if (totalBets > totalCoins) return;
        totalCoins -= totalBets;
        totalWinnings = 0;
        ReelSpin.TriggerSpin();
    }

    public void IncreaseBet(){
        //increases the total bet by multiples of 20
        if((betValue + 1 )* 20 <= totalCoins){
            betValue++;
            totalBets = betValue * 20;
        }
    }

    public void DecreaseBet(){
        //decreases the total bet by multiples of 20
        if((betValue - 1) * 20 >= 100){
            betValue--;
            totalBets = betValue * 20;
        }
    }

    public void Payout(int payout){
        //called in the results checker to determine the amount of coins won
        totalWinnings = payout * betValue;
        totalCoins += totalWinnings;
        betValue = 4;
    }
}
