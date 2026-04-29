using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent finish;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DelayedFunction win = new DelayedFunction(Win, 1000);
        }
    }

    private void Win()
    {
        finish.Invoke();
        DelayedFunction mainMenu = new DelayedFunction(Main, 1000);
    }

    private void Main()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
