using Systems;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ChickenMoneyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text henText;
        [SerializeField] private TMP_Text stonyText;
        [SerializeField] private TMP_Text briarText;
        [SerializeField] private TMP_Text roostroText;

        private void Start()
        {
            InitializeText();
        }

        public void InitializeText()
        {
            var data = GameDataSystem.LoadData();

            henText.text = data.HenHenChickenMoney.ToString();
            stonyText.text = data.StonyHenChickenMoney.ToString();
            briarText.text = data.BriarHenChickenMoney.ToString();
            roostroText.text = data.RoostroChickenMoney.ToString();
        }
    }
}