using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Attention
{
    [DisallowMultipleComponent]
    public class RotationSway : MonoBehaviour
    {
        #region Inspector

        public float intensityMultiplier = 10f;

        public float frequencyMultiplier = 1f;

        public float period = 2.6f;

        public float timeOffset = 0;

        public float seedX = 0f;

        public float seedY = 1f;

        #endregion

        private float time;

        private void Start()
        {
            time = timeOffset;
        }

        private void LateUpdate()
        {
            time += Time.deltaTime * frequencyMultiplier;

            transform.localRotation = Quaternion.Euler(
                GetRandom(seedX) * intensityMultiplier,
                GetRandom(seedY) * intensityMultiplier,
                0
            );
        }

        private float GetRandom(float seed)
        {
            float x = seed;
            float y = time;

            float val;

            //val = noise.snoise(new float2(x, y));
            val = noise.pnoise(new float2(x, y), period);

            //val = Mathf.Log(Mathf.Abs(val) + 1, 2) * Mathf.Sign(val);
            //float abs = Mathf.Abs(val);
            //val = abs * (1 - abs) * Mathf.Sign(val);

            return val;
            //return Mathf.PerlinNoise(x, y) * 2 - 1;
        }
    }
    
    [Serializable]
    public class UnityEventRotationSway : UnityEvent<RotationSway> { }
}