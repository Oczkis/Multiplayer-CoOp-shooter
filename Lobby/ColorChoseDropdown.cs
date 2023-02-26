using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChoseDropdown : MonoBehaviour
{
    private Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }

    public void ChooseColor()
    {
        LobbyMenu.Instance.ChooseColor(dropdown.value);
    }
}
