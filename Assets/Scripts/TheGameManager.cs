using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TheGameManager : MonoBehaviour
{
    [SerializeField][Header("Player Hands")] private HandManager[] playerHand;
    private int[] maxCardWithPlayer = {0, 8, 7, 6, 6}; 
    public int CurrentMaxCard{
        get{ return maxCardWithPlayer[playerCount]; }
    }
    private int playerCount = 1;
    private int currentPlayingHand;
    [SerializeField][Header("Deque")] private DequeManager dequeManager;
    [SerializeField][Header("Field Manager")] private FieldManager fieldManager;

    [SerializeField][Header("Fade Panel Image")] private Image fadeImage;

    [SerializeField][Header("Win Game Object")] private GameObject winGO;
    [SerializeField][Header("Lose Game Object")] private GameObject loseGO;
    [SerializeField][Header("Use More Cards Animation")] private Animation moreCardAnim;
    [SerializeField][Header("BGM Audio Source")] private AudioSource bgmAudio;

    private IEnumerator fadeOutCoroutine = null;

    private bool isGameOver;
    public bool IsGameOver{
        get{ return isGameOver; }
    }

    public HandManager CurrentPlayingHand{
        get { return playerHand[currentPlayingHand]; }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log("Game Start");
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn(){
        // float maxVolume = 0.15f;
        bgmAudio.volume = 0;
        for(float i = 1; i >= 0.5f; i -= 0.01f){
            fadeImage.color = new Color (0, 0, 0, i);
            yield return new WaitForEndOfFrame();
            bgmAudio.volume += 0.0015f;
        }
        SetGame();
        for(float i = 0.5f; i >= 0; i -= 0.01f){
            fadeImage.color = new Color (0, 0, 0, i);
            yield return new WaitForEndOfFrame();
            bgmAudio.volume += 0.0015f;
        }

    }

    IEnumerator FadeOut(){
        for(float i = 0; i <= 1; i += 0.01f){
            fadeImage.color = new Color (0, 0, 0, i);
            yield return new WaitForEndOfFrame();
            bgmAudio.volume -= 0.0015f; 
        }
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    void Awake(){
        currentPlayingHand = 0;
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        //SetGame();
    }

    // void Update(){
    //     if(Input.GetKey(KeyCode.Escape)){
    //         StartCoroutine(FadeOut());
    //     }
    // }

    public void SetGame(){
        // 게임 세팅 (리셋 가능?인가)
        
        dequeManager.SetDeque();
        for(int i = 0; i < playerCount; i++){
            playerHand[i].SetHand(maxCardWithPlayer[playerCount]);
        }
        isGameOver = false;
    }

    public void QuitGame(){
        if(fadeOutCoroutine == null){
            fadeOutCoroutine = FadeOut();
            StartCoroutine(fadeOutCoroutine);
        }
    }

    public void ChangePlayer(){
        if(CurrentPlayingHand.CardCountToDraw >= 2 ||(CurrentPlayingHand.CardCountToDraw == 1 && dequeManager.NumberOfRemainCards == 0)){
            if(dequeManager.NumberOfRemainCards > 0){
                CurrentPlayingHand.DrawCards();
            }
            currentPlayingHand++;
            if(currentPlayingHand == playerHand.Length){
                currentPlayingHand = 0;
            }
            
            fieldManager.CheckGameOver();
        }
        else{
            moreCardAnim.Play();
        }
    }

    public void ShowGameOver(bool win){
        isGameOver = true;
        if(win){
            StartCoroutine(ShowWin());
        }else{
            StartCoroutine(ShowLose());
        }
    }

    IEnumerator ShowWin(){
        Debug.Log("You Win!");
        winGO.SetActive(true);
        winGO.GetComponent<Animation>().Play();
        yield return null;
    }

    IEnumerator ShowLose(){
        Debug.Log("You Lose!");
        loseGO.SetActive(true);
        loseGO.GetComponent<Animation>().Play();
        yield return null;
    }
}
