using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;
using UnityStandardAssets.CrossPlatformInput;

public class SplashControl : MonoBehaviour{

    void Start(){
        StartCoroutine(SplashScreen());
    }

    IEnumerator SplashScreen(){
        yield return new WaitForSeconds(15.0f);
        //load main menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject lobby = GameObject.FindGameObjectWithTag("LobbyManager");
        SceneManager.LoadScene("MainMenu");
        if(lobby){
            lobby.GetComponent<LobbyManager>().StopHostClbk();
            Destroy(lobby);
        }
    }
}
