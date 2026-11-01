using UnityEngine;

public class PenaltyTarget : MonoBehaviour
{
    // the laugh sound that mocks the player when this target is hit
    public AudioClip laughSound;
    [Range(0f, 1f)]
    public float laughVolume = 1f;

    // how many points are taken away when the joker is hit
    public int penaltyPoints = 5;

    // called by ShootGun when the raycast hits this target
    public void OnHit(Vector3 pos)
    {
        // play the laugh sound to mock the player
        if (laughSound != null)
            AudioSource.PlayClipAtPoint(laughSound, pos, laughVolume);

        // subtract points — minus sign makes AddScore subtract instead of add
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(-penaltyPoints);

        Debug.Log("Penalty hit! -" + penaltyPoints + " points");
    }
}