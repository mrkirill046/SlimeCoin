using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class SlimeSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource music;
        
        [SerializeField] private ChickenMoneyUI chickenMoneyUI;
        [SerializeField] private ChickenTypes chickenType;
        [SerializeField] private SlimeTypes slimeType;

        private const int PlusSlime = 1;

        private void Start()
        {
            CheckSlime();
        }

        private void CheckSlime()
        {
            var data = GameDataSystem.LoadData();

            if (slimeType == SlimeTypes.One && data.OneSlime >= 1)
            {
                SetButtonVisible(GetComponent<Button>());
            }
            else if (slimeType == SlimeTypes.Two && data.TwoSlime >= 1)
            {
                SetButtonVisible(GetComponent<Button>());
            }
            else if (slimeType == SlimeTypes.Three && data.ThreeSlime >= 1)
            {
                SetButtonVisible(GetComponent<Button>());
            }
        }

        private bool CheckMoney(int cost)
        {
            return GameDataSystem.LoadData().Money >= cost;
        }

        private void UpdateChickenCount(ChickenTypes chickenTypes, int cost)
        {
            GameDataSystem.SaveData(data =>
            {
                switch (chickenTypes)
                {
                    case ChickenTypes.HenHenChicken:
                        data.HenHenChickenMoney -= cost;
                        break;
                    case ChickenTypes.StonyHenChicken:
                        data.StonyHenChickenMoney -= cost;
                        break;
                    case ChickenTypes.BriarHenChicken:
                        data.BriarHenChickenMoney -= cost;
                        break;
                    case ChickenTypes.RoostroChicken:
                        data.RoostroChickenMoney -= cost;
                        break;
                }
            });
        }

        private void UpdateSlimeCount(SlimeTypes slimeTypes, int cost)
        {
            GameDataSystem.SaveData(data =>
            {
                switch (slimeTypes)
                {
                    case SlimeTypes.One:
                        data.OneSlime += cost;
                        break;
                    case SlimeTypes.Two:
                        data.TwoSlime += cost;
                        break;
                    case SlimeTypes.Three:
                        data.ThreeSlime += cost;
                        break;
                }
            });
        }

        public void BuySlime(int cost)
        {
            CheckSlime();

            if (CheckMoney(cost))
            {
                UpdateChickenCount(chickenType, cost);
                UpdateSlimeCount(slimeType, PlusSlime);

                chickenMoneyUI.InitializeText();
                music.Play();
            }
        }

        private void SetButtonVisible(Button btn)
        {
            btn.interactable = false;
        }
    }

    public enum ChickenTypes
    {
        HenHenChicken,
        StonyHenChicken,
        BriarHenChicken,
        RoostroChicken
    }

    public enum SlimeTypes
    {
        One,
        Two,
        Three
    }
}