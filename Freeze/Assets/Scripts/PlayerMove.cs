using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 10f;
    Rigidbody2D rb;
    Vector2 latestPos;
    Quaternion latestRot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
            MoveTo(Vector2.up);
        if (Input.GetKey(KeyCode.S))
            MoveTo(Vector2.down);
        if (Input.GetKey(KeyCode.D))
            MoveTo(Vector2.right);
        if (Input.GetKey(KeyCode.A))
            MoveTo(Vector2.left);
    }

    void MoveTo(Vector2 dir)
    {
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
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
