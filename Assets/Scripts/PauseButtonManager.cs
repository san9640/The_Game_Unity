using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonManager : MonoBehaviour
{
    [SerializeField][Header("The Game Manager")] private TheGameManager theGameManager;
    private Animation quitButtonAnim;

    void Awake(){
        quitButtonAnim = this.GetComponent<Animation>();
    }
    
    void OnMouseOver(){
        if(!quitButtonAnim.isPlaying){
            quitButtonAnim.Play();
        }
        if(Input.GetMouseButtonDown(0)){
            theGameManager.QuitGame();
        }
    }
}
