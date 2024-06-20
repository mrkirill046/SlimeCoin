using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Systems
{
    public class BoostSystem : MonoBehaviour
    {
        [SerializeField] private Rotation moneyRotation;
        [SerializeField] private long cost;

        [SerializeField] private Sprite boostSprite;
        [SerializeField] private Sprite superBoostSprite;
        [SerializeField] private Sprite superBoostEffectSprite;
        [SerializeField] private Sprite boostEffectSprite;

        [Space(10)] [SerializeField] private Image money;
        [SerializeField] private Image moneyEffect;

        [SerializeField] private BoostSystem boostSystem;
        [SerializeField] private BoostSystem superBoostSystem;

        [SerializeField] private bool isSuperBoost;
        
        private Sprite _defaultSprite;
        private Sprite _defaultEffectSprite;
        private int _startForce;

        public void ClickBuyBoost()
        {
            if (GameDataSystem.LoadData().Money < cost)
            {
                Debug.Log("Now money for buy boost");
            }
            else
            {
                GameDataSystem.SaveData(data => { data.Money -= cost; });

                moneyRotation.rotationSpeed *= 4;
                StartCoroutine(StartBoost(30, 2));
            }
        }

        public void ClickBuySuperBoost()
        {
            if (GameDataSystem.LoadData().Money < cost)
            {
                Debug.Log("Now money for buy boost");
            }
            else
            {
                GameDataSystem.SaveData(data => { data.Money -= cost; });

                moneyRotation.rotationSpeed *= 4;
                StartCoroutine(StartBoost(30, 3));
            }
        }

        private IEnumerator StartBoost(int sec, int x)
        {
            _defaultSprite = money.sprite;
            _defaultEffectSprite = moneyEffect.sprite;
            _startForce = GameDataSystem.LoadData().ClickForce;
            PlayerPrefs.SetInt("CurrentClickForce", GameDataSystem.LoadData().ClickForce);

            GameDataSystem.SaveData(data => { data.ClickForce *= x; });

            GetComponent<Button>().interactable = false;

            if (isSuperBoost)
            {
                money.sprite = superBoostSprite;
                moneyEffect.sprite = superBoostEffectSprite;
                boostSystem.GetComponent<Button>().interactable = false;
            }
            else
            {
                money.sprite = boostSprite;
                moneyEffect.sprite = boostEffectSprite;
                superBoostSystem.GetComponent<Button>().interactable = false;
            }

            yield return new WaitForSeconds(sec);

            if (isSuperBoost)
            {
                boostSystem.GetComponent<Button>().interactable = true;
            }
            else
            {
                superBoostSystem.GetComponent<Button>().interactable = true;
            }

            money.sprite = _defaultSprite;
            moneyEffect.sprite = _defaultEffectSprite;

            GetComponent<Button>().interactable = true;

            GameDataSystem.SaveData(data => { data.ClickForce = _startForce; });

            moneyRotation.rotationSpeed /= 4;
        }

        private void OnDestroy()
        {
            GameDataSystem.SaveData(data =>
            {
                data.ClickForce = PlayerPrefs.GetInt("CurrentClickForce");
            });
        }
    }
}