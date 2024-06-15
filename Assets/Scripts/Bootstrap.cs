using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ClickSystem clickSystem;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        Initialize();

        if (clickSystem != null) clickSystem.Initialize();
        if (sceneChanger != null) sceneChanger.Initialize();
    }

    private void Initialize()
    {
        GameDataSystem.GameData data = GameDataSystem.LoadData();

        if (data == null)
        {
            GameDataSystem.GameData defaultData = new GameDataSystem.GameData
            {
                money = 0,
                clickForce = 1,
                upgradeLevel = 0
            };

            GameDataSystem.SaveData(defaultData);
        }

        if (upgradeManager != null) SetTextures();
    }

    private void SetTextures()
    {
        GameDataSystem.GameData data = GameDataSystem.LoadData();

        if (data.upgradeLevel < 1 || data.upgradeLevel > 20)
        {
            Debug.LogWarning("Invalid level: " + data.upgradeLevel);
            return;
        }

        for (int i = 0; i < data.upgradeLevel; i++)
        {
            if (upgradeManager.positions[i].GetComponent<Image>() == null)
            {
                upgradeManager.positions[i].AddComponent<Image>().sprite = upgradeManager.textures[i];
            }
            else
            {
                upgradeManager.positions[i].GetComponent<Image>().sprite = upgradeManager.textures[i];
            }
        }

        uiManager.upgradeButton.sprite = upgradeManager.buttonTextures[data.upgradeLevel - 1];
    }
}