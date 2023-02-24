using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    DeviceType system;

    [Header("Animation")]
    public Animator playerAnimator;

    private void Start()
    {
        system = SystemInfo.deviceType;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0) && system == DeviceType.Desktop)
        {
            //playerAnimator.SetBool("isPunching", true);
            Debug.Log(_playerData.attackDamage);
        }
    }
}
