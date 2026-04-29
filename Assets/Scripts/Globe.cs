using UnityEngine;

public class Globe : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private AudioClip mergeClip;

    Vector3 prevAnchorPos;

    private void Start()
    {
        prevAnchorPos = anchor.position;
    }

    private void Update()
    {
        Vector3 delta = anchor.position - prevAnchorPos;
        transform.position += new Vector3(-delta.x, delta.y, delta.z);

        prevAnchorPos = anchor.position;
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(mergeClip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Globe"))
            return;

        Vector3 center = transform.position + (anchor.position - transform.position) / 2;
        transform.position = center;
        GetComponent<Animator>().SetTrigger("Merge");
        GetComponent<AudioSource>().PlayOneShot(mergeClip);
        Destroy(anchor.gameObject);
        this.enabled = false;
        GetComponent<Carry>().carrying = true;
    }
}
