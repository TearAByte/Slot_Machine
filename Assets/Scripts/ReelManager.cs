using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelManager : MonoBehaviour{
    //an array of sprites for all the symbols to be inserted in the reels
    public Sprite[] Symbols;
    //contains the GameObjects for all the symbols they hold
    public GameObject[] Reel1, Reel2, Reel3, Reel4, Reel5;
    //contains the integer information displayed on the sprites for each reel
    public int[] reelInputs1, reelInputs2, reelInputs3, reelInputs4, reelInputs5;
    // Start is called before the first frame update
    void Start(){
        InsertReelInput(reelInputs1, Reel1);
        InsertReelInput(reelInputs2, Reel2);
        InsertReelInput(reelInputs3, Reel3);
        InsertReelInput(reelInputs4, Reel4);
        InsertReelInput(reelInputs5, Reel5);
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void InsertReelInput(int[] reelInput, GameObject[] Reel){
        //Inserts the sprite from Symbols to each of the Reels based on the reelInput integers
        for(int i = 0; i < reelInput.Length; i++){
            Reel[i].GetComponent<SpriteRenderer>().sprite = Symbols[reelInput[i]-1];
        }

        //extra symbols that copy the initial 3 symbols of each reel to be used for looping
        Reel[10] = Reel[0];
        Reel[11] = Reel[1];
        Reel[12] = Reel[2];
    }
}
