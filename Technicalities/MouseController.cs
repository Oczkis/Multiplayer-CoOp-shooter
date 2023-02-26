using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private static MouseController _instance;

    public static MouseController Instance { get { return _instance; } }

    private static Vector3 _worldMousePosition;

    public static Vector3 WorldMousePosition { get { return _worldMousePosition; } }

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string colliderTagToDetect;

    private void Update()
    {
        HandleMouseToWorldPosition();
    }

    private void HandleMouseToWorldPosition()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.tag == colliderTagToDetect)
            {
                _worldMousePosition = new Vector3(hit.point.x, 2, hit.point.z);
            }
        }
    }
}
