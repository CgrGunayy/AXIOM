using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Interact()
    {
        var results = GetComponents<IInteractionResult>();
        foreach (var result in results)
        {
            result.Run();
        }
    }
}
