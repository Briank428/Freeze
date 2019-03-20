using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 10f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.singlePlayer || photonView.IsMine)
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
        rb.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }

    #region latency vars
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = rb.position;
        }
    }
}
