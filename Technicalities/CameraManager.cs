using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    public CinemachineFreeLook followCamera;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void FollowMyCharacter(GameObject playerCharacterModel)
    {
        followCamera.m_Follow = playerCharacterModel.transform;
        followCamera.m_LookAt = playerCharacterModel.transform;
    }
}
