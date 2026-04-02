using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody rb;
    private Camera cam;

    public float speed = 5;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(input.MoveZ, 0, input.MoveX);
        direction = Vector3.ClampMagnitude(direction, 1f);
        rb.linearVelocity = direction * speed;

        LookAtMouse();
    }

    void LookAtMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);

            // y값 고정 (캐릭터가 기울어지지 않게)
            pointToLook.y = transform.position.y;

            transform.LookAt(pointToLook);
        }
    }
}
