using UnityEngine;
using UnityEngine.UI;
using EG;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject Player;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private ParticleSystem _LevelUpEffect;

    public int level;
    [SerializeField] private float currentXp;
    [SerializeField] private float requiredXp;


    [Header("Multiplier")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;
    void Start()
    {
        currentXp = 0;
        slider.value = currentXp;

        requiredXp = CalculateRequiredXp();
        slider.maxValue = requiredXp;

        levelText.text = "Lv. " + level;
    }

    void Update()
    {
        UpdateXpUi();

        if(currentXp > requiredXp)
        {
            LevelUp();
        }
        // test the xp system
        if(Input.GetKeyDown(KeyCode.H))
        {
            GainExperienceFlateRate(20);
        }
    }

    private void UpdateXpUi()
    {
        slider.value = currentXp;
        slider.maxValue = requiredXp;
    }
    public void GainExperienceFlateRate(float xpGained)
    {
        currentXp += xpGained;
    }

    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if(passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXp += xpGained * multiplier;
        }
        else
        {
            currentXp += xpGained;
        }
    }

    private void LevelUp()
    {
        level++;
        slider.value = 0;
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        // boost playerStats
        Player.GetComponent<PlayerStats>().IncreaseMaxHealthLevel(level);

        Instantiate(_LevelUpEffect, Player.transform);

        requiredXp = CalculateRequiredXp();
        slider.maxValue = requiredXp;
        levelText.text = "Lv. " + level;
    }

    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
}
