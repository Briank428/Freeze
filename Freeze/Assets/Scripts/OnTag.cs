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
        if (tag == "It") GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<OnTag>() == null) return;
        
        if (tag == "It" && !collision.gameObject.GetComponent<OnTag>().IsFrozen)
        {
            collision.gameObject.GetComponent<OnTag>().IsFrozen = true;
            Debug.Log("Freeze");
        }
        else if (tag != "It" && collision.gameObject.GetComponent<OnTag>().IsFrozen)
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
