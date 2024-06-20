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
            money = data.Money;
            _uiManager.moneyText.text = data.Money.ToString();
        }

        public void ClickMoneyButton()
        {
            GameDataSystem.GameData data = GameDataSystem.LoadData();

            money += data.ClickForce;
            _uiManager.moneyText.text = money.ToString();

            StartCoroutine(ClickEffect());

            GameDataSystem.SaveData(newData =>
            {
                newData.Money = money;
            });
        }

        private IEnumerator ClickEffect()
        {
            _uiManager.coinClickEffect.GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            _uiManager.coinClickEffect.GetComponent<Image>().enabled = false;
        }
    }
}