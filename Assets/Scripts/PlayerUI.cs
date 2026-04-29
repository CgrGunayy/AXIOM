using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Transform staminaBar;
    [SerializeField] private Transform shield;
    [SerializeField] private Transform brokenShield;

    FirstPersonController fpsController;

    private void Awake()
    {
        fpsController = GetComponent<FirstPersonController>();
    }

    public void BreakShield()
    {
        shield.gameObject.SetActive(false);
        brokenShield.gameObject.SetActive(true);
    }

    private void Update()
    {
        staminaBar.localScale = new Vector3(Mathf.Lerp(0, 0.03f, fpsController.Stamina / fpsController.MaxStamina), staminaBar.localScale.y, staminaBar.localScale.z);
    }
}
