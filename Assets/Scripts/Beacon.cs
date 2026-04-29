using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField] private AudioClip beapSound;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayBeap()
    {
        source.PlayOneShot(beapSound);
    }
}
