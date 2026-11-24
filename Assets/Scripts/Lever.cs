using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [Header("Setup")]
    public Transform handle;
    public string requiredTag = "Open";
    public Vector3 pulledRotation = new Vector3(60f, 0f, 0f);
    public float moveTime = 0.2f;

    [Header("Feedback")]
    public AudioSource audioSource;
    public AudioClip pullSound;

    [Header("What it activates")]
    public UnityEvent onPulled;

    bool pulled;

    void OnTriggerEnter(Collider other)
    {
        if (pulled) return;
        if (requiredTag != "" && !other.CompareTag(requiredTag)) return;

        pulled = true;
        StartCoroutine(Rotate());
        if (audioSource && pullSound) audioSource.PlayOneShot(pullSound);
        onPulled.Invoke();
    }

    IEnumerator Rotate()
    {
        Quaternion from = handle.localRotation;
        Quaternion to = from * Quaternion.Euler(pulledRotation);

        for (float t = 0; t < 1f; t += Time.deltaTime / moveTime)
        {
            handle.localRotation = Quaternion.Slerp(from, to, t);
            yield return null;
        }
        handle.localRotation = to;
    }
}