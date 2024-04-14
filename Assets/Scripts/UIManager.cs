using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerAttribute playerAtt;

    public Image healthBarFill, energyBarFill;

    public Gradient healthBarGradient;

    public Color energyFull_Color, energyDepleted_Color;

    public Text score;

    public GameManager gm;

    public ScoreHistoryManager scoreHistoryManager;

    public GameObject scoreDataPrefab, scoreHistoryPanelContainer, scoreHistoryGameobject;

    bool isScoreHistoryCreated;

    float dur;
    // Start is called before the first frame update
    void Start()
    {
        //dur = playerAtt.maxInvisEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        SetUIValue();
    }

    void SetUIValue()
    {
        SetHealthBarFill();

        SetEnergyBarFill();

        SetScore();

        SetScoreHistory();
    }

    void SetHealthBarFill()
    {
        healthBarFill.fillAmount = playerAtt.healthPoints / (float)playerAtt.maxHealthPoints;

        healthBarFill.color = healthBarGradient.Evaluate(healthBarFill.fillAmount);
    }

    void SetEnergyBarFill()
    {
        if (!playerAtt.isInvis)
        {
            dur = 0;
            energyBarFill.fillAmount = playerAtt.invisEnergy / playerAtt.maxInvisEnergy;

            if(energyBarFill.fillAmount >= 1)
            {
                energyBarFill.color = energyFull_Color;
            }
        }


        else
        {
            DepleteEnergyOvertime();
        }
    }
    void DepleteEnergyOvertime()
    {
        dur += Time.deltaTime;
        energyBarFill.fillAmount = 1 - ((1 / (float)playerAtt.invisDuration) * dur);
        energyBarFill.color = energyDepleted_Color;
    }

    void SetScore()
    {
        score.text = gm.score.ToString();
    }

    void SetScoreHistory()
    {
        if (gm.isEnd == true && !isScoreHistoryCreated)
        {
            for (int i = 1; i < scoreHistoryManager.historyData.Count; i++)
            {
                GameObject temp = Instantiate(scoreDataPrefab, scoreHistoryPanelContainer.transform);
                temp.transform.GetChild(0).GetComponent<Text>().text = scoreHistoryManager.historyData[i].name;
                temp.transform.GetChild(1).GetComponent<Text>().text = scoreHistoryManager.historyData[i].score.ToString();
            }

            scoreHistoryGameobject.SetActive(true);

            isScoreHistoryCreated = true;
        }
    }
}
