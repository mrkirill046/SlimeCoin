using UnityEngine;
using Systems;
using UI;
using UnityEngine.UI;

namespace Upgrades
{
    public class GardenUpgradeManager : MonoBehaviour
    {
        public Sprite texture;
        public Sprite[] buttonTextures;
        public GameObject position;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private ClickSystem clickSystem;

        private GardenSystem _gardenSystem;

        public void Initialize()
        {
            _gardenSystem = FindFirstObjectByType<GardenSystem>();
        }

        public void ClickUpgradeButton()
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            switch (data.GardenUpgradeLevel)
            {
                case 0:
                    BuyUpgrade(1500, 1);
                    break;
                case 1:
                    BuyUpgrade(4500, 2);
                    break;
                default:
                    Debug.Log("Error in buy garden upgrade");
                    break;
            }
        }

        private void BuyUpgrade(long cost, int lvl)
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            if (data.Money < cost)
            {
                Debug.Log("Error, money < lvl cost");
                return;
            }

            GameDataSystem.SaveData(newData =>
            {
                newData.GardenUpgradeLevel = lvl;
                newData.Money = data.Money -= cost;
            });
            
            AddTexture(lvl);
            uiManager.moneyText.text = GameDataSystem.LoadData().Money.ToString();
            clickSystem.money = GameDataSystem.LoadData().Money;

            if (GameDataSystem.LoadData().GardenUpgradeLevel == 2)
            {
                GetComponent<Button>().interactable = false;
            }

            _gardenSystem.StopAllCoroutines();
            _gardenSystem.StartCoroutine(_gardenSystem.GardenMoney());
            _gardenSystem.Initialize();
        }

        private void AddTexture(int textureId)
        {
            if (textureId is < 1 or > 1)
            {
                Debug.LogWarning("Invalid textureId: " + textureId);
                return;
            }

            int index = textureId - 1;

            if (!position.GetComponent<Image>())
            {
                position.AddComponent<Image>().sprite = texture;
            }
            uiManager.gardenButton.sprite = buttonTextures[index];
        }
    }
}