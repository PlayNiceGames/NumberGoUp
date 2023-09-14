using TMPro;
using UnityEngine;

namespace GameDebug
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        private void Update()
        {
            int fps = GetFps();
            _label.text = fps.ToString();
        }

        private int GetFps()
        {
            return Mathf.RoundToInt(1.0f / Time.unscaledDeltaTime);
        }
    }
}