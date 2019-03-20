using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region public vars
    public bool singlePlayer;
    public float timeLeft = 60f;
    public GameObject runnerPrefab;
    public GameObject itPrefab;
    public static GameManager Instance;
    #endregion

    #region private vars
    private List<GameObject> runners;
    private GameObject it;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine("StartGame");

    }

    public IEnumerator StartGame()
    {
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 || AllRunnersFrozen())
        {
            EndGame();
        }
        
    }
     bool AllRunnersFrozen()
    {
        foreach (GameObject runner in runners)
        {
            if (runner.GetComponent<OnTag>().IsFrozen) return false;
        }
        return true;
    }

    void EndGame()
    {

    }

}
