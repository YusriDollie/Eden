using UnityEngine;
using UnityEngine.UI;

public class OptionsControl : MonoBehaviour{
    public GameObject mainCanvas;
    public GameObject optionCanvas;

    public void BackBtn(){
        mainCanvas.SetActive(true);
        this.gameObject.SetActive(false); 
    }

}
