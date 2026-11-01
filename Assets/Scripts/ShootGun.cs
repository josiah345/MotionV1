using UnityEngine;

public class ShootGun : MonoBehaviour
{
    // where the bullet comes out of the gun
    [Header("Shooting")]
    public Transform firePos;

    // how far the shot can reach
    public float range = 100f;

    // the visible bullet  purely for looks, not for hit detection
    [Header("Bullet Visual")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    // how long before the bullet disappears
    public float bulletTime = 0.5f;

    // the gunshot sound
    [Header("Audio")]
    public AudioClip gunShotSound;

    // how loud the gun is 0 is silent, 1 is full volume
    [Range(0f, 1f)]
    public float gunShotVolume = 1f;

    // three particle effects that make up the muzzle flash
    [Header("Muzzle Flash")]
    public ParticleSystem muzzleCore;    // bright flash
    public ParticleSystem muzzleSmoke;  // smoke puff
    public ParticleSystem muzzleSparks; // flying sparks

    // stops the gun from firing too fast
    [Header("Fire Cooldown")]
    public float fireCooldown = 0.2f;
    private float lastFireTime = 0f;

    // called by the VR trigger when the player fires
    public void FireBullet()
    {
        // block the shot if not enough time has passed since the last one
        if (Time.time - lastFireTime < fireCooldown) return;
        lastFireTime = Time.time;

        PlayEffects();    // sound and flash
        SpawnBullet();    // visible bullet
        ProcessRaycast(); // hit detection
    }

    // plays the gunshot sound and all three muzzle flash effects
    void PlayEffects()
    {
        if (gunShotSound != null)
            AudioSource.PlayClipAtPoint(gunShotSound, firePos.position, gunShotVolume);

        if (muzzleCore != null) muzzleCore.Play();
        if (muzzleSmoke != null) muzzleSmoke.Play();
        if (muzzleSparks != null) muzzleSparks.Play();
    }

    // creates the visible bullet and launches it forward
    // destroyed automatically after bulletTime seconds
    void SpawnBullet()
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = firePos.forward * bulletSpeed;
        Destroy(bullet, bulletTime);
    }

    // detects what was hit using a sphere shaped invisible line
    // sphere is used instead of a thin line so hits are more reliable in VR
    void ProcessRaycast()
    {
        RaycastHit[] hits = Physics.SphereCastAll(firePos.position, 0.05f, firePos.forward, range);

        // check for the joker penalty target first
        // if hit — deduct points and stop, do not check rings
        foreach (RaycastHit hit in hits)
        {
            PenaltyTarget penalty = hit.collider.GetComponent<PenaltyTarget>();
            if (penalty != null)
            {
                penalty.OnHit(hit.point);
                return;
            }
        }

        // find the ring with the highest points among everything hit
        // this makes sure the bullseye always wins over the outer rings
        RingZone bestRing = null;
        RaycastHit bestHit = new RaycastHit();
        int best = -1;

        foreach (RaycastHit hit in hits)
        {
            RingZone ring = hit.collider.GetComponent<RingZone>();
            if (ring != null && ring.points > best)
            {
                bestRing = ring;
                bestHit = hit;
                best = ring.points;
            }
        }

        // tell the winning ring it was hit
        if (bestRing != null)
            bestRing.OnHit(bestHit.point);
    }
}