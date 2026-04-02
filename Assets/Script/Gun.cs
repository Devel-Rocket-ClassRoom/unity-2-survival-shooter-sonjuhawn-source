using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public enum States
    {
        Empty,
        Ready,
        Reloading
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
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineEffect = GetComponent<LineRenderer>();

        bulletLineEffect.positionCount = 2;
        bulletLineEffect.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;

        State = States.Ready;
        lastFireTime = 0f;
    }

    public void Fire()
    {
        if (State == States.Ready && Time.time > lastFireTime + gunData.timeBetfrie)
        {
            lastFireTime = Time.time;

            Shot();
        }
    }

    private void Shot()
    {
        Vector3 hitPosition = Vector3.zero;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance))
        {
            hitPosition = hit.point;

            Debug.Log("Hit: " + hit.collider.name);
            var target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        if (coShot != null)
        {
            StopCoroutine(coShot);
            coShot = null;
        }
        coShot = StartCoroutine(CoShotEffect(hitPosition));

        magAmmo--;
        Debug.Log(magAmmo);
        if (magAmmo < 0)
        {
            State = States.Empty;
        }
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        muzzleEffect.Play();

        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineEffect.SetPosition(0, fireTransform.position);
        bulletLineEffect.SetPosition(1, hitPosition);
        bulletLineEffect.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineEffect.enabled = false;
    }

    public bool Reload()
    {
        if (State == States.Reloading || magAmmo == 0 || magAmmo == gunData.magCapacity)
        {
            return false;
        }
        else
        {
            coReload = StartCoroutine(CoReload());
            return true;
        }


    }
    private IEnumerator CoReload()
    {
        State = States.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
        yield return new WaitForSeconds(gunData.reloadTime);
        int ammoToFill = gunData.magCapacity - magAmmo;
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;
        Debug.Log(magAmmo);
        State = States.Ready;
    }
}
