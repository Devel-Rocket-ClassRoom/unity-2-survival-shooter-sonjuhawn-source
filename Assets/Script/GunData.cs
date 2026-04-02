using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public float damage = 25;
    public int startAmmoRemain = 80;
    public int magCapacity = 25;

    public float timeBetfrie = 0.12f;
    public float reloadTime = 1.2f;
}
