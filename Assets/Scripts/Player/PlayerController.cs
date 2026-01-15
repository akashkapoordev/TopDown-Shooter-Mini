using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAimer aimer;
    [SerializeField] private GunController gun;

    private void OnEnable()
    {
        input.FirePressed += gun.OnFirePressed;
        input.FireReleased += gun.OnFireReleased;
    }

    private void OnDisable()
    {
        input.FirePressed -= gun.OnFirePressed;
        input.FireReleased -= gun.OnFireReleased;
    }


    private void Update()
    {
        movement.SetMoveInput(input.Move);
        aimer.SetAimScreen(input.Aim);
    }
}
