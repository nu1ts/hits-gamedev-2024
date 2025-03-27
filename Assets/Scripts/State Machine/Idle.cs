using UnityEngine;

namespace State_Machine
{
    public class Idle : State
    {
        [Header("Animation Clips")]
        public AnimationClip anim;
        public AnimationClip animWithGun;
        private bool _isGunEquipped;
        public override void Enter()
        {
            PlayCorrectAnimation();
        }

        public override void Do()
        {
            var isGunEquippedNow = core.gunDetector.isGunEquipped;
            if (isGunEquippedNow != _isGunEquipped)
            {
                _isGunEquipped = isGunEquippedNow;
                PlayCorrectAnimation();
            }
            
            IsComplete = true; 
        }

        public override void Exit()
        {
            
        }
        
        private void PlayCorrectAnimation()
        {
            _isGunEquipped = core.gunDetector.isGunEquipped;
            var animToPlay = _isGunEquipped && animWithGun ? animWithGun : anim;
            Animator.Play(animToPlay.name);
        }
    }
}