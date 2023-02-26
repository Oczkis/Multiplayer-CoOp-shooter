using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] hitFX = new GameObject[0];

    public void ReceiveDamage(int damage)
    {

    }

    public GameObject GetHitFX()
    {
        return hitFX[Random.Range(0, hitFX.Length)];
    }
}
