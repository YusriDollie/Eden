using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour{

    private static Dictionary <int, NetworkPlayerController> players = new Dictionary<int, NetworkPlayerController>();

    public static void RegisterPlayer(int netID, NetworkPlayerController player){
        int playerID = netID - 1;
        players.Add(playerID, player);
        player.transform.name = "Player " + playerID;
    }

    public static void DeRegisterPlayer(int playerID){
        players.Remove(playerID);
    }

    public static NetworkPlayerController GetPlayer(int playerID){
        return players[playerID];
    }

    public static Dictionary <int, NetworkPlayerController> GetPlayers(){
        return players;
    }

    void OnGUI(){
        if(Input.GetKey(KeyCode.Tab)){
            if(players.Count == 0){
                return;
            }
            GUILayout.BeginArea(new Rect(20, 240, 200, 500));
            GUILayout.BeginVertical();

            foreach(int playerID in players.Keys){
                GUILayout.Label(players[playerID].transform.name + " - " + players[playerID].transform.gameObject.GetComponent<MHealth>().currentHealth + " - " + players[playerID].transform.gameObject.GetComponent<MFPSCombat>().currentStamina);
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
