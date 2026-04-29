using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip breathingClip;
    [SerializeField] private AudioSource breathingSource;
    [Space]
    [SerializeField] private float footstepInterval;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource footstepSource;

    private float footstepTimer;
    private bool previousMovingState;

    FirstPersonController fpsController;

    private void Awake()
    {
        fpsController = GetComponent<FirstPersonController>();

        breathingSource.clip = breathingClip;
        breathingSource.Play();
    }

    private void Update()
    {
        if (fpsController.IsMoving && previousMovingState == false)
        {
            footstepSource.PlayOneShot(footstepClip);
            footstepTimer = footstepInterval;
        }

        if (fpsController.IsMoving)
        {
            if (footstepTimer > 0)
                footstepTimer -= Time.deltaTime;
            else
            {
                footstepSource.pitch = Random.Range(0.9f, 1.1f);
                footstepSource.PlayOneShot(footstepClip);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = footstepInterval;
        }

        previousMovingState = fpsController.IsMoving;
    }
}
