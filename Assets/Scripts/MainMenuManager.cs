using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField][Header("Fade Panel Image")] private Image fadeImage;
    [SerializeField][Header("Fog Transforms")] private Transform[] fogTf;
    [SerializeField][Header("Parchment2 Transform")] private Transform movingObjTf;
    [SerializeField][Header("BGM Audio Source")] private AudioSource bgmAudio;
    private float xPosDif;
    private int currentFogIndex = 0;

    private IEnumerator fadeOutCoroutine = null;
    private IEnumerator showParchmentCoroutine = null;

    void Awake(){
        Application.targetFrameRate = 60;
    }

    void Start(){
        //textAnim.Play();
        //StartCoroutine(PlayTextAnim());
        xPosDif = fogTf[1].position.x - fogTf[0].position.x;
        StartCoroutine(Fog1Move());
    }

    void FixedUpdate(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    // IEnumerator PlayTextAnim(){
    //     while(true){
    //         if(!textAnim.isPlaying){
    //             Debug.Log("Animation Play");
    //             textAnim.Play();
    //         }
    //         yield return new WaitForSeconds(0.5f);
    //     }
    // }

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // void OnMouseDown(){
    //     if(fadeOutCoroutine == null){
    //         Debug.Log("Play");
    //         fadeOutCoroutine = FadeOut();
    //         StartCoroutine(fadeOutCoroutine);
    //     }
    // }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            if(fadeOutCoroutine == null){
                Debug.Log("Play");
                fadeOutCoroutine = FadeOut();
                StartCoroutine(fadeOutCoroutine);
            }

            // if(showParchmentCoroutine == null){
            //     StartCoroutine(showParchmentCoroutine = ShowParchment());
            // }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log("Game Start");
        StartCoroutine(FadeIn());
    }

    IEnumerator Fog1Move(){
        while(true){
            fogTf[0].position += Vector3.left * 0.03f;
            fogTf[1].position += Vector3.left * 0.03f;
            if(fogTf[1 - currentFogIndex].position.x <= 0){
                fogTf[currentFogIndex].position += Vector3.right * xPosDif;
                currentFogIndex = 1 - currentFogIndex;
            }else{
                yield return new WaitForEndOfFrame();   // 이렇게 안하면 프레임 중간에 안개의 위치 변경이 보이는듯함
            }
        }
    }

    IEnumerator FadeOut(){
        for(float i = 0; i <= 1; i += 0.01f){
            fadeImage.color = new Color (0, 0, 0, i);
            bgmAudio.volume -= 0.003f;
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(Fog1Move());
        StopCoroutine(ShowParchment());
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    IEnumerator FadeIn(){
        bgmAudio.volume = 0;
        bgmAudio.Play();
        for(float i = 1; i >= 0; i -= 0.01f){
            fadeImage.color = new Color (0, 0, 0, i);
            bgmAudio.volume += 0.003f;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShowParchment(){
        //movingObjTf.gameObject.GetComponent<Animation>().Stop();
        for(int i = 0; i < 3; i++){
            movingObjTf.GetChild(i).GetComponent<Animation>().Stop();
        }

        Vector3 posStart = movingObjTf.position;
        Vector3 posEnd = new Vector3(0, 0, 0);
        float scaleSize = 0.001f;
        Vector3 scale = new Vector3(1, 1, 0);
        int x = 1;

        for(float i = 0; i <= 1.0f; i += 0.007f, x++){
            movingObjTf.position = Vector3.Lerp(posStart, posEnd, i);
            if(i > 0.3f){
                if(fadeOutCoroutine == null){
                    fadeOutCoroutine = FadeOut();
                    StartCoroutine(fadeOutCoroutine);
                }
            }
            movingObjTf.localScale += scaleSize * scale * x;
            yield return new WaitForEndOfFrame();
        }

        while(true){
            movingObjTf.localScale += scaleSize * scale * x;
            x++;
            yield return new WaitForEndOfFrame();
        }

        // for(float i = 0.5f; i <= 1; i += 0.001f){
        //     parchmentTf.position = Vector3.Lerp(posStart, Vector3.zero, i);
        //     parchmentTf.localScale += scale;
        //     yield return new WaitForEndOfFrame();
        // }
    }

}
