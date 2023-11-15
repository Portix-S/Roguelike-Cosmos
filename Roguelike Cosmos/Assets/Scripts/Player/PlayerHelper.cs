using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    private PlayerCombat pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerCombat>();
    }

    private void RangedSkill(){
        pc.RangedSkill();
    }

    private void AreaSkill(){
        pc.AreaSkill();
    }

    private void FinishAttack(){
        pc.isAreaCasting = false;
        pc.isShooting = false;

    }

}
