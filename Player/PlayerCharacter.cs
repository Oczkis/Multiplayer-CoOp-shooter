using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacter : Character
{
    public Material defaultMaterial;
    public Material[] colourMaterials;

    [SyncVar(hook = nameof(ClientHandleColourChanged))]
    private int colourID;

    [SerializeField] private GameObject characterModel;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    public override void OnStartClient()
    {
        if(hasAuthority)
        {
            CameraManager.Instance.FollowMyCharacter(characterModel);
        }
        else
        {
            GetComponent<PlayerCharacterMotor>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
        }      
    }

    public void Initialize(int colourId, int playerId, Vector3 pos)
    {
        colourID = colourId;
        characterID = playerId;
        NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
        TargetStartingPosition(networkIdentity.connectionToClient, pos);
        Respawn();
    }

    private void ClientHandleColourChanged(int oldValue, int newValue)
    {
        skinnedMeshRenderer.materials = new Material[] { defaultMaterial, colourMaterials[colourID] };
    }

    [TargetRpc]
    private void TargetStartingPosition(NetworkConnection target, Vector3 pos)
    {
        transform.position = pos;
    }
}
