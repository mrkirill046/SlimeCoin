using UnityEngine;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}