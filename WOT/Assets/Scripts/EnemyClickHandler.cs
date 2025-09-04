using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class EnemyClickHandler : MonoBehaviour
{
    private Camera mainCam;
    private InputSystem_Actions inputActions;
    [SerializeField] Health health;
    [SerializeField] CharacterStats characterStats;
    private void OnEnable()
    {
        mainCam = Camera.main;
        health = GetComponent<Health>();
        characterStats = GetComponent<CharacterStats>();

        inputActions = new InputSystem_Actions();
        inputActions.UI.Click.Enable();

        EnhancedTouchSupport.Enable();
        inputActions.Enable();
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Game")
            return;

        if (CombatManager.instance.GetPlayerTurn && inputActions.UI.Click.WasPerformedThisFrame())
        {
            if (health.GetHealth <= 0) return;

            Vector2 screenPos;

            // Mouse
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                screenPos = Mouse.current.position.ReadValue();
            }
            // Touch
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            else
            {
                return;
            }

            Ray ray = mainCam.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    HandleClick();
                }
            }
        }
    }

    private void HandleClick()
    {
        CharacterStats[] charStats = FindObjectsByType<CharacterStats>(FindObjectsSortMode.None);

        for (int i = 0; i < charStats.Length; i++)
        {
            if (charStats[i].selectionIcon != null)
                charStats[i].selectionIcon.SetActive(false);
        }

        
        if (characterStats != null)
        {
            Debug.Log(characterStats.characterName + " Clicked");
            CombatManager.instance.GetCurrentTargetedEnemy = gameObject;
            characterStats.selectionIcon.SetActive(true);
        }
    }

    public void TurnOffSelector()
    {
        characterStats.selectionIcon.SetActive(false);
    }
}
