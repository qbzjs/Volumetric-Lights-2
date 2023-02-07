using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class TypeWriter : MonoBehaviour
    {
        public float Delay = 0.1f;
        public string FullText;
        private string CurrentText = string.Empty;
        public TMP_Text DisplayText;

        public IEnumerator ShowText()
        {
            for (int i = 0; i <= FullText.Length; i++)
            {
                CurrentText = FullText.Substring(0, i);
                DisplayText.text = CurrentText;
                yield return new WaitForSeconds(Delay);
            }
        }
    }
}