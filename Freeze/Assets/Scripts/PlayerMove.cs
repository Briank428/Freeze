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
        transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
        transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Debug.Log("Is receiving");
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
