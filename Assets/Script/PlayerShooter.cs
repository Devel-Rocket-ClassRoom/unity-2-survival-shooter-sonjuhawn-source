using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;

    public Transform gunPivot;

    private PlayerInput playerInput;



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    void Update()
    {

        if (playerInput.Fire)
        {
            gun.Fire();
        }
        if (playerInput.Reload)
        {
            gun.Reload();
        }
    }

    
}
