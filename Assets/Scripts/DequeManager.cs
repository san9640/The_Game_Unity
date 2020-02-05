using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DequeManager : MonoBehaviour
{
    // private List<int> cardNumberRemain;
    public static int MAX_COUNT = 98;
    private bool[] isNumberRemain;// = new bool[100];
    [SerializeField][Header("Left Number Sprite Renderer")] private SpriteRenderer leftNumberRenderer;
    [SerializeField][Header("Right Number Sprite Renderer")] private SpriteRenderer rightNumberRenderer;
    //[SerializeField][Header("Number Sprites")] private Sprite[] numberSprites;
    [SerializeField][Header("Number SpriteAtlas")] private SpriteAtlas numberSpriteAtlas;
    [SerializeField][Header("Deque Sprite Game Object")] private GameObject dequeSpriteGO;

    private int numberOfCardsInDeque;
    private int NumberOfCardsInDeque{
        get{ return numberOfCardsInDeque; }
        set{
            numberOfCardsInDeque = value;
            if(numberOfCardsInDeque == 0){
                dequeSpriteGO.SetActive(false);
            }

            leftNumberRenderer.sprite = numberSpriteAtlas.GetSprite($"m{numberOfCardsInDeque/10}");
            rightNumberRenderer.sprite = numberSpriteAtlas.GetSprite($"m{numberOfCardsInDeque%10}");
        }
    }

    public int NumberOfRemainCards{
        get { return numberOfCardsInDeque; }
    }

    [SerializeField][Header("The Game Manager")]private TheGameManager theGameManager;

    // Start is called before the first frame update
    // void Awake(){
    //     //theGameManager = this.transform.parent.GetComponent<TheGameManager>();
    // }

    public void SetDeque(){
        NumberOfCardsInDeque = MAX_COUNT;
        isNumberRemain = Enumerable.Repeat(true, MAX_COUNT + 1).ToArray();
    }

    public List<CardManager> GetCards(int count){
        List<CardManager> list = new List<CardManager>();
        for(int i = 0; i < count && NumberOfCardsInDeque > 0; i++){
            while(true){
                int x = Random.Range(1, MAX_COUNT + 1);
                if(isNumberRemain[x]){
                    isNumberRemain[x] = false;
                    CardManager card = CardRequestManager.instance.RequestCard().GetComponent<CardManager>();
                    card.CardNumber = x + 1;
                    card.transform.position = this.transform.position;
                    list.Add(card);
                    break;
                }
            }
            
            NumberOfCardsInDeque--;
        }
        return list;
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            //Debug.Log("Deque Clicked");
            if(!theGameManager.IsGameOver){
                theGameManager.ChangePlayer();
            }
        }
    }

    // void OnMouseDown(){
    //     theGameManager.ChangePlayer();
    // }
}
