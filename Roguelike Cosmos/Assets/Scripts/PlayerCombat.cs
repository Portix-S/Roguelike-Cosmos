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

    [Header("Colliders")]
    [SerializeField] private BoxCollider leftHandCollider;
    [SerializeField] private BoxCollider rightHandCollider;

    [Header("")]
    public bool isAttacking;

    public void UpdateColliders(bool enable){
        leftHandCollider.enabled = enable;
        rightHandCollider.enabled = enable;
    }

    private void Awake() {
        UpdateColliders(false);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            //UpdateColliders(true);
            //isAttacking = true;
            playerAnimator.SetTrigger("isPunching");
        }
    }

    public void StartAttack(){
        isAttacking = true;
        UpdateColliders(true);
    }

    public void FinishAttack(){
        isAttacking = false;
        UpdateColliders(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemie"){
            UpdateColliders(false);
            Debug.Log("Dealing " + _playerData.attackDamage + " damage to an enemie");
        }
    }
}
