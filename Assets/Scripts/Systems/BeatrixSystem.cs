using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class BeatrixSystem : MonoBehaviour
    {
        [SerializeField] private int cost;

        public GameObject beatrixPosition;
        
        public Image bobsShopDefaultSprite;

        public Button beatrixIsBuyingButton;
        public Sprite bobsShopIsAvailableSprite;
        public Sprite beatrixSprite;

        public void BuyBeatrix()
        {
            if (!GameDataSystem.LoadData().BeatrixBuying)
            {
                GameDataSystem.SaveData(data =>
                {
                    data.ClickForce += 20;
                    data.Money -= cost;
                    data.BeatrixBuying = true;
                });

                PlayerPrefs.SetInt("CurrentClickForce", GameDataSystem.LoadData().ClickForce);
                beatrixIsBuyingButton.interactable = false;
                bobsShopDefaultSprite.sprite = bobsShopIsAvailableSprite;
                beatrixPosition.AddComponent<Image>().sprite = beatrixSprite;
            }
            else
            {
                Debug.Log("ERROR: Beatrix is buy!!!");
            }
        }
    }
}