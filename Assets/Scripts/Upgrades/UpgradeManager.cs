using UnityEngine;
using UnityEngine.UI;
using Systems;
using UI;

namespace Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        public Sprite[] textures;
        public Sprite[] buttonTextures;
        public GameObject[] positions;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private ClickSystem clickSystem;

        public void ClickUpgradeButton()
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            switch (data.UpgradeLevel)
            {
                case 0:
                    BuyUpgrade(100, 1);
                    break;
                case 1:
                    BuyUpgrade(250, 2);
                    break;
                case 2:
                    BuyUpgrade(500, 3);
                    break;
                case 3:
                    BuyUpgrade(1000, 4);
                    break;
                case 4:
                    BuyUpgrade(2500, 5);
                    break;
                case 5:
                    BuyUpgrade(10_000, 6);
                    break;
                case 6:
                    BuyUpgrade(25_000, 7);
                    break;
                case 7:
                    BuyUpgrade(50_000, 8);
                    break;
                case 8:
                    BuyUpgrade(120_000, 9);
                    break;
                case 9:
                    BuyUpgrade(500_000, 10);
                    break;
                case 10:
                    BuyUpgrade(1_500_000, 11);
                    break;
                case 11:
                    BuyUpgrade(8_000_000, 12);
                    break;
                case 12:
                    BuyUpgrade(24_000_000, 13);
                    break;
                case 13:
                    BuyUpgrade(64_000_000, 14);
                    break;
                case 14:
                    BuyUpgrade(84_000_000, 15);
                    break;
                case 15:
                    BuyUpgrade(200_000_000, 16);
                    break;
                case 16:
                    BuyUpgrade(450_000_000, 17);
                    break;
                case 17:
                    BuyUpgrade(700_000_000, 18);
                    break;
                case 18:
                    BuyUpgrade(900_000_000, 19);
                    break;
                case 19:
                    BuyUpgrade(1_500_000_000, 20);
                    break;
                default:
                    Debug.Log("Error in buy upgrade");
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
                newData.UpgradeLevel = lvl;
                newData.Money = data.Money -= cost;
                newData.ClickForce += 1;
            });
            
            PlayerPrefs.SetInt("CurrentClickForce", GameDataSystem.LoadData().ClickForce);
            AddTexture(lvl);
            uiManager.moneyText.text = GameDataSystem.LoadData().Money.ToString();
            clickSystem.money = GameDataSystem.LoadData().Money;
        }

        private void AddTexture(int textureId)
        {
            if (textureId is < 1 or > 20)
            {
                Debug.LogWarning("Invalid textureId: " + textureId);
                return;
            }

            int index = textureId - 1;

            positions[index].AddComponent<Image>().sprite = textures[index];
            positions[index].GetComponent<Image>().raycastTarget = false;
            uiManager.upgradeButton.sprite = buttonTextures[index];
        }
    }
}