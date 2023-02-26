using UnityEngine;
using UnityEngine.UI;

public class LobbySlotPanel : MonoBehaviour
{
    public bool isSlotEmpty;
    public int slotIndex;
    public Text playerName;
    public Image chosenColor;

    public Dropdown choseColorButton;
    public void SetInteractable(bool interactable)
    {
        Debug.Log("Slot number of " + slotIndex.ToString() + " is interactable? " + choseColorButton.interactable);
        choseColorButton.interactable = interactable;
    }

    public void Open()
    {
        chosenColor.gameObject.SetActive(false);
    }

    public void Occupy()
    {
        chosenColor.gameObject.SetActive(true);
    }
}
