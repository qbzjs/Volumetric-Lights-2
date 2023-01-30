using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        #region Singleton
        
        public static DialogManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        #endregion

        public TextMeshProUGUI TextMeshPro;
        public GameObject DialogUIGameObject;
        public Image PressButtonImage;

        public void ToggleDialog(bool setActive)
        {
            DialogUIGameObject.SetActive(setActive);
        }
    }
}