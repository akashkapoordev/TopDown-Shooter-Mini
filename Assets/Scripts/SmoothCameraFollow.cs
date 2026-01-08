using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private float damping = 0.15f;
    [SerializeField] private Vector3 offset = new Vector3(0,0,-10);
    [SerializeField] private Transform target;
    Vector3 velocity = Vector3.zero;
    private void LateUpdate()
    {
        if (!target) return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
    }
}
