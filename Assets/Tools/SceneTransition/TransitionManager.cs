using System;

namespace SceneTransition
{
    using UnityEngine;

    public class TransitionManager : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Transition type")] 
        
        [SerializeField, Tooltip("Choice of the transition type")]
        private TransitionType _startTransitionType;

        [Header("References")] 
        
        [Tooltip("Reference the fade image")]
        public Animator FadeImageAnimator;

        [Tooltip("Reference the size image")]
        public Animator SizeImageAnimator;

        [Tooltip("Reference the slide image")]
        public Animator SlideImageAnimator;
        
        #endregion

        #region Methods

        private void Awake()
        {
            SetupActiveTransition(_startTransitionType);
        }

        private void Update()
        {
           
        }

        private void SetupActiveTransition(TransitionType transitionType)
        {
            FadeImageAnimator.gameObject.SetActive(transitionType == TransitionType.Fade);
            SizeImageAnimator.gameObject.SetActive(transitionType == TransitionType.Size);
            SlideImageAnimator.gameObject.SetActive(transitionType == TransitionType.Slide);
        }

        public void LaunchTransitionIn(TransitionType transitionType)
        {
            SetupActiveTransition(transitionType);
            
            const string transitionInTriggerName = "TransitionIn";
            SetAnimatorsTrigger(transitionInTriggerName, transitionType);
        }
        
        public void LaunchTransitionOut(TransitionType transitionType)
        {
            SetupActiveTransition(transitionType);
            
            const string transitionInTriggerName = "TransitionOut";
            SetAnimatorsTrigger(transitionInTriggerName, transitionType);
        }

        private void SetAnimatorsTrigger(string triggerName, TransitionType transitionType)
        {
            switch (transitionType)
            {
                case TransitionType.Fade:
                    FadeImageAnimator.SetTrigger(triggerName);
                    break;
                case TransitionType.Size:
                    SizeImageAnimator.SetTrigger(triggerName);
                    break;
                case TransitionType.Slide:
                    SlideImageAnimator.SetTrigger(triggerName);
                    break;
            }
        }

        #endregion
    }
}