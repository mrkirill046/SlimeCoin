using System.Collections;
using UnityEngine;
using UI;
using UnityEngine.UI;

namespace Systems
{
    public class ClickSystem : MonoBehaviour
    {
        public long money;

        private UIManager _uiManager;

        public void Initialize()
        {
            _uiManager = FindFirstObjectByType<UIManager>();

            GameDataSystem.GameData data = GameDataSystem.LoadData();
            money = data.money;
            _uiManager.moneyText.text = data.money.ToString();
        }

        public void ClickMoneyButton()
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            money += data.clickForce;
            _uiManager.moneyText.text = money.ToString();

            StartCoroutine(ClickEffect());

            GameDataSystem.GameData newData = new GameDataSystem.GameData
            {
                money = money,
                clickForce = data.clickForce,
                upgradeLevel = data.upgradeLevel
            };

            GameDataSystem.SaveData(newData);
        }

        private IEnumerator ClickEffect()
        {
            _uiManager.coinClickEffect.GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            _uiManager.coinClickEffect.GetComponent<Image>().enabled = false;
        }
    }
}