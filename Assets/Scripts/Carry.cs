using UnityEngine;

public class Carry : MonoBehaviour, IInteractionResult
{
    public bool carrying;

    public void Run()
    {
        carrying = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && carrying)
        {
            carrying = false;
        }

        if (carrying)
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2; 
        }
    }
}
