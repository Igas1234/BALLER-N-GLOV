using UnityEngine;

public class ShurikenTrap : MonoBehaviour
{
    [Header("Gerakan Kiri Kanan")]
    public float moveSpeed = 3f;
    public float moveDistance = 4f;
    private Vector2 startPosition;
    private bool movingRight = true;

    [Header("Rotasi")]
    public float rotateSpeed = 360f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        RotateShuriken();
        MoveLeftRight();
    }

    void RotateShuriken()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    void MoveLeftRight()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.World);

            if (transform.position.x >= startPosition.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);

            if (transform.position.x <= startPosition.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }
}