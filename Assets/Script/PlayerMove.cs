using UnityEngine;

public class PlayerMove : MonoBehaviour , IDamageable
{
    private PlayerInput input;
    private Rigidbody rb;
    private Camera cam;
    private Animator playerAnimator;
    private Collider playerCollider;
    

    public ParticleSystem damagedParticle;
    public float speed = 5;
    public int health = 100;
    public  bool isDead { get; private set; } = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float moveAmount = new Vector2(input.MoveX, input.MoveZ).magnitude;
        playerAnimator.SetFloat("Move", moveAmount);

        Vector3 direction = new Vector3(input.MoveZ, 0, input.MoveX);
        direction = Vector3.ClampMagnitude(direction, 1f);
        rb.linearVelocity = direction * speed;
      
        LookAtMouse();

        if (isDead)
        {
            return;
        }

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

    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= (int)damage;
        Debug.Log(health);
        damagedParticle.transform.position = hitPoint;
        damagedParticle.transform.forward = hitNormal;
        damagedParticle.Play();
        if (health < 0)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        playerAnimator.SetTrigger("Death");
    }
}
