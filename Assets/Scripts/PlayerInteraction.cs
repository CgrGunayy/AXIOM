using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayermask;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 2f, interactionLayermask);

            if (hit.collider != null && hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
            {
                interactable.Interact();
            }
        }
    }
}
