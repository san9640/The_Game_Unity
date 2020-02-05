using System.Net.WebSockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

public class CardManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField][Header("Card Num1 Sprite Renderer")] private SpriteRenderer numLeftSprite;
    [SerializeField][Header("Card Num3 Sprite Renderer")] private SpriteRenderer numCenterSprite;
    [SerializeField][Header("Card Num2 Sprite Renderer")] private SpriteRenderer numRightSprite;

    //[SerializeField][Header("Number Sprites")] private Sprite[] numberSprites;
    [SerializeField][Header("Number SpriteAtlas")] private SpriteAtlas numberSpriteAtlas;

    [SerializeField][Header("Field Card World Scale")] private Vector3 fieldCardScale;
    private bool isClicked = false;
    public bool canClick = true;

    public bool IsClicked{
        get { return isClicked; }
    }

    private int cardNumber;

    public int CardNumber{
        get{
            return cardNumber;
        }set{
            cardNumber = value;
            if(cardNumber >= 10){
                numLeftSprite.gameObject.SetActive(true);
                numRightSprite.gameObject.SetActive(true);
                numCenterSprite.gameObject.SetActive(false);
                
                numLeftSprite.sprite = numberSpriteAtlas.GetSprite($"m{cardNumber/10}");
                numRightSprite.sprite = numberSpriteAtlas.GetSprite($"m{cardNumber%10}");
            }else{
                numLeftSprite.gameObject.SetActive(false);
                numRightSprite.gameObject.SetActive(false);
                numCenterSprite.gameObject.SetActive(true);

                numCenterSprite.sprite = numberSpriteAtlas.GetSprite($"m{cardNumber}");;
            }
        }
    }

    [HideInInspector] private HandManager handManager;
    public HandManager Hand{
        set{
            handManager = value;
        }
    }

    void Awake(){
        CardNumber = 0;
    }

    void OnEnable(){
        isClicked = false;
        canClick = true;
    }

    public void MoveCard(Vector3 posStart, Vector3 posEnd){
        StartCoroutine(CardAnim(posStart, posEnd, 0.1f));
    }

    public void MoveCardToField(PlacingInfo info, Vector3 posEnd, Action<PlacingInfo> callback){
        StartCoroutine(PlacingCardAnim(info, posEnd, callback));
    }

    IEnumerator CardAnim(Vector3 posStart, Vector3 posEnd, float speed){
        for(float i = 0.0f; i <= 1.0f; i += speed){
            this.transform.position = Vector3.Lerp(posStart, posEnd, i);
            yield return new WaitForEndOfFrame();
        }
        this.transform.position = posEnd;
        canClick = true;
        //yield return null;
    }

    IEnumerator PlacingCardAnim(PlacingInfo info, Vector3 posEnd,Action<PlacingInfo> callback){
        Vector3 posStart = this.transform.position;
        Vector3 scaleStart = this.transform.localScale;
        
        for(float i = 0.0f; i <= 1.0f; i += 0.1f){
            this.transform.position = Vector3.Lerp(posStart, posEnd, i);
            this.transform.localScale = Vector3.Lerp(scaleStart, fieldCardScale, i);
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = posEnd;
        canClick = true;
        callback(info);
    }

    // void OnMouseDown(){
    //     // Debug.Log($"{CardNumber} Clicked!");

    //     //SetPosOnClick();
    //     if(handManager != null && canClick){
    //         handManager.SetCardPosOnClick(this);
    //     }
    // }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            if(handManager != null && canClick){
                handManager.SetCardPosOnClick(this);
            }
        }
    }

    public void SetPosOnClick(){
        if(isClicked){
            isClicked = false;
            StartCoroutine(MoveOnClick(this.transform.position, this.transform.position + Vector3.down, isClicked));
            //this.transform.position += Vector3.down;
        }else{
            isClicked = true;
            //canClick = false;
            StartCoroutine(MoveOnClick(this.transform.position, this.transform.position + Vector3.up, isClicked));
            // this.transform.position += Vector3.up;

        }
    }

    IEnumerator MoveOnClick(Vector3 posStart, Vector3 posEnd, bool clickFlag){
        canClick = false;
        yield return CardAnim(posStart, posEnd, 0.2f);
        // isClicked = clickFlag;
    }

}
