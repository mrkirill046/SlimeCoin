using UnityEngine;

namespace Other
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private static DontDestroyOnLoad _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}