using UnityEngine;

namespace Systems
{
    public class ChickenMoneySystem : MonoBehaviour
    {
        [SerializeField] private AudioSource music;
        
        [SerializeField] private ChickenType chickenType;

        private bool CheckMoney(int cost)
        {
            Debug.Log("Not money for buy chicken");
            return GameDataSystem.LoadData().Money >= cost;
        }

        private void UpdateChickenCount(ChickenType chickenTypes)
        {
            GameDataSystem.SaveData(data =>
            {
                switch (chickenTypes)
                {
                    case ChickenType.HenHenChicken:
                        data.HenHenChickenMoney += 1;
                        break;
                    case ChickenType.StonyHenChicken:
                        data.StonyHenChickenMoney += 1;
                        break;
                    case ChickenType.BriarHenChicken:
                        data.BriarHenChickenMoney += 1;
                        break;
                    case ChickenType.RoostroChicken:
                        data.RoostroChickenMoney += 1;
                        break;
                }
            });
        }

        public void BuyChicken(int cost)
        {
            if (CheckMoney(cost))
            {
                GameDataSystem.SaveData(data =>
                {
                    data.Money -= cost;
                });
                
                UpdateChickenCount(chickenType);
                Debug.Log("Buy chickens!");
                music.Play();
            }
        }
    }

    public enum ChickenType
    {
        HenHenChicken,
        StonyHenChicken,
        BriarHenChicken,
        RoostroChicken
    }
}