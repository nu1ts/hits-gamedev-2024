using System.Collections;
using UnityEngine;

namespace State_Machine
{
    public class AttackKnife : State
    {
        [Header("Animation Clip")]
        public AnimationClip attack;

        public override void Enter()
        {
            Animator.Play(attack.name);
        }
    }
}