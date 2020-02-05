using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRequestManager : MonoBehaviour
{
    [SerializeField][Header("Card Prefab")] private GameObject cardPrefGO;
    [SerializeField][Header("Card Pool Size")] private int cardPoolSize;
    
    struct Item{
        public GameObject item;
        public CardManager card;
        public bool active;
        public Item(GameObject _item, bool _active){
            item = _item;
            active = _active;
            card = item.GetComponent<CardManager>();
        }
    }

    private Item[] itemArray;

    public static CardRequestManager instance;

    void Awake(){
        instance = this;
        MakeCards();
    }

    void MakeCards(){
        itemArray = new Item[cardPoolSize];
        for(int i = 0; i < cardPoolSize; i++){
            itemArray[i] = new Item(Instantiate(cardPrefGO), false);
        }
        DisableCards();
    }

    public void DisableCards(){
        for(int i = 0; i < cardPoolSize; i++){
            itemArray[i].item.SetActive(false);
            itemArray[i].active = false;
        }
    }

    public void ReturnCard(GameObject cardToReturn){
        for(int i = 0; i < cardPoolSize; i++){
            if(cardToReturn == itemArray[i].item){
                itemArray[i].active = false;
                itemArray[i].item.SetActive(false);
                return;
            }
        }
    }

    public GameObject RequestCard(){
        for(int i = 0; i < cardPoolSize; i++){
            if(!itemArray[i].active){
                itemArray[i].active = true;
                itemArray[i].item.SetActive(true);
                itemArray[i].card.transform.localScale = Vector3.one;
                return itemArray[i].item;
            }
        }
        return null;
    }

    void DeleteCards(){
        for(int i = 0; i < cardPoolSize; i++){
            Destroy(itemArray[i].item);
        }
    }

    void OnApplicationQuit(){
        DeleteCards();
    }
}
