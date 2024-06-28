using UnityEngine;

namespace State_Machine
{
    public class Run : State
    {
        [Header("Animation Clips")]
        public AnimationClip anim;
        public AnimationClip animWithGun;
        private float _maxAnimationSpeed;
        private bool _isGunEquipped;

        public override void Enter()
        {
            PlayCorrectAnimation();
        }

        public override void Do()
        {
            var currentSpeed = Body.velocity.magnitude;
            Animator.speed = Helpers.Map(currentSpeed, 0, 1, 0, _maxAnimationSpeed, true);
            
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
            Animator.speed = 1f;
        }

        private void PlayCorrectAnimation()
        {
            _isGunEquipped = core.gunDetector.isGunEquipped;
            var animToPlay = _isGunEquipped && animWithGun ? animWithGun : anim;
            Animator.Play(animToPlay.name);
            _maxAnimationSpeed = 1f / animToPlay.length;
        }
    }
}