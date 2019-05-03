using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image panel;
    public Text top;
    public Text center;
    public Text bottom;

    public GameObject playerPrefab, aiPrefab;
    public Transform playerSpawn, p2Spawn, p3Spawn, p4Spawn, aiSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tutorial());
        panel.gameObject.SetActive(false);
        center.gameObject.SetActive(false);
    }

    public IEnumerator Tutorial() //use ui instead of debug, will implement later
    {
        yield return new WaitForSeconds(0.5f);

        panel.gameObject.SetActive(true);
        top.text = "This is the Map.\nIt is randomly generated at the start of every game";
        bottom.text = "Press Space To Continue";
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        top.text = "";
        bottom.text = "";
        yield return new WaitForSeconds(1f);

        GameObject player = Instantiate(playerPrefab) as GameObject;
        player.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        player.GetComponent<PlayerMove>().enabled = false;
        top.text = "This is you.";
        bottom.text = "Press Space To Continue";
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        top.text = "";
        bottom.text = "";
        yield return new WaitForSeconds(1f);

        player.transform.position = playerSpawn.position;
        player.GetComponent<PlayerMove>().enabled = true;
        top.gameObject.SetActive(false);
        bottom.gameObject.SetActive(false);
        center.gameObject.SetActive(true);
        center.text = "You can use the arrow keys or WASD to move around the map.";

        yield return new WaitForSeconds(10f);
        player.GetComponent<PlayerMove>().enabled = false;
        center.text = "Press Space To Continue";
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        center.text = "";
        yield return new WaitForSeconds(1f);
        GameObject t1 = Instantiate(playerPrefab) as GameObject;
        t1.GetComponent<PlayerMove>().enabled = false;
        center.gameObject.SetActive(false);
        top.gameObject.SetActive(true);
        bottom.gameObject.SetActive(true);
        top.text = "These are your teammates.";
        bottom.text = "Press Space To Continue";

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        top.text = "";
        bottom.text = "";
        yield return new WaitForSeconds(1f);

        t1.transform.position = p2Spawn.position;
        GameObject t2 = Instantiate(playerPrefab, p3Spawn.transform);
        t2.GetComponent<PlayerMove>().enabled = false;
        GameObject t3 = Instantiate(playerPrefab, p4Spawn.transform);
        t3.GetComponent<PlayerMove>().enabled = false;
        top.text = "There can be up to five players to a room.";
        bottom.text = "Press Space To Continue";
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        top.text = "";
        bottom.text = "";
        yield return new WaitForSeconds(1f);

        GameObject ai = Instantiate(aiPrefab);
 
        top.text = "This is IT. He attempts tries to tag you and your friends before time runs out";
        bottom.text = "Press Space To Continue";

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        top.gameObject.SetActive(false); bottom.gameObject.SetActive(false); center.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        ai.transform.position = aiSpawn.position;
        yield return new WaitForSeconds(1f);

        center.text = "If you are tagged by IT, you become frozen. You can tag your teammates to unfreeze them";

        while (t2.GetComponent<OnTag>().IsFrozen) ;
        yield return new WaitForSeconds(2f);
        center.gameObject.SetActive(false); top.gameObject.SetActive(true); bottom.gameObject.SetActive(true);

        top.text = "If IT can't catch you before time runs out, you win!";
        bottom.text = "Press Space To Continue";
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        panel.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        yield return null;
        SceneManager.LoadScene("Menu");
    }
}


