using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Bonfire bonfire;
    [SerializeField] private float bombRadius = 15;
    [SerializeField] private GameObject dark;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) ResourceData.LogAmount += 10;
        if (Input.GetKeyDown(KeyCode.G)) Bomb();
        if (Input.GetKeyDown(KeyCode.L)) DarkSwitch();
        if (Input.GetKeyDown(KeyCode.P)) Pyromania();
        if (Input.GetKeyDown(KeyCode.O)) Pyrophobia();
        if (Input.GetKeyDown(KeyCode.I)) Invulnerability();
    }

    private void Bomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);
        foreach (Collider2D coll in colliders)
        {
            if (coll.gameObject.CompareTag("Monster"))
            {
                coll.gameObject.GetComponent<Enemy>().OnDamage(9999);
            }
        }
    }

    private void DarkSwitch()
    {
        if (dark.activeSelf) dark.SetActive(false);
        else dark.SetActive(true);
    }

    private void Pyromania()
    { 
        bonfire.LeftTime = 100;
    }

    private void Pyrophobia()
    {
        bonfire.LeftTime = 0;
    }

    private void Invulnerability()
    {
        
    }
}
