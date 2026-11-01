using UnityEngine;
using System.Collections;

public class RingZone : MonoBehaviour
{
    // this ring's own GameObject
    public GameObject thisRing;

    // the whole target — all rings together as one group
    public GameObject targetGroup;

    // how many points this ring is worth
    public int points = 0;

    // sound that plays when this ring is hit
    public AudioClip hitSound;

    // how loud the hit sound is
    [Range(0f, 5f)]
    public float hitVolume = 1f;

    // how long the target stays hidden after being hit
    public float disappearTime = 7f;

    // saves where the target started so it can return to the same spot
    private Vector3 originalPosition;

    // saves the target position before anything moves
    void Start()
    {
        if (targetGroup != null)
            originalPosition = targetGroup.transform.position;
    }

    // called by ShootGun when this ring is confirmed as the best hit
    public void OnHit(Vector3 pos)
    {
        // play this ring's unique sound at the hit position
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, pos, hitVolume);

        // add this ring's points to the score
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(points);

        // hide the target and bring it back after disappearTime seconds
        // coroutine runs on ScoreManager because when the target is disabled
        // this script also gets disabled which would stop the coroutine mid wait
        if (targetGroup != null)
            ScoreManager.instance.StartCoroutine(HideAndReturn());
    }

    IEnumerator HideAndReturn()
    {
        // hide the entire target immediately
        targetGroup.SetActive(false);

        // wait disappearTime seconds before doing anything else
        yield return new WaitForSeconds(disappearTime);

        // move back to the saved original position before showing it again
        targetGroup.transform.position = originalPosition;

        // bring the target back
        targetGroup.SetActive(true);
    }
}