using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickSystem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    
    private UIManager _uiManager;
    private int _money;
    private GameDataSystem.GameData _data;

    public void Initialize()
    {
        _uiManager = FindFirstObjectByType<UIManager>();
        
        _data = GameDataSystem.LoadData();
        _money = _data.money;
        _uiManager.moneyText.text = _data.money.ToString();
    }

    private void LateUpdate()
    {
        _uiManager.coinClickEffect.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void ClickMoneyButton()
    {
        _money += _data.clickForce;
        _uiManager.moneyText.text = _money.ToString();

        StartCoroutine(ClickEffect());

        GameDataSystem.GameData newData = new GameDataSystem.GameData
        {
            money = _money,
            clickForce = _data.clickForce
        };
        
        GameDataSystem.SaveData(newData);
    }

    private IEnumerator ClickEffect()
    {
        _uiManager.coinClickEffect.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        _uiManager.coinClickEffect.GetComponent<Image>().enabled = false;
    }
}
