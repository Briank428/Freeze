using Photon.Pun;
using UnityEngine;

public class OnTag : MonoBehaviourPun
{
    [SerializeField]
    public bool IsFrozen;

    // Start is called before the first frame update
    void Start()
    {
        IsFrozen = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.tag == "Runner")
        {
            if (IsFrozen)
            {
                Debug.Log("Unfreeze");
                IsFrozen = false;
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                if (photonView.IsMine) transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = Color.blue;
            }
        }
        else if(collision.gameObject.tag == "It")
        {
            Debug.Log("Freeze");
            IsFrozen = true;
            transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = Color.cyan;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
