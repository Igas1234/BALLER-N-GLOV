using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Target Kamera")]
    public Transform cameraTransform;

    [Header("Kekuatan Parallax")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(
            deltaMovement.x * parallaxEffect,
            deltaMovement.y * parallaxEffect,
            0f
        );

        lastCameraPosition = cameraTransform.position;
    }
}