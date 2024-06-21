using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class SkinSystem : MonoBehaviour
    {
        [SerializeField] private int cost;
        [SerializeField] private int level;

        [SerializeField] private Image image;
        [SerializeField] private Image otherImage;
        
        [SerializeField] private Image image2;
        [SerializeField] private Image otherImage2;
        
        [SerializeField] private Sprite buyingSprite;
        [SerializeField] private Sprite eqSprite;

        [SerializeField] private TMP_Text coinText;

        private void Start()
        {
            coinText.text = GameDataSystem.LoadData().Money.ToString();
            var data = GameDataSystem.LoadData();

            if (data.CurrentSkin == 1)
            {
                image2.sprite = eqSprite;
                otherImage2.sprite = buyingSprite;
            }
            if (data.CurrentSkin == 0 && data.SkinLevel == 1)
            {
                image2.sprite = buyingSprite;
                otherImage2.sprite = eqSprite;
            }
        }

        private void ApplySkin()
        {
            GameDataSystem.SaveData(data =>
            {
                data.CurrentSkin = level;
            });

            image.sprite = eqSprite;
            otherImage.sprite = buyingSprite;
        }

        public void BuySkin()
        {
            var data = GameDataSystem.LoadData();
            
            if (data.SkinLevel >= 1)
            {
                ApplySkin();
            }
            else
            {
                if (data.Money >= cost)
                {
                    GameDataSystem.SaveData(updatedData =>
                    {
                        updatedData.Money -= cost;
                        updatedData.SkinLevel = level;
                    });
                    coinText.text = GameDataSystem.LoadData().Money.ToString();
                    
                    ApplySkin();
                }
                else
                {
                    Debug.Log("Not enough money to buy the skin.");
                }
            }
        }
    }
}