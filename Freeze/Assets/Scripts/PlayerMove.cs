using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 10f;
    Rigidbody rb;
    Vector3 latestPos;
    Quaternion latestRot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

}

// Update is called once per frame
void Update()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
            InputMovement();
        else SyncedMovement();
    }

    void InputMovement()
    {
        if (Input.GetKey(KeyCode.W))
            rb.MovePosition(rb.position + Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            rb.MovePosition(rb.position - Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            rb.MovePosition(rb.position + Vector3.right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            rb.MovePosition(rb.position - Vector3.right * speed * Time.deltaTime);
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetComponent<Rigidbody>().position);
        }
        else
        {
            syncEndPosition = (Vector3)stream.ReceiveNext();
            syncStartPosition = GetComponent<Rigidbody>().position;

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
