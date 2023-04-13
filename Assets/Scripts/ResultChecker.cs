using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChecker : MonoBehaviour{
    private ReelManager ReelManager;
    private CoinManager CoinManager;
    private int[] reelData1, reelData2, reelData3, reelData4, reelData5;

    //spinResult will contain all the integers in a 5x3 array as a result of then spin
    private int[,] spinResult, reelData;

    private int payout;
    // Start is called before the first frame update
    void Start(){
        CoinManager = GameObject.FindGameObjectWithTag("SlotControl").GetComponent<CoinManager>();
        ReelManager = GetComponent<ReelManager>();
        reelData = new int[5, 10];
        spinResult = new int[5,3];

        //inserts all reelInput data from ReelManager to reelData
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < 10; j++){
                switch (i){
                    case 0:
                        reelData[i,j] = ReelManager.reelInputs1[j];
                        break;
                    case 1:
                        reelData[i, j] = ReelManager.reelInputs2[j];
                        break;
                    case 2:
                        reelData[i, j] = ReelManager.reelInputs3[j];
                        break;
                    case 3:
                        reelData[i, j] = ReelManager.reelInputs4[j];
                        break;
                    case 4:
                        reelData[i, j] = ReelManager.reelInputs5[j];
                        break;
                }
            }
        }

        //remove later, useless now
        reelData1 = ReelManager.reelInputs1;
        reelData2 = ReelManager.reelInputs2;
        reelData3 = ReelManager.reelInputs3;
        reelData4 = ReelManager.reelInputs4;
        reelData5 = ReelManager.reelInputs5;

    }

    // Update is called once per frame
    void Update(){
        
    }

    public void ResultCheck(float[] rowLocation){
        //loops through each of the reels and determines their output by examining the symbols parent location 
        for(int i = 0; i < 5; i++){
            switch (rowLocation[i]){
                case 8.67f:
                case 24.486f:
                    spinResult[i , 0] = reelData[i , 0];
                    spinResult[i, 1] = reelData[i, 1];
                    spinResult[i, 2] = reelData[i, 2];
                    break;
                case 10.26f:
                    spinResult[i, 0] = reelData[i, 1];
                    spinResult[i, 1] = reelData[i, 2];
                    spinResult[i, 2] = reelData[i, 3];
                    break;
                case 11.84f:
                    spinResult[i, 0] = reelData[i, 2];
                    spinResult[i, 1] = reelData[i, 3];
                    spinResult[i, 2] = reelData[i, 4];
                    break;
                case 13.44f:
                    spinResult[i, 0] = reelData[i, 3];
                    spinResult[i, 1] = reelData[i, 4];
                    spinResult[i, 2] = reelData[i, 5];
                    break;
                case 15f:
                    spinResult[i, 0] = reelData[i, 4];
                    spinResult[i, 1] = reelData[i, 5];
                    spinResult[i, 2] = reelData[i, 6];
                    break;
                case 16.58f:
                    spinResult[i, 0] = reelData[i, 5];
                    spinResult[i, 1] = reelData[i, 6];
                    spinResult[i, 2] = reelData[i, 7];
                    break;
                case 18.18f:
                    spinResult[i, 0] = reelData[i, 6];
                    spinResult[i, 1] = reelData[i, 7];
                    spinResult[i, 2] = reelData[i, 8];
                    break;
                case 19.74f:
                    spinResult[i, 0] = reelData[i, 7];
                    spinResult[i, 1] = reelData[i, 8];
                    spinResult[i, 2] = reelData[i, 9];
                    break;
                case 21.36f:
                    spinResult[i, 0] = reelData[i, 8];
                    spinResult[i, 1] = reelData[i, 9];
                    spinResult[i, 2] = reelData[i, 0];
                    break;
                case 22.93f:
                    spinResult[i, 0] = reelData[i, 9];
                    spinResult[i, 1] = reelData[i, 0];
                    spinResult[i, 2] = reelData[i, 1];
                    break;
                default:
                    Debug.Log("Default result hit, not working  " + rowLocation[i]);
                    break;
            }
        }

        LineCheck();
        DiagonalCheck();

        //after checking all the results, the payout gained is sent to CoinManager to determine the amount of coins gained
        CoinManager.Payout(payout);
    }

    private void LineCheck(){
        //checks each row in a straight line from left to right if there are symbols that are similar. If there are no 3 matching symbols in the first 3 reels, the 4th and 5th reels are skipped.
        int timesSeen = 0;
        for (int i = 0; i < 3; i++){
            for(int j = 0; j < 10; j++){
                for(int k = 0; k < 5; k++){
                    if (spinResult[k, i] == j + 1) timesSeen++;
                    if (k >= 2 && timesSeen < 3) break;
                }

                if (timesSeen >= 3) {
                    Debug.Log(j + 1 + " spotted on LineCheck " + timesSeen);
                    //after the line checking is done, if there are 3 matching pairs spotted, the number on the symbols are noted and sent to SymbolCheck
                    SymbolCheck(j + 1, timesSeen);
                }
                timesSeen = 0;
            }
        }
    }

    private void DiagonalCheck(){
        //checks the other lines that are not straight. They also do not check the 4th and 5th reel if the first three do not have matching symbols
        int timesSeen = 0;

        //checks every result if they have a match to the symbols being iterated by the for loop
        for (int i = 0; i < 10; i++){
            if (spinResult[0, 0] == i + 1 && spinResult[1, 1] == i + 1 && spinResult[2, 0] == i + 1) timesSeen = 3;
            if (spinResult[3, 1] == i + 1) timesSeen++;
            if (spinResult[4, 0] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 1,2,1,2,1");
            }
            timesSeen = 0;

            if (spinResult[0, 1] == i + 1 && spinResult[1, 0] == i + 1 && spinResult[2, 1] == i + 1) timesSeen = 3;
            if (spinResult[3, 0] == i + 1) timesSeen++;
            if (spinResult[4, 1] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 2,1,2,1,2");
            }
            timesSeen = 0;

            if (spinResult[0, 1] == i + 1 && spinResult[1, 2] == i + 1 && spinResult[2, 1] == i + 1) timesSeen = 3;
            if (spinResult[3, 2] == i + 1) timesSeen++;
            if (spinResult[4, 1] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 2,3,2,3,2");
            }
            timesSeen = 0;

            if (spinResult[0, 2] == i + 1 && spinResult[1, 1] == i + 1 && spinResult[2, 2] == i + 1) timesSeen = 3;
            if (spinResult[3, 1] == i + 1) timesSeen++;
            if (spinResult[4, 2] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 3,2,3,2,3");
            }
            timesSeen = 0;


            if (spinResult[0, 0] == i + 1 && spinResult[1, 1] == i + 1 && spinResult[2, 2] == i + 1) timesSeen = 3;
            if (spinResult[3, 1] == i + 1) timesSeen++;
            if (spinResult[4, 0] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 1,2,3,2,1");
            }
            timesSeen = 0;

            if (spinResult[0, 2] == i + 1 && spinResult[1, 1] == i + 1 && spinResult[2, 0] == i + 1) timesSeen = 3;
            if (spinResult[3, 1] == i + 1) timesSeen++;
            if (spinResult[4, 2] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 3,2,1,2,3");
            }
            timesSeen = 0;


            if (spinResult[0, 0] == i + 1 && spinResult[1, 2] == i + 1 && spinResult[2, 0] == i + 1) timesSeen = 3;
            if (spinResult[3, 2] == i + 1) timesSeen++;
            if (spinResult[4, 0] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 1,3,1,3,1");
            }
            timesSeen = 0;

            if (spinResult[0, 2] == i + 1 && spinResult[1, 0] == i + 1 && spinResult[2, 2] == i + 1) timesSeen = 3;
            if (spinResult[3, 0] == i + 1) timesSeen++;
            if (spinResult[4, 2] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 3,1,3,1,3");
            }
            timesSeen = 0;


            if (spinResult[0, 0] == i + 1 && spinResult[1, 1] == i + 1 && spinResult[2, 1] == i + 1) timesSeen = 3;
            if (spinResult[3, 1] == i + 1) timesSeen++;
            if (spinResult[4, 0] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 1,2,2,2,1");
            }
            timesSeen = 0;

            if (spinResult[0, 0] == i + 1 && spinResult[1, 2] == i + 1 && spinResult[2, 2] == i + 1) timesSeen = 3;
            if (spinResult[3, 2] == i + 1) timesSeen++;
            if (spinResult[4, 0] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 1,3,3,3,1");
            }
            timesSeen = 0;

            if (spinResult[0, 1] == i + 1 && spinResult[1, 0] == i + 1 && spinResult[2, 0] == i + 1) timesSeen = 3;
            if (spinResult[3, 0] == i + 1) timesSeen++;
            if (spinResult[4, 1] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 2,1,1,1,2");
            }
            timesSeen = 0;

            if (spinResult[0, 1] == i + 1 && spinResult[1, 2] == i + 1 && spinResult[2, 2] == i + 1) timesSeen = 3;
            if (spinResult[3, 2] == i + 1) timesSeen++;
            if (spinResult[4, 1] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 2,3,3,3,2");
            }
            timesSeen = 0;

            if (spinResult[0, 2] == i + 1 && spinResult[1, 0] == i + 1 && spinResult[2, 0] == i + 1) timesSeen = 3;
            if (spinResult[3, 0] == i + 1) timesSeen++;
            if (spinResult[4, 2] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 3,1,1,1,3");
            }
            timesSeen = 0;

            if (spinResult[0, 2] == i + 1 && spinResult[1, 1] == i + 1 && spinResult[2, 1] == i + 1) timesSeen = 3;
            if (spinResult[3, 1] == i + 1) timesSeen++;
            if (spinResult[4, 2] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 3,2,2,2,3");
            }
            timesSeen = 0;


            if (spinResult[0, 0] == i + 1 && spinResult[1, 2] == i + 1 && spinResult[2, 1] == i + 1) timesSeen = 3;
            if (spinResult[3, 2] == i + 1) timesSeen++;
            if (spinResult[4, 0] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 1,3,2,3,1");
            }
            timesSeen = 0;

            if (spinResult[0, 2] == i + 1 && spinResult[1, 0] == i + 1 && spinResult[2, 1] == i + 1) timesSeen = 3;
            if (spinResult[3, 0] == i + 1) timesSeen++;
            if (spinResult[4, 2] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 3,1,2,1,3");
            }
            timesSeen = 0;

            if (spinResult[0, 1] == i + 1 && spinResult[1, 0] == i + 1 && spinResult[2, 2] == i + 1) timesSeen = 3;
            if (spinResult[3, 0] == i + 1) timesSeen++;
            if (spinResult[4, 1] == i + 1) timesSeen++;
            if (timesSeen >= 3) {
                SymbolCheck(i + 1, timesSeen);
                Debug.Log(i + 1 + " spotted on 2,1,3,1,2");
            }
            timesSeen = 0;
        }
    }

    private void SymbolCheck(int symbol, int times){
        //determines the payout based on the symbol with matching pairs, and how many of them there were
        switch (symbol){
            case 1:
                payout += 2 * (times - 2);
                Debug.Log("Payout case 1: " + payout);
                break;
            case 2:
                payout += 3 * (times - 2);
                Debug.Log("Payout case 2: " + payout);
                break;
            case 3:
                payout += 4 * (times - 2);
                Debug.Log("Payout case 3: " + payout);
                break;
            case 4:
                payout += 5 * (times - 2);
                Debug.Log("Payout case 4: " + payout);
                break;
            case 5:
                payout += 6 * (times - 2);
                Debug.Log("Payout case 5: " + payout);
                break;
            case 6:
                payout += 7 * (times - 2);
                Debug.Log("Payout case 6: " + payout);
                break;
            case 7:
                payout += 70 * (times - 2);
                Debug.Log("Payout case 7: " + payout);
                break;
            case 8:
                payout += 9 * (times - 2);
                Debug.Log("Payout case 8: " + payout);
                break;
            case 9:
                payout += 10 * (times - 2);
                Debug.Log("Payout case 9: " + payout);
                break;
            case 10:
                payout += 100 * (times - 2);
                Debug.Log("Payout case 10: " + payout);
                break;
        }
    }
}
