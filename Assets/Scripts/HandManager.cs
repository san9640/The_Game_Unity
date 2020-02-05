// using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandManager : MonoBehaviour
{
    private List<CardManager> cardsOnHand;// = new List<CardManager>();
    private int cardCountMax;
    [SerializeField][Header("Deque Manager")] private DequeManager dequeManager;
    [SerializeField][Header("Field Manager")] private FieldManager fieldManager;
    [SerializeField][Header("Left Card Pos")] private Vector3 leftTransform;
    [SerializeField][Header("Right Card Pos")] private Vector3 rightTransform;
    [SerializeField][Header("The Game Manager")] private TheGameManager theGameManager;

    [SerializeField][Header("Placing Sounds")] private AudioSource[] placingAS;
    [SerializeField][Header("Drawing Sounds")] private AudioSource[] drawingAS;

    [HideInInspector] public int cardIndexClicked;

    public int CardCountToDraw{
        get{
            return cardCountMax - cardsOnHand.Count;
        }
    }


    public void SetHand(int maxCard){
        cardCountMax = maxCard;
        //dequeManager.GetCards(cardCountMax);
        cardsOnHand = new List<CardManager>();
        DrawCards();
        // = dequeManager.GetCards(cardCountMax);
        cardIndexClicked = -1;
        // Debug.Log($"{cardsOnHand.Count}");
        //SetCardsPos();
    }

    public void DrawCards(){
        List<CardManager> list = dequeManager.GetCards(CardCountToDraw);
        for(int i = 0; i < list.Count; i++){
            cardsOnHand.Add(list[i]);
        }
        int x = UnityEngine.Random.Range(0, drawingAS.Length);
        drawingAS[x].Play();
        int y = UnityEngine.Random.Range(0, drawingAS.Length);
        if(x == y){ y = (y + 1) % drawingAS.Length; }
        drawingAS[y].Play();
        SetCardsPos();
    }

    public void SetCardsPos(){
        cardsOnHand.Sort(delegate(CardManager c1, CardManager c2){
            return c1.CardNumber.CompareTo(c2.CardNumber);
        });

        for(int i = 0; i < cardsOnHand.Count; i++){
            cardsOnHand[i].canClick = false;
            cardsOnHand[i].Hand = this;

            Vector3 posStart = cardsOnHand[i].transform.position;
            Vector3 posEnd = (rightTransform + leftTransform) / 2;
            if(cardsOnHand.Count > 1){
                posEnd = leftTransform + (rightTransform - leftTransform) / (cardsOnHand.Count - 1) * i;
            }
            
            if(cardsOnHand[i].IsClicked){
                posEnd += Vector3.up;
                cardIndexClicked = i;
            }

            cardsOnHand[i].MoveCard(posStart, posEnd);
        }
    }

    public void SetCardPosOnClick(CardManager card){
        if(!theGameManager.IsGameOver){
            if(cardIndexClicked >= 0){  // 클릭되었던 카드가 있을 때
                cardsOnHand[cardIndexClicked].SetPosOnClick();  // 클릭되었던 카드 원위치

                if(cardsOnHand[cardIndexClicked] != card){  // 현재 클릭한 카드와 같지 않을 때
                    card.SetPosOnClick();   // 현재 클릭한 카드 하이라이트
                    cardIndexClicked = cardsOnHand.IndexOf(card);
                    fieldManager.ShowImpossiblePlace(true, card.CardNumber);
                }else{  // 클릭된 카드를 다시 누른 경우
                    cardIndexClicked = -1;
                    fieldManager.ShowImpossiblePlace(false, 0);
                }
            }else{  // 클릭되었던 카드가 없을 때
                card.SetPosOnClick();   // 현재 클릭한 카드 하이라이트
                cardIndexClicked = cardsOnHand.IndexOf(card);
                fieldManager.ShowImpossiblePlace(true, card.CardNumber);
            }
        }
    }

    public void SetCardPosOnPlacing(Vector3 fieldCardPos, PlacingInfo info, Action<PlacingInfo> callback){
        if(cardIndexClicked >= 0){
            cardsOnHand[cardIndexClicked].MoveCardToField(info, fieldCardPos, callback);
            for(int i = 0, x = UnityEngine.Random.Range(0, placingAS.Length); i < placingAS.Length; i++){
                if(!placingAS[x].isPlaying){
                    placingAS[x].Play();
                    Debug.Log($"{x} 번째 사운드");
                    break;
                }

                x++;
                if(x >= placingAS.Length){
                    x = 0;
                }
                // x %= placingAS.Length;
            }
        }
    }

    public int NumberOfCardClicked(){
        if(cardIndexClicked >= 0){
            if(cardsOnHand[cardIndexClicked].canClick){
                int num = cardsOnHand[cardIndexClicked].CardNumber;
                return num;
            }else{
                return 0;
            }
        }else{
            return 0;
        }
        // return 0;
    }

    public void DeleteCardClicked(){
        if(cardIndexClicked >= 0){
            CardRequestManager.instance.ReturnCard(cardsOnHand[cardIndexClicked].gameObject);
            cardsOnHand.RemoveAt(cardIndexClicked);
            cardIndexClicked = -1;
        }
    }

    public List<int> ListOfCardNumbers(){
        List<int> list = new List<int>();
        foreach(CardManager card in cardsOnHand){
            list.Add(card.CardNumber);
        }
        return list;
    }

}
