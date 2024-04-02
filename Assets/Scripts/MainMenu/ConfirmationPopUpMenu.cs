using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmationPopUpMenu : Menu
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    // Activates the confirmation popup menu with provided display text and actions for confirm and cancel
    public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
    {
        // Make the popup menu visible
        this.gameObject.SetActive(true);

        // Set the display text in the popup menu
        this.displayText.text = displayText;

        // Remove any existing listeners
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // Assign new onClick listeners for confirm and cancel actions
        confirmButton.onClick.AddListener(() => {
            DeactivateMenu();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() => {
            DeactivateMenu();
            cancelAction();
        });
    }

    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
