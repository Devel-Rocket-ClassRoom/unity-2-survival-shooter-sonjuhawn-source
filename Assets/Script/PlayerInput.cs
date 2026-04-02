using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string horizontal = "Horizontal";
    public static readonly string vertical = "Vertical";
    public static readonly string fire = "Fire1";
    public static readonly string reload = "Reload";

    public float MoveX {  get; private set; }
    public float MoveZ { get; private set; }
    public bool Fire {  get; private set; }
    public bool Reload {  get; private set; }

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxis(vertical);
        MoveZ = Input.GetAxis(horizontal);
        Fire = Input.GetButton(fire);
        Reload = Input.GetButton(reload);
    }
}
