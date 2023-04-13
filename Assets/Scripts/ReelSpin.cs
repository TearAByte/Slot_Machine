using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelSpin : MonoBehaviour{
    private ReelManager ReelManager;
    private ResultChecker ResultChecker;
    private GameObject[] Reel1, Reel2, Reel3, Reel4, Reel5;

    //contains the parent GameObject of the Symbols from each Reel
    private Transform Reel1Parent, Reel2Parent, Reel3Parent, Reel4Parent, Reel5Parent;

    //used for determining slot spin speed
    public int spinSpeed;
    private float spinAcceleration1, spinAcceleration2, spinAcceleration3, spinAcceleration4, spinAcceleration5;
    private bool isSpinning;

    //Y coordinates shared by all reels that displays 3 symbols at once,    rowlocation will contain the y coordinates of all reels when they stop spinning
    private float[] reelStops = { 8.67f, 10.26f, 11.84f, 13.44f, 15f, 16.58f, 18.18f, 19.74f, 21.36f, 22.93f, 24.486f }, rowLocation;
    
    // Start is called before the first frame update
    void Start(){
        ReelManager = GetComponent<ReelManager>();
        ResultChecker = GetComponent<ResultChecker>();
        rowLocation = new float[5];
        
        Reel1 = ReelManager.Reel1;
        Reel2 = ReelManager.Reel2;
        Reel3 = ReelManager.Reel3;
        Reel4 = ReelManager.Reel4;
        Reel5 = ReelManager.Reel5;

        Reel1Parent = Reel1[0].transform.parent;
        Reel2Parent = Reel2[0].transform.parent;
        Reel3Parent = Reel3[0].transform.parent;
        Reel4Parent = Reel4[0].transform.parent;
        Reel5Parent = Reel5[0].transform.parent;

        spinAcceleration1 = 0f;
        spinAcceleration2 = 0f;
        spinAcceleration3 = 0f;
        spinAcceleration4 = 0f;
        spinAcceleration5 = 0f;

        isSpinning = false;
    }

    // Update is called once per frame
    void Update(){
        if (isSpinning){
            SpinSlots(Reel1Parent, spinAcceleration1);
            SpinSlots(Reel2Parent, spinAcceleration2);
            SpinSlots(Reel3Parent, spinAcceleration3);
            SpinSlots(Reel4Parent, spinAcceleration4);
            SpinSlots(Reel5Parent, spinAcceleration5);
        }
    }

    private void SpinSlots(Transform ReelParent, float acceleration){
        //move the symbols parent from the reel down
        float currSpinSpeed = spinSpeed * Time.deltaTime * acceleration;
        ReelParent.Translate(Vector3.down * currSpinSpeed);

        //move the symbols parent up after reaching a certain height
        if (ReelParent.position.y <= 8.67f){
            Vector3 newPosition = ReelParent.position;
            newPosition.y = 24.486f;
            ReelParent.position = newPosition;
        }
    }

    public void TriggerSpin(){
        //initiates or stops the spin of the slots
        isSpinning = !isSpinning;

        if (isSpinning){
            //randomizes the accleration of each reels to better randomize the outcome
            spinAcceleration1 = Random.Range(1.5f, 3.0f);
            spinAcceleration2 = Random.Range(1.5f, 3.0f);
            spinAcceleration3 = Random.Range(1.5f, 3.0f);
            spinAcceleration4 = Random.Range(1.5f, 3.0f);
            spinAcceleration5 = Random.Range(1.5f, 3.0f);
        }
        else{
            //instantly halts the slots when pressed stop
            spinAcceleration1 = 0f;
            spinAcceleration2 = 0f;
            spinAcceleration3 = 0f;
            spinAcceleration4 = 0f;
            spinAcceleration5 = 0f;

            AlignRows(Reel1Parent, 0);
            AlignRows(Reel2Parent, 1);
            AlignRows(Reel3Parent, 2);
            AlignRows(Reel4Parent, 3);
            AlignRows(Reel5Parent, 4);

            //calls ResultCheck to send the rowLocation info and determine the results of the spin
            ResultChecker.ResultCheck(rowLocation);
        }
    }

    private void AlignRows(Transform Reel, int index){
        //aligns the symbols parent of each reel, positioning them to the nearest Y coordinate while rowLocation notes the Y coordinate of the parent
        for(int i = 0; i < 11; i++){
            if (Vector3.Distance(Reel.position, new Vector3(Reel.position.x, reelStops[i], Reel.position.z)) < 0.9f){
                Reel.position = Vector3.MoveTowards(Reel.position, new Vector3(Reel.position.x, reelStops[i], Reel.position.z), 2f);
                rowLocation[index] = reelStops[i];
                break;
            }
        }
    }
}
