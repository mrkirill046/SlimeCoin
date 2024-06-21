using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class BgSystem : MonoBehaviour
    {
        [SerializeField] private Sprite buyingImage;
        [SerializeField] private int cost;
        [SerializeField] private string bgName;

        private void Awake()
        {
            if (PlayerPrefs.GetInt("2") == 0)
            {
                PlayerPrefs.SetInt("2", 1);
            }

            if (PlayerPrefs.GetInt(bgName) == 1)
            {
                GetComponent<Image>().sprite = buyingImage;
            }
        }

        private void ApplyBg()
        {
            PlayerPrefs.SetString("BG", bgName);
        }

        public void BuyBg()
        {
            if (GameDataSystem.LoadData().Money >= cost && PlayerPrefs.GetInt(bgName) == 0)
            {
                GameDataSystem.SaveData(data =>
                {
                    data.Money -= cost;
                });
            
                PlayerPrefs.SetInt(bgName, 1);
                GetComponent<Image>().sprite = buyingImage;
                ApplyBg();
            }
            else
            {
                ApplyBg();
            }
        }
    }
}