using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public TMP_Text moneyText;
        public Image statueButton;
        public Image coinClickEffect;
        public Image coinButton;
        public Image upgradeButton;
        public Image clickUpgradeButton;
        public Image gardenButton;
        public Image bg;
        public Image textBox;

        public Sprite[] bgs;
        public Sprite[] coins;
        public Sprite[] coinEffects;
        public Sprite[] textBoxes;

        public Sprite[] chickenSprites;
        public Sprite[] slimeSprites;
        
        public GameObject[] chickenPositions;
        public GameObject[] slimePositions;
    }
}