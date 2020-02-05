using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlacingInfo{
    public int fieldCardIndex;
    public int cardNum;
    public int dif;
    public PlacingInfo(int _fieldCardIndex, int _cardNum, int _dif){
        fieldCardIndex = _fieldCardIndex;
        cardNum = _cardNum;
        dif = _dif;
    }
}

public class FieldManager : MonoBehaviour
{
    [SerializeField][Header("Field Cards")] private FieldCardManager[] fieldCards;
    [SerializeField][Header("Field Arrow Animations")] private Animation[] arrowAnims;
    [SerializeField][Header("Field Arrow Special Animations")] private Animation[] arrowSpecialAnims;
    [SerializeField][Header("Deque Manager")] private DequeManager dequeManager;

    [SerializeField][Header("The Game Manager")] private TheGameManager theGameManager;
    //private int placedCardCount;

    void Awake(){
        // theGameManager = this.transform.parent.GetComponent<TheGameManager>();
        //placedCardCount = 0;
    }

    public void OnCardClicked(FieldCardManager fieldCard){
        if(!theGameManager.IsGameOver){
            int num = theGameManager.CurrentPlayingHand.NumberOfCardClicked();
            if(num > 1 && num < 100){
                for(int i = 0; i < fieldCards.Length; i++){
                    if(fieldCard == fieldCards[i]){
                        // Debug.Log($"To Place {num}");
                        (bool flag, int x) = fieldCard.CanPlaceNumber(num);
                        if(flag){
                            theGameManager.CurrentPlayingHand.SetCardPosOnPlacing(fieldCard.transform.position, new PlacingInfo(i, num, x), AfterPlacing);
                        }
                        
                        return;
                    }
                }
            }
        }
    }

    private void AfterPlacing(PlacingInfo info){
        fieldCards[info.fieldCardIndex].CardNumber = info.cardNum;
        theGameManager.CurrentPlayingHand.DeleteCardClicked();
        theGameManager.CurrentPlayingHand.SetCardsPos();
        //placedCardCount++;

        if(info.dif > 0){
            arrowAnims[info.fieldCardIndex].Play();
        }else{
            arrowSpecialAnims[info.fieldCardIndex].Play();
        }

        CheckGameOver();

        ShowImpossiblePlace(false, 0);
    }
    
    // void AllCardPlaced(){
    //     Debug.Log("YOU WIN!");
    // }

    public void ShowImpossiblePlace(bool flag, int number){
        if(flag){
            // int num = theGameManager.CurrentPlayingHand.NumberOfCardClicked();
            // Debug.Log(num);
            if(number > 1 && number < 100){
                for(int i = 0; i< fieldCards.Length; i++){
                    //Debug.Log($"{i} : {fieldCards[i].CanPlaceNumber(num)} to {num}");
                    (bool f, int x) = fieldCards[i].CanPlaceNumber(number);
                    if(!f){
                        fieldCards[i].CanPlace = false;
                    }else{
                        fieldCards[i].CanPlace = true;
                    }
                }
            }
        }else{
            for(int i = 0; i< fieldCards.Length; i++){
                fieldCards[i].CanPlace = true;
            }
        }
    }

    public void CheckGameOver(){
        List<int> numberList = theGameManager.CurrentPlayingHand.ListOfCardNumbers();
        if(numberList.Count == 0 && dequeManager.NumberOfRemainCards == 0){
            // Game Win
            theGameManager.ShowGameOver(true);
            return;
        }

        foreach(int x in numberList){
            for(int i = 0; i < fieldCards.Length; i++){
                if(fieldCards[i].CanPlaceNumber(x).flag){
                    return;
                }
            }
        }

        if(dequeManager.NumberOfRemainCards == 0 ){
            theGameManager.ShowGameOver(false);
        }else if(theGameManager.CurrentPlayingHand.CardCountToDraw < 2){
            theGameManager.ShowGameOver(false);
            // GameOver
        }
    }
}
