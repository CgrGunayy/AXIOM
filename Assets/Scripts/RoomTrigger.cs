using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public bool IsTriggered { get; private set; }

    public GameObject activateRoom;
    public GameObject deactivateRoom;
    [Space]
    public RoomTrigger prequisite;

    private void OnTriggerEnter(Collider other)
    {
        if (prequisite != null && !prequisite.IsTriggered)
            return;

        if (IsTriggered)
            return;

        activateRoom.SetActive(true);
        deactivateRoom.SetActive(false);
        IsTriggered = true;
    }
}
