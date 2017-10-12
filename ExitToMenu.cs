using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class ExitToMenu : MonoBehaviour{

    // Use this for initialization
    void Start(){
	
    }
	
    // Update is called once per frame
    void Update(){
	
    }

    public void MainMenuBtn(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
