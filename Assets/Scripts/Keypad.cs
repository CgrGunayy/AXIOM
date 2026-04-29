using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Keypad : MonoBehaviour, IInteractionResult
{
    [SerializeField] private GameObject keypadUI;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private GameObject escButton;
    [SerializeField] private string answer;
    [Space]
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip failClip;
    [Space]
    [SerializeField] private UnityEvent correctEvent;

    private string input;

    AudioSource source;
    FirstPersonController fpsController;

    private void Awake()
    {
        fpsController = FindAnyObjectByType<FirstPersonController>(FindObjectsInactive.Include);
        source = GetComponent<AudioSource>();   
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void Run()
    {
        DelayedFunction delay = new DelayedFunction(Display, 700);
    }

    public void Input(int num)
    {
        input += num.ToString();
        display.text = input;
    }

    public void Delete()
    {
        if (input.Length == 0)
            return;

        input = input.Remove(input.Length - 1, 1);
        display.text = input;
    }

    public void Clear()
    {
        input = "";
        display.text = input;
    }

    public void Accept()
    {
        if (input == answer)
        {
            correctEvent.Invoke();
            display.text = "ERWIN";
            display.color = Color.green;
            source.PlayOneShot(correctClip);
            DelayedFunction delay = new DelayedFunction(Hide, 500);
        }
        else
        {
            source.PlayOneShot(failClip);
            Clear();
        }
    }

    private void Display()
    {
        fpsController.cameraCanMove = false;
        fpsController.playerCanMove = false;
        escButton.SetActive(true);
        keypadUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Hide()
    {
        fpsController.cameraCanMove = true;
        fpsController.playerCanMove = true;
        escButton.SetActive(false);
        keypadUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
