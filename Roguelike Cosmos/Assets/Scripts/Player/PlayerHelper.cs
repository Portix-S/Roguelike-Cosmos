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

    }

    private void AreaSkill(){
        pc.AreaSkill();
        StartCoroutine(FinishAttack());
    }

    IEnumerator FinishAttack(){
        yield return new WaitForSeconds(.8f);
        pc.isAreaCasting = false;
    }

}
