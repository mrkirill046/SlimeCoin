using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class PassiveClickSystem : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;

        private Transform _thisObject;
        private Coroutine _clickCoroutine;

        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private float targetScale = 2f;

        private void Start()
        {
            _thisObject = transform;
            StartCoroutine(StartClickAnimation());
        }

        private void Update()
        {
            if (gameObject.GetComponent<Image>() != null && _clickCoroutine == null)
            {
                _clickCoroutine = StartCoroutine(Click());
            }
            else if (gameObject.GetComponent<Image>() == null && _clickCoroutine != null)
            {
                StopCoroutine(_clickCoroutine);
                _clickCoroutine = null;
            }
        }

        private IEnumerator Click()
        {
            while (true)
            {
                GameDataSystem.GameData data = GameDataSystem.LoadData();
                GameDataSystem.SaveData(newData =>
                {
                    newData.Money = data.Money + data.ClickForce;
                });
                
                uiManager.moneyText.text = GameDataSystem.LoadData().Money.ToString();
                yield return new WaitForSeconds(animationDuration * 2);
            }
        }

        private IEnumerator StartClickAnimation()
        {
            Vector3 initialScale = _thisObject.localScale;
            Vector3 targetScaleVector = initialScale / targetScale;

            while (true)
            {
                float elapsedTime = 0f;
                while (elapsedTime < animationDuration)
                {
                    _thisObject.localScale = Vector3.Lerp(initialScale, targetScaleVector, elapsedTime / animationDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                _thisObject.localScale = targetScaleVector;

                elapsedTime = 0f;
                while (elapsedTime < animationDuration)
                {
                    _thisObject.localScale = Vector3.Lerp(targetScaleVector, initialScale, elapsedTime / animationDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                _thisObject.localScale = initialScale;
            }
        }
    }
}
