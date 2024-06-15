using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ClickSystem clickSystem;
    [SerializeField] private SceneChanger sceneChanger;

    private void Awake()
    {
        GameDataSystem.GameData data = GameDataSystem.LoadData();
        
        if (data == null)
        {
            GameDataSystem.GameData defaultData  = new GameDataSystem.GameData
            {
                money = 0,
                clickForce = 1
            };
        
            GameDataSystem.SaveData(defaultData);
        }
        
        if (clickSystem != null) clickSystem.Initialize();
        if (sceneChanger != null) sceneChanger.Initialize();
    }
}