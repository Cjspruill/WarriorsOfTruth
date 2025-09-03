using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnemyClickHandler : MonoBehaviour
{
    private Camera mainCam;
    private InputAction clickAction;


    private void OnEnable()
    {        
        mainCam = Camera.main;

        // Create a simple click action if not using an InputAction asset
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.Enable();
    }

    private void OnDestroy()
    {
        clickAction.Disable();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (CombatManager.instance.GetPlayerTurn && clickAction.WasPerformedThisFrame())
            {
                Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        HandleClick();
                    }
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


        CharacterStats thisCharacterStats = GetComponent<CharacterStats>();
        if (thisCharacterStats != null)
        {
            Debug.Log(thisCharacterStats.characterName + " Clicked");
            CombatManager.instance.GetCurrentTargetedEnemy = gameObject;
            thisCharacterStats.selectionIcon.SetActive(true);
        }
    }
}
