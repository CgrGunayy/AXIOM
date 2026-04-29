using UnityEngine;
using UnityEngine.UI;

public class MissionTrigger : MonoBehaviour
{
    public bool IsTriggered { get; private set; }

    public string quest;
    [SerializeField] private Text text;
    [Space]
    public MissionTrigger prequisite;

    private void OnTriggerEnter(Collider other)
    {
        if (prequisite != null && !prequisite.IsTriggered)
            return;

        if (IsTriggered)
            return;

        text.text = quest;
        text.GetComponent<Animator>().SetTrigger("Mission");
        IsTriggered = true;
    }
}
