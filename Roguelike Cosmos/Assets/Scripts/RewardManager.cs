using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RewardType;
using TMPro;

public class RewardManager : MonoBehaviour
{
    private Vector3 position;

    [SerializeField]
    public List<RewardTypeData> rewards;
    public List<float> prob;

    [SerializeField]
    List<RewardTypeData> rewardsAvailable;
    [SerializeField]
    List<float> probAvailable;

    [SerializeField]
    private GameObject screen;

    public GameObject statsTextGO;
    public GameObject reward1;
    public GameObject reward2;
    public GameObject reward3;

    public void ReleaseReward()
    {
        screen.SetActive(true);

        if (screen) {
            // Verificando se a quantidade de probabilidades na lista bate com a quantidade de rewards
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

            rewardsAvailable = new List<RewardTypeData>(rewards);
            probAvailable = new List<float>(prob);

            SetRewardCard(reward1, rewardsAvailable, probAvailable);
            SetRewardCard(reward2, rewardsAvailable, probAvailable);
            SetRewardCard(reward3, rewardsAvailable, probAvailable);
        }   

        void SetRewardCard(GameObject reward, List<RewardTypeData> rewardsAv, List<float> probAv)
        {
            /*
             * Função para sortear as recompensas que vão aparecer
            */

            probAv.Sort();
            float sum = 0;
            foreach (float value in probAv)
                sum += value;
 
            
            // Número sorteado
            float randValue = Random.Range(0, sum);

            sum = 0;
            for (int i = 0; i < probAv.Count; i++)
            {
                sum += probAv[i];
                
                // Itera até que a soma das probabilidades seja maior que o randValue
                if (randValue <= sum)
                {
                    if (rewardsAv[i] != null)
                    {
                        reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rewardsAv[i].title;
                        reward.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = rewardsAv[i].descriptions[0];
                        //Ainda bugado, era para inserir os atributos que vão ser adicionados 
                        
                        for (int j = 1; j < rewardsAv[i].descriptions.Count; j++)
                        {
                            TextMeshProUGUI tm = CreateText(reward.transform.GetChild(2));
                            tm.text = rewardsAv[i].descriptions[j];
                        }
                        probAv.RemoveAt(i);
                        rewardsAv.RemoveAt(i);
                    }
                    break;
                }
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
        Debug.Log("Você pegou: " + rc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        screen.SetActive(false);
    }
}
