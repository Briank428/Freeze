using UnityEngine;

public class AIMove : MonoBehaviour
{
    public float movementSpeed = 10f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(transform.position + new Vector3(0,1,0) * Time.deltaTime * movementSpeed);
    }

    void Turn()
    {
        int dir = (int)Mathf.Pow(-1, Random.Range(0, 100));
        transform.Rotate(0, 0, 90 * dir);
    }
}
