using UnityEngine;
using System.Collections;


/*
 * Crossfade can be under 5 seconds
 */

public class AudioManager : MonoBehaviour{

    public int currentTrack = 0;
    public float trackTime = 0.0f;
    public float masterVolume = 1.0f;
    private float timeSinceLastPlay = 0.0f;

    private AudioSource channel1;
    private AudioSource channel2;
    [SerializeField] private AudioClip[] tracks;

    // Use this for initialization
    void Start(){
        channel1 = gameObject.AddComponent<AudioSource>();
        channel2 = gameObject.AddComponent<AudioSource>();

        channel1.loop = true;
        channel2.loop = true;
        channel1.volume = masterVolume;
        channel2.volume = 0.0f;

        if(tracks.Length != 0){
            StartCoroutine(PlayMusic(120.0f));
        }
    }
	
    // Update is called once per frame
    void Update(){
        trackTime = Time.time - timeSinceLastPlay;
    }

    private IEnumerator CrossFade(AudioSource a, AudioSource b, float seconds){
//        Debug.Log("Starting crossfade");

        //calculate duration of each step
        float stepInterval = seconds / 20.0f;
        float volInterval = masterVolume / 20.0f;

        b.Play();

        //fade between tracks lowering a's volume while increasing b's [0, vol] and vice versa
        for(int i = 0; i < 20; i++){
            a.volume -= (volInterval * 2);
            b.volume += volInterval;

            yield return new WaitForSeconds(stepInterval);
        }

        a.Stop();
//        Debug.Log("Finished crossfade");
    }

    private IEnumerator SwitchTrack(int i, float switchDuration){
        bool play_a = true;

        if(channel2.volume == 0.0f){
            play_a = false;
        }

        if(play_a){
            channel1.clip = tracks[i];
            yield return StartCoroutine(CrossFade(channel2, channel1, switchDuration));
        } else{
            channel2.clip = tracks[i];
            yield return StartCoroutine(CrossFade(channel1, channel2, switchDuration));
        }

        Debug.Log("Now Playing: " + tracks[i].name);

    }

    private IEnumerator PlayMusic(float duration){
        while(true){

            timeSinceLastPlay = Time.time;

            StartCoroutine(SwitchTrack(currentTrack, 3.0f));

            //play track for 5 seconds before swapping tracks
            yield return new WaitForSeconds(duration);
        }
    }

    public void PlayTrack(int i){
        //loops through tracks and prevents indexing errors
        if(i > tracks.Length - 1){
            i = 0;
        }
        if(i < 0){
            i = tracks.Length - 1;
        }

        currentTrack = i;

        StartCoroutine(PlayMusic(120.0f));
    }

    public void StopMusic(){
        channel1.Stop();
        channel2.Stop();
    }

}
