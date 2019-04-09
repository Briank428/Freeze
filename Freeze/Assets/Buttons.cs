using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public Button lobbyB;
    public Button tutorialB;
    public Button soundB;
    public Button musicB;
    public Button exitB;

    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite musicOn;
    public Sprite musicOff;

    public bool music;
    public bool sound;

    void Update() { if (Input.GetKey("escape")) Application.Quit(); }
    void Start()
    {
        lobbyB.onClick.AddListener(Lobby);
        tutorialB.onClick.AddListener(Tutorial);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        exitB.onClick.AddListener(Application.Quit);
        music = true;
        sound = true;
    }
    public void Lobby() { SceneManager.LoadScene("Lobby"); }
    public void Tutorial () { SceneManager.LoadScene("Tutorial"); }
    public void ToggleMusic()
    {
        if (music) { music = false; musicB.image.sprite = musicOff; }
        else { music = true; musicB.image.sprite = musicOn; }
        Debug.Log("Music: " + music);
    }
    public void ToggleSound()
    {
        if (sound) { sound = false; soundB.image.sprite = soundOff; }
        else { sound = true; soundB.image.sprite = soundOn; }
        Debug.Log("Sound: " + sound);
    }
    
}
