using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour{
    public GameObject mainCanvas;

    public void SPlayerBtn(){
        SceneManager.LoadScene("FinalPlacements");
    }

    public void MPlayerBtn(){
        SceneManager.LoadScene("GameLobby");
    }

    public void VRBtn(){
        SceneManager.LoadScene("VRScene");
    }

    public void QuitBtn(){
        Application.Quit();
    }
}
