using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class ListNetworkPlayers : MonoBehaviour{

    GameObject lobby;
    public LobbyManager netLobby;
    GameObject playerList;
    // Use this for initialization
    void Awake(){
        lobby = GameObject.FindGameObjectWithTag("LobbyManager");
        playerList = GameObject.FindGameObjectWithTag("playerList");
        netLobby = lobby.GetComponent<LobbyManager>();
        if(netLobby != null){
            for(int i = 0; i < netLobby.lobbySlots.Length; i++){
                LobbyPlayer player = netLobby.lobbySlots[i].GetComponent<LobbyPlayer>();
                string playerName = player.GetComponent<InputField>().text;
                playerList.GetComponent<Text>().text += playerName + "\n";
            }
        }
    }
}
