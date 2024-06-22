using System.Collections;
using UnityEngine;
using UI;
using UnityEngine.UI;

namespace Systems
{
    public class GardenSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource music;
        
        [SerializeField] private UIManager uiManager;
        [SerializeField] private ClickSystem clickSystem;
        [SerializeField] private Sprite fullMoneySprite;

        private bool _canClick;
        private Sprite _defaultGardenSprite;
        private Button _gardenButton;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (GetComponent<Image>() != null)
            {
                _defaultGardenSprite = GetComponent<Image>().sprite;
                _gardenButton = GetComponent<Button>();
                _gardenButton.targetGraphic = GetComponent<Image>();
                _gardenButton.onClick.AddListener(ClickGarden);
                StartCoroutine(GardenMoney());
            }
        }

        public void ClickGarden()
        {
            if (!_canClick)
            {
                return;
            }
            else
            {
                music.Play();
                var data = GameDataSystem.LoadData();
                switch (data.GardenUpgradeLevel)
                {
                    case 1:
                        GameDataSystem.SaveData(d => { d.Money += 500; });
                        break;
                    case 2:
                        GameDataSystem.SaveData(d => { d.Money += 1000; });
                        break;
                    default:
                        Debug.Log("Error in GardenSystem");
                        break;
                }

                GetComponent<Image>().sprite = _defaultGardenSprite;
                _canClick = false;
                _gardenButton.interactable = false;
                uiManager.moneyText.text = GameDataSystem.LoadData().Money.ToString();
                clickSystem.money = GameDataSystem.LoadData().Money;
            }
        }

        public IEnumerator GardenMoney()
        {
            if (GetComponent<Image>() != null)
            {
                var image = GetComponent<Image>();
                _gardenButton = GetComponent<Button>();
                _gardenButton.targetGraphic = image;
                _defaultGardenSprite = image.sprite;
                _gardenButton.interactable = false;

                while (true)
                {
                    yield return new WaitForSeconds(120);
                    image.sprite = fullMoneySprite;
                    _canClick = true;
                    _gardenButton.interactable = true;
                    yield return new WaitUntil(() => !_canClick);
                }
            }
        }
    }
}