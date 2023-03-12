using System;
using Character.Camera;
using Character.State;
using Kayak;
using SceneTransition;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {

        [Header("References")]
        public CharacterStateBase CurrentStateBase;
        public CameraManager CameraManagerRef;
        [Tooltip("Reference of the KayakController script")]
        public KayakController KayakController;
        [Tooltip("Reference of the InputManagement script")]
        public InputManagement InputManagement;
        [Tooltip("Reference of the paddle Animator")]
        public Animator PaddleAnimator;
        [Tooltip("Reference of the TransitionManager script")]
        public TransitionManager TransitionManager;

        [Header("Balance")]
        [SerializeField, Range(0, 1), Tooltip("The lerp value that reset the balance to 0 over time")]
        private float balanceLerpTo0Value = 0.01f;
        [ReadOnly, Tooltip("Can the balance lerp itself to 0 ?")]
        public bool LerpBalanceTo0 = true;
        [ReadOnly, Tooltip("The current balance value")]
        public float Balance = 0f;
        [Range(0, 40), Tooltip("The limit over which the player will go in unbalance state")]
        public float BalanceLimit = 10f;
        [Range(0, 40), Tooltip("The limit over which the player will die")]
        public float BalanceDeathLimit = 15f;
        [Range(0, 40), Tooltip("The angle the player has to reach when unbalanced to get back balanced")]
        public float RebalanceAngle = 8f;
        [Range(0, 10), Tooltip("Minimum Time/Balance the player has to react when unbalanced")]
        public float MinimumTimeUnbalanced = 2f;
        [Range(0,20), Tooltip("The Multiplier applied to the paddle force to the balance")]
        public float RotationToBalanceMultiplier = 10f;

        [Header("VFX")]
        [SerializeField] private ParticleSystem _paddleLeftParticle;
        [SerializeField] private ParticleSystem _paddleRightParticle;


        private void Awake()
        {
            CharacterNavigationState navigationState =
                new CharacterNavigationState(KayakController, InputManagement, this, this, CameraManagerRef);
            CurrentStateBase = navigationState;
        }

        private void Start()
        {
            CurrentStateBase.EnterState(this);
            CurrentStateBase.OnPaddleRight.AddListener(PlayPaddleRightParticle);
            CurrentStateBase.OnPaddleLeft.AddListener(PlayPaddleLeftParticle);
        }
        private void Update()
        {
            CurrentStateBase.UpdateState(this);

            BalanceManagement();
        }
        private void FixedUpdate()
        {
            CurrentStateBase.FixedUpdate(this);
        }
        public void SwitchState(CharacterStateBase stateBaseCharacter)
        {
            CurrentStateBase = stateBaseCharacter;
            stateBaseCharacter.EnterState(this);
        }

        /// <summary>
        /// Lerp the Balance value to 0 
        /// </summary>
        private void BalanceManagement()
        {
            if (LerpBalanceTo0)
            {
                Balance = Mathf.Lerp(Balance, 0, balanceLerpTo0Value);
            }
        }

        #region VFX

        private void PlayPaddleLeftParticle()
        {
            if (_paddleLeftParticle == null)
            {
                return;
            }
            _paddleLeftParticle.Play();
        }

        private void PlayPaddleRightParticle()
        {
            if (_paddleRightParticle == null)
            {
                return;
            }
            _paddleRightParticle.Play();
        }

        #endregion

        #region GUI

        private void OnGUI()
        {
            GUI.skin.label.fontSize = 50;

            GUI.color = Color.white;
            GUI.Label(new Rect(10, 10, 500, 100), "Balance : " + Math.Round(Balance, 1));
        }

        #endregion
    }
}
