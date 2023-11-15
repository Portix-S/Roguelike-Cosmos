using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;

public class StatsGiver : MonoBehaviour
{
    public PlayerSkills.SkillType playerSkill;
    private GameManager gm;

    public PlayerModifiers[] modifiers;
    private Button button;
    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowPopup);
    }

    public void ShowPopup()
    {
        // button.y+155 --> Altura que deve spawnar o popup
        // Chamar pelo gm
        Debug.Log("Can unlock: " + gm.playerSkills.CanUnlock(playerSkill)); // Loses mana?
        if(gm.playerSkills.CanUnlock(playerSkill))
            gm.ShowPopup(this.transform, playerSkill ,modifiers, this);
    }
}
