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
    [SerializeField] private StatueUpgradeManager statueUpgradeManager;
    [SerializeField] private GardenUpgradeManager gardenUpgradeManager;
    [SerializeField] private GardenSystem gardenSystem;
    [SerializeField] private BeatrixSystem beatrixSystem;

    private void Awake()
    {
        Initialize();

        if (clickSystem != null) clickSystem.Initialize();
        if (sceneChanger != null) sceneChanger.Initialize();
        if (gardenUpgradeManager != null) gardenUpgradeManager.Initialize();
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
                defaultData.StatueUpgradeLevel = 0;
                defaultData.ClickUpgradeLevel = 0;
                defaultData.BeatrixBuying = false;
                defaultData.GardenUpgradeLevel = 0;
                defaultData.SkinLevel = 0;
                defaultData.CurrentSkin = 0;
                defaultData.HenHenChickenMoney = 0;
                defaultData.BriarHenChickenMoney = 0;
                defaultData.StonyHenChickenMoney = 0;
                defaultData.RoostroChickenMoney = 0;
                defaultData.OneSlime = 0;
                defaultData.TwoSlime = 0;
                defaultData.ThreeSlime = 0;
            });
        }

        PlayerPrefs.SetInt("CurrentClickForce", GameDataSystem.LoadData().ClickForce);

        if (upgradeManager != null) SetTextures();
        if (beatrixSystem != null) SetBeatrix();
        if (gardenSystem != null) SetGarden();
    }

    private void SetGarden()
    {
        if (GameDataSystem.LoadData().GardenUpgradeLevel > 0)
        {
            gardenUpgradeManager.position.AddComponent<Image>().sprite = gardenUpgradeManager.texture;
            uiManager.gardenButton.sprite = gardenUpgradeManager.buttonTextures[0];

            if (GameDataSystem.LoadData().GardenUpgradeLevel == 2)
            {
                uiManager.gardenButton.GetComponent<Button>().interactable = false;
            }
        }
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
        else
        {
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

            uiManager.upgradeButton.sprite = upgradeManager.buttonTextures[data.UpgradeLevel - 1];
        }

        if (data.ClickUpgradeLevel is < 1 or > 7)
        {
            Debug.LogWarning("Invalid clickUpgradeLevel: " + data.ClickUpgradeLevel);
            return;
        }
        else
        {
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

            uiManager.clickUpgradeButton.sprite = clickUpgradeManager.buttonTextures[data.ClickUpgradeLevel - 1];
        }

        if (data.StatueUpgradeLevel is < 1 or > 4)
        {
            Debug.LogWarning("Invalid statueUpgradeLevel: " + data.StatueUpgradeLevel);
            return;
        }
        else
        {
            uiManager.statueButton.sprite = statueUpgradeManager.buttonTextures[data.StatueUpgradeLevel - 1];
        }

        if (data.GardenUpgradeLevel is < 1 or > 2)
        {
            Debug.LogWarning("Invalid gardenUpgradeLevel: " + data.GardenUpgradeLevel);
            return;
        }
        else
        {
            if (data.GardenUpgradeLevel == 2)
            {
                gardenUpgradeManager.GetComponent<Button>().interactable = false;
            }
            else
            {
                uiManager.gardenButton.sprite = gardenUpgradeManager.buttonTextures[data.GardenUpgradeLevel - 1];
            }
        }
    }

    private void Start()
    {
        if (upgradeManager != null)
        {
            var data = GameDataSystem.LoadData();

            ChickenInfo[] chickenInfos = {
                new() { Money = data.HenHenChickenMoney, PositionIndex = 0, SpriteIndex = 0 },
                new() { Money = data.StonyHenChickenMoney, PositionIndex = 1, SpriteIndex = 1 },
                new() { Money = data.BriarHenChickenMoney, PositionIndex = 2, SpriteIndex = 2 },
                new() { Money = data.RoostroChickenMoney, PositionIndex = 3, SpriteIndex = 3 }
            };

            foreach (var chickenInfo in chickenInfos)
            {
                if (chickenInfo.Money > 0)
                {
                    uiManager.chickenPositions[chickenInfo.PositionIndex].AddComponent<Image>().sprite = uiManager.chickenSprites[chickenInfo.SpriteIndex];
                }
            }
            
            SlimeInfo[] slimeInfos = {
                new() { Money = data.OneSlime, PositionIndex = 0, SpriteIndex = 0 },
                new() { Money = data.TwoSlime, PositionIndex = 1, SpriteIndex = 1 },
                new() { Money = data.ThreeSlime, PositionIndex = 2, SpriteIndex = 2 }
            };

            foreach (var slimeInfo in slimeInfos)
            {
                if (slimeInfo.Money > 0)
                {
                    uiManager.slimePositions[slimeInfo.PositionIndex].AddComponent<Image>().sprite = uiManager.slimeSprites[slimeInfo.SpriteIndex];
                }
            }
            
            if (GameDataSystem.LoadData().CurrentSkin == 0)
            {
                uiManager.coinButton.sprite = uiManager.coins[0];
                uiManager.coinClickEffect.sprite = uiManager.coinEffects[0];
                uiManager.textBox.sprite = uiManager.textBoxes[0];
            }
            else
            {
                uiManager.coinButton.sprite = uiManager.coins[1];
                uiManager.coinClickEffect.sprite = uiManager.coinEffects[1];
                uiManager.textBox.sprite = uiManager.textBoxes[1];
            }

            switch (PlayerPrefs.GetString("BG"))
            {
                case "1":
                    uiManager.bg.sprite = uiManager.bgs[0];
                    break;
                case "2":
                    uiManager.bg.sprite = uiManager.bgs[1];
                    break;
                case "3":
                    uiManager.bg.sprite = uiManager.bgs[2];
                    break;
                case "4":
                    uiManager.bg.sprite = uiManager.bgs[3];
                    break;
                case "5":
                    uiManager.bg.sprite = uiManager.bgs[4];
                    break;
                default:
                    uiManager.bg.sprite = uiManager.bgs[1];
                    Debug.Log("Error in apply bg");
                    break;
            }
        }
    }
    
    private struct ChickenInfo
    {
        public int Money;
        public int PositionIndex;
        public int SpriteIndex;
    }
    
    private struct SlimeInfo
    {
        public int Money;
        public int PositionIndex;
        public int SpriteIndex;
    }
}