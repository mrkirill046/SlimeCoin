using UnityEngine;
using Systems;
using UnityEngine.UI;
using UI;

namespace Upgrades
{
    public class ClickUpgradeManager : MonoBehaviour
    {
        [SerializeField] private AudioSource music;
        
        [SerializeField] private UIManager uiManager;
        [SerializeField] private ClickSystem clickSystem;

        public GameObject[] positions;
        public Sprite[] buttonTextures;

        public Sprite clickSprite;

        public void ClickUpgradeClickButton()
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            switch (data.ClickUpgradeLevel)
            {
                case 0:
                    BuyUpgrade(2400, 1);
                    break;
                case 1:
                    BuyUpgrade(12_000, 2);
                    break;
                case 2:
                    BuyUpgrade(75_000, 3);
                    break;
                case 3:
                    BuyUpgrade(120_000, 4);
                    break;
                case 4:
                    BuyUpgrade(410_000, 5);
                    break;
                case 5:
                    BuyUpgrade(2_300_000, 6);
                    break;
                default:
                    Debug.Log("Error in buy click upgrade");
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
                newData.ClickUpgradeLevel = lvl;
                newData.Money = data.Money -= cost;
            });
            
            AddTexture(lvl);
            uiManager.moneyText.text = GameDataSystem.LoadData().Money.ToString();
            clickSystem.money = GameDataSystem.LoadData().Money;
            music.Play();
        }

        private void AddTexture(int textureId)
        {
            if (textureId is < 1 or > 7)
            {
                Debug.LogWarning("Invalid textureId: " + textureId);
                return;
            }

            int index = textureId - 1;

            positions[index].AddComponent<Image>().sprite = clickSprite;
            positions[index].GetComponent<Image>().raycastTarget = false;
            uiManager.clickUpgradeButton.sprite = buttonTextures[index];
        }
    }
}