using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FieldCardManager : MonoBehaviour
{
    [SerializeField][Header("Card Num1 Sprite Renderer")] private SpriteRenderer numLeftSprite;
    [SerializeField][Header("Card Num2 Sprite Renderer")] private SpriteRenderer numRightSprite;
    [SerializeField][Header("Card Num3 Sprite Renderer")] private SpriteRenderer numCenterSprite;

    [SerializeField][Header("X Sign Game Object")] private GameObject xSignSpriteGO;

    //[SerializeField][Header("Number Sprites")] private Sprite[] numberSprites;
    [SerializeField][Header("Number SpriteAtlas")] private SpriteAtlas numberSpriteAtlas;

    [SerializeField][Header("Field Manager")] private FieldManager fieldManager;
    private int cardNumber;
    public int CardNumber{
        set{
            if(cardNumber == 100){
                numLeftSprite.transform.localPosition += new Vector3(0.26f, 0, 0);
                numRightSprite.transform.localPosition += new Vector3(-0.26f, 0, 0);
            }
            
            cardNumber = value;
            if(cardNumber >= 10){
                numLeftSprite.gameObject.SetActive(true);
                numRightSprite.gameObject.SetActive(true);
                numCenterSprite.gameObject.SetActive(false);
                
                //numLeftSprite.sprite = numberSprites[cardNumber/10];
                numLeftSprite.sprite = numberSpriteAtlas.GetSprite($"m{cardNumber/10}");

                //numRightSprite.sprite = numberSprites[cardNumber%10];
                numRightSprite.sprite = numberSpriteAtlas.GetSprite($"m{cardNumber%10}");
            }else{
                numLeftSprite.gameObject.SetActive(false);
                numRightSprite.gameObject.SetActive(false);
                numCenterSprite.gameObject.SetActive(true);

                // numCenterSprite.sprite = numberSprites[cardNumber];
                numCenterSprite.sprite = numberSpriteAtlas.GetSprite($"m{cardNumber}");
            }
        }
    }

    private bool canPlace = true;
    public bool CanPlace{
        set{
            canPlace = value;
            
            if(canPlace){
                xSignSpriteGO.SetActive(false);
            }else{
                xSignSpriteGO.SetActive(true);
            }
        }
    }

    [SerializeField][Header("isAscending")] private bool isAscending;

    public bool IsAscending{
        get{
            return isAscending;
        }
    }

    void Awake(){
        //fieldManager = this.transform.parent.GetComponent<FieldManager>();
        if(isAscending){
            cardNumber = 1;
        }else{
            cardNumber = 100;
        }
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            fieldManager.OnCardClicked(this);
            // Debug.Log("Field Clicked!");
        }
    }

    public (bool flag, int difference)  CanPlaceNumber(int number){
        if(isAscending){
            if(number > cardNumber || number == cardNumber - 10){
                return (true, number - cardNumber);
            }else{
                return (false, 0);
            }
        }else{
            if(number < cardNumber || number == cardNumber + 10){
                return (true, cardNumber - number);
            }else{
                return (false, 0);
            }
        }
    }
}
