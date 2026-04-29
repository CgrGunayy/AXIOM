using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool initialOpen;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    private bool isOpen = false;

    Animator anim;
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        anim.SetBool("isOpen", initialOpen);
        isOpen = initialOpen;
        anim.SetTrigger("Start");
    }

    public void SetOpen(bool isOpen)
    {
        if (isOpen)
        {
            anim.SetBool("isOpen", true);
            isOpen = true;
            source.PlayOneShot(openClip);
        }
        else
        {
            anim.SetBool("isOpen", false);
            isOpen = false;
            source.PlayOneShot(closeClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            anim.SetBool("isOpen", true);
            isOpen = true;
            source.PlayOneShot(openClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            anim.SetBool("isOpen", false);
            isOpen = false;
            source.PlayOneShot(closeClip);
        }
    }
}
