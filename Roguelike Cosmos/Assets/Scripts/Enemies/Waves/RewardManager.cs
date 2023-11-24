using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RewardType;
using TMPro;
using System;

public class RewardManager : MonoBehaviour
{
    private Vector3 position;
    public GameObject pausePrefab;
    [SerializeField]
    public List<RewardTypeData> rewards;
    public List<float> prob;

    public RewardPool rp;

    [SerializeField]
    private GameObject screen;

    public GameObject statsTextGO;
    public GameObject reward1;
    public GameObject reward2;
    public GameObject reward3;
    public Player.PlayerData data;


 
    public void ReleaseReward()
    {

        screen.SetActive(true);

        if (screen)
        {
            // Verificando se a quantidade de probabilidades na lista bate com a quantidade
            // de rewards
            if (prob.Count > rewards.Count)
            {
                while (prob.Count > rewards.Count)
                {
                    prob.RemoveAt(0);
                }
            }
            else if (prob.Count > rewards.Count)
            {
                while (prob.Count > rewards.Count)
                {
                    prob.Add(prob[0]);
                }
            }


            SetRewardCard(reward1);
            SetRewardCard(reward2);
            SetRewardCard(reward3);
            pausePrefab.GetComponent<PauseMenu>().PauseGame(false);
        }

        void SetRewardCard(GameObject reward)
        {
            /*
             * Função para sortear as recompensas que vão aparecer
            */
            RewardTypeData r = rp.GetRandomReward();


            reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = r.title;
            reward.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = r.descriptions[0];
           

            for (int j = 1; j < r.descriptions.Count; j++)
            {
                TextMeshProUGUI tm = CreateText(reward.transform.GetChild(2));
                tm.text = r.descriptions[j];
            }

            Reward rew = reward.GetComponent<Reward>();
            rew.modifier = new List<Player.PlayerModifiers>();

            for (int k = 0; k < r.modifier.Length; k++)
            {
                rew.modifier.Add(r.modifier[k]);
            }



        }
    }
    private TextMeshProUGUI CreateText(Transform parent)
    {
        GameObject go = Instantiate(statsTextGO);
        go.transform.SetParent(parent);
        TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
        text.fontSize = 18;
        return text;
    }



    public void ChoosedReward(GameObject rc)
    {
        /*
         Função para atribuir os atributos da recompensa escolhida
         */
        pausePrefab.GetComponent<PauseMenu>().ResumeGame();
        Debug.Log("Você pegou: " + rc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        Reward r = rc.GetComponent<Reward>();

        foreach(Player.PlayerModifiers mod in r.modifier)
        {
            switch (mod.stat)
            {
                case Player.PlayerModifier.Strength:
                    data.temp_modifier[0].value += mod.value;
                    break;
                case Player.PlayerModifier.Constitution:
                    data.temp_modifier[1].value += mod.value;
                    break;
                case Player.PlayerModifier.Agility:
                    data.temp_modifier[2].value += mod.value;
                    break;
                case Player.PlayerModifier.Intelligence:
                    data.temp_modifier[3].value += mod.value;
                    break;
                case Player.PlayerModifier.Wisdom:
                    data.temp_modifier[4].value += mod.value;
                    break;
            }
        }


        screen.SetActive(false);
    }
}
