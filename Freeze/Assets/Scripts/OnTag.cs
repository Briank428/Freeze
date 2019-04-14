using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTag : MonoBehaviour
{
    [SerializeField]
    public bool IsFrozen { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        IsFrozen = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (tag == "It" && collision.gameObject.tag == "Runner" && !collision.gameObject.GetComponent<OnTag>().IsFrozen)
        {
            collision.gameObject.GetComponent<OnTag>().IsFrozen = true;
            Debug.Log("Freeze");
        }
        else if (tag == "Runner" && collision.gameObject.tag == "Runner" && collision.gameObject.GetComponent<OnTag>().IsFrozen)
        {
            collision.gameObject.GetComponent<OnTag>().IsFrozen = false;
            Debug.Log("Unfreeze");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
