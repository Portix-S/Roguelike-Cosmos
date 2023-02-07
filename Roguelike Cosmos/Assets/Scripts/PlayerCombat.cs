using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    [Header("Animation")]
    public Animator playerAnimator;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //playerAnimator.SetBool("isPunching", true);
            Debug.Log(_playerData.attackDamage);
        }
    }
}
