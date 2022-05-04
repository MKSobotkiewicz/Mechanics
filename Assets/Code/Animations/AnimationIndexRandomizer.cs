using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Animations
{
    public class AnimationIndexRandomizer:MonoBehaviour
    {
        public int MaxIndex;
        public string IndexName;
        [Range(0f,1f)]
        public float SpeedRandomness = 0.2f;

        private Animator animator;
        private static readonly System.Random random = new System.Random();

        private float timer = 0;

        public void Start()
        {
            animator = GetComponent<Animator>();
            if (animator is null)
            {
                Debug.LogError(name+" is missing animator.");
            }
            UpdateAnimator();
            animator.Update((float)random.NextDouble()*10+8f);
        }

        public void FixedUpdate()
        {
            timer -= UnityEngine.Time.fixedDeltaTime;
            if (timer < 0)
            {
                timer += 0.8f + (float)random.NextDouble()*0.4f;
                UpdateAnimator();
            }
        }

        public void UpdateAnimator()
        {
            animator.SetInteger(IndexName,random.Next(MaxIndex));
            animator.speed = (float)random.NextDouble()* SpeedRandomness *2+1- SpeedRandomness;
        }
    }
}
