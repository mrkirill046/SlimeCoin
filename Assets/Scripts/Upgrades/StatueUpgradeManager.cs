using Systems;
using UI;
using UnityEngine;

namespace Upgrades
{
    public class StatueUpgradeManager : MonoBehaviour
    {
        public Sprite[] buttonTextures;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private ClickSystem clickSystem;

        public void ClickUpgradeButton()
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            switch (data.StatueUpgradeLevel)
            {
                case 0:
                    BuyUpgrade(100_000, 1);
                    break;
                case 1:
                    BuyUpgrade(1_000_000, 2);
                    break;
                case 2:
                    BuyUpgrade(10_000_000, 3);
                    break;
                case 3:
                    BuyUpgrade(65_000_000, 4);
                    break;
                default:
                    Debug.Log("Error in buy statue upgrade");
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
                newData.StatueUpgradeLevel = lvl;
                newData.Money = data.Money -= cost;
                newData.ClickForce += lvl * 100;
            });
            
            PlayerPrefs.SetInt("CurrentClickForce", GameDataSystem.LoadData().ClickForce);
            AddTexture(lvl);
            uiManager.moneyText.text = GameDataSystem.LoadData().Money.ToString();
            clickSystem.money = GameDataSystem.LoadData().Money;
        }

        private void AddTexture(int textureId)
        {
            if (textureId is < 1 or > 4)
            {
                Debug.LogWarning("Invalid textureId: " + textureId);
                return;
            }
            
            uiManager.statueButton.sprite = buttonTextures[textureId - 1];
        }
    }
}