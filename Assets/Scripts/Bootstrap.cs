using UnityEngine;
using UnityEngine.UI;
using Systems;
using Upgrades;
using UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ClickSystem clickSystem;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ClickUpgradeManager clickUpgradeManager;
    [SerializeField] private BeatrixSystem beatrixSystem;

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
            GameDataSystem.SaveData(defaultData =>
            {
                defaultData.Money = 0;
                defaultData.ClickForce = 1;
                defaultData.UpgradeLevel = 0;
                defaultData.ClickUpgradeLevel = 0;
                defaultData.BeatrixBuying = false;
            });
        }

        if (upgradeManager != null) SetTextures();
        if (beatrixSystem != null) SetBeatrix();
    }

    private void SetBeatrix()
    {
        if (GameDataSystem.LoadData().BeatrixBuying)
        {
            beatrixSystem.beatrixIsBuyingButton.interactable = false;
            beatrixSystem.bobsShopDefaultSprite.sprite = beatrixSystem.bobsShopIsAvailableSprite;
            beatrixSystem.beatrixPosition.AddComponent<Image>().sprite = beatrixSystem.beatrixSprite;
        }
    }

    private void SetTextures()
    {
        GameDataSystem.GameData data = GameDataSystem.LoadData();

        if (data.UpgradeLevel is < 1 or > 20)
        {
            Debug.LogWarning("Invalid level: " + data.UpgradeLevel);
            return;
        }

        if (data.ClickUpgradeLevel is < 1 or > 7)
        {
            Debug.LogWarning("Invalid clickUpgradeLevel: " + data.ClickUpgradeLevel);
            return;
        }

        for (int i = 0; i < data.UpgradeLevel; i++)
        {
            if (upgradeManager.positions[i].GetComponent<Image>() == null)
            {
                upgradeManager.positions[i].AddComponent<Image>().sprite = upgradeManager.textures[i];
                upgradeManager.positions[i].GetComponent<Image>().raycastTarget = false;
            }
            else
            {
                upgradeManager.positions[i].GetComponent<Image>().sprite = upgradeManager.textures[i];
                upgradeManager.positions[i].GetComponent<Image>().raycastTarget = false;
            }
        }

        for (int i = 0; i < data.ClickUpgradeLevel; i++)
        {
            if (clickUpgradeManager.positions[i].GetComponent<Image>() == null)
            {
                clickUpgradeManager.positions[i].AddComponent<Image>().sprite = clickUpgradeManager.clickSprite;
                clickUpgradeManager.positions[i].GetComponent<Image>().raycastTarget = false;
            }
            else
            {
                clickUpgradeManager.positions[i].GetComponent<Image>().sprite = clickUpgradeManager.clickSprite;
                clickUpgradeManager.positions[i].GetComponent<Image>().raycastTarget = false;
            }
        }

        uiManager.upgradeButton.sprite = upgradeManager.buttonTextures[data.UpgradeLevel - 1];
        uiManager.clickUpgradeButton.sprite = clickUpgradeManager.buttonTextures[data.ClickUpgradeLevel - 1];
    }
}