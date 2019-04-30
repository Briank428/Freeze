using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject playerPrefab, aiPrefab;
    public Transform playerSpawn, p2Spawn, p3Spawn, p4Spawn, aiSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tutorial());
    }

    public IEnumerator Tutorial() //use ui instead of debug, will implement later
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("This is the Map. It is randomly generated at the start of every game");

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        GameObject player = Instantiate(playerPrefab) as GameObject;
        player.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        player.GetComponent<PlayerMove>().enabled = false;
        Debug.Log("This is you.");

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        player.transform.position = playerSpawn.position;
        player.GetComponent<PlayerMove>().enabled = true;
        Debug.Log("You can use the arrow keys or WASD to move around the map.");

        yield return new WaitForSeconds(10f);
        player.GetComponent<PlayerMove>().enabled = false;

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        GameObject t1 = Instantiate(playerPrefab) as GameObject;
        t1.GetComponent<PlayerMove>().enabled = false;
        Debug.Log("These are your teammates.");

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        t1.transform.position = p2Spawn.position;
        GameObject t2 = Instantiate(playerPrefab, p3Spawn.transform);
        t2.GetComponent<PlayerMove>().enabled = false;
        GameObject t3 = Instantiate(playerPrefab, p4Spawn.transform);
        t3.GetComponent<PlayerMove>().enabled = false;
        Debug.Log("There can be up to five players to a room.");

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        GameObject ai = Instantiate(aiPrefab);
        ai.GetComponent<AIMove>().enabled = false;
        Debug.Log("This is the ai. He attempts tries to tag you and your friends before time runs out");

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        ai.transform.position = aiSpawn.position;
        yield return new WaitForSeconds(1f);

        Debug.Log("If you are tagged by the ai, you become frozen. You can tag your teammates to unfreeze them");
        while (t2.GetComponent<OnTag>().IsFrozen) ;
        yield return new WaitForSeconds(1f);

        Debug.Log("If the ai can't catch you before time runs out, you win!");

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        yield return new WaitForSeconds(1f);

        yield return null;
        SceneManager.LoadScene("Menu");
    }
}


