using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script adicionado no objeto filho do jogador que mantém o modelo e o animator
    Contém funções auxiliares que são chamadas por evento nas animações
*/
public class PlayerHelper : MonoBehaviour
{
    private PlayerCombat pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerCombat>();
    }

    private void RangedSkill(){
        pc.Shoot();
    }

    private void AreaSkill(){
        pc.AreaSkill();
    }

    private void FinishAttack(){
        pc.isShooting = false;
        pc.canShoot = true;

        pc.isAreaCasting = false;
    }

}
