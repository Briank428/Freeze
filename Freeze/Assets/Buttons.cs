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

    public bool music;
    public bool sound;
    
    void Start()
    {
        lobbyB.onClick.AddListener(Lobby);
        tutorialB.onClick.AddListener(Tutorial);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        music = true;
        sound = true;
    }
    public void Lobby() { SceneManager.LoadScene("Lobby"); }
    public void Tutorial () { SceneManager.LoadScene("Tutorial"); }
    public void ToggleMusic()
    {
        if (music) music = false;
        else music = true;
    }
    public void ToggleSound()
    {
        if (sound) sound = false;
        else sound = true;
    }
    
}
