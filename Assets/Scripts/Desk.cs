using UnityEngine;

public class Desk : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Globe"))
        {
            other.GetComponent<Carry>().enabled = false;
            other.GetComponent<Interactable>().enabled = false;
            other.transform.position = transform.position + Vector3.up * 1.7f;
            other.GetComponent<Animator>().SetTrigger("Merge");
            other.GetComponent<Globe>().PlaySound();
            door.SetOpen(true);
        }
    }
}
