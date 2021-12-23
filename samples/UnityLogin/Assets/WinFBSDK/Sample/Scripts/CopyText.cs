using UnityEngine;
using UnityEngine.UI;

namespace DVS.Utils
{
    [RequireComponent(typeof(Text))]
    public class CopyText : MonoBehaviour
    {
        private Text textToCopy;

        private void Awake()
        {
            textToCopy = GetComponent<Text>();
        }

        public void CopyToClipboard()
        {
            GUIUtility.systemCopyBuffer = textToCopy.text;
            Debug.Log("TEXT COPIED!");
            Login.loggerUnity.Invoke("Text Copied!");
        }
    }
}