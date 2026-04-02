using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum States
    {
        Empty,
        Ready,
        Reload
    }
    public States State { get; private set; }

    public Transform fireTransform;

    public ParticleSystem muzzleEffect;

    private LineRenderer bulletLineEffect;
    private AudioSource gunAudioPlayer;

    public GunData gunData;

    private float fireDistance = 50f;

    public int ammoRemain = 100;
    public int magAmmo;

    private float lastFireTime;

    private Coroutine coShot;
    private Coroutine coReload;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
