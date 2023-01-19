namespace Singleton
{
    using UnityEngine;

    public class SingletonClass : MonoBehaviour
    {
        [SerializeField] private bool _stayBetweenScenes;
        public static SingletonClass Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;

            if (_stayBetweenScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}