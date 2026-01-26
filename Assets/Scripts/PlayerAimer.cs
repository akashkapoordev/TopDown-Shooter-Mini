using UnityEngine;

public class PlayerAimer: MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 aimScreen;
    private float targetAngle;



    public void SetAimScreen(Vector2 screenPos) => aimScreen = screenPos;

    private void Update()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(aimScreen.x, aimScreen.y, 0f));
        Vector2 direction = mouseWorld - transform.position;
        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    }


    private void FixedUpdate()
    {
        rb.MoveRotation(targetAngle);
    }
}