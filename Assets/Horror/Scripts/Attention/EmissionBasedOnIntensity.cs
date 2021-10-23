using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Attention
{
    [RequireComponent(typeof(MeshRenderer))]
    public class EmissionBasedOnIntensity : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private new Light light = null;

        #endregion

        private MeshRenderer meshRenderer;

        private Color maxEmissionColor;
        private float maxLightIntensity;

        [Inject]
        private void Inject()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.EnableKeyword("_EMISSION");
            //meshRenderer.material.color = Color.black;
            //meshRenderer.material.SetColor("_EmissionColor", Color.black);
            maxEmissionColor = meshRenderer.material.GetColor("_EmissionColor");
            maxLightIntensity = light.intensity;
        }

        private void Update()
        {
            Color currentColor = Color.Lerp(Color.black, maxEmissionColor, light.intensity / maxLightIntensity);
            meshRenderer.material.SetColor("_EmissionColor", currentColor);
        }
    }
    
    [Serializable]
    public class UnityEventEmissionBasedOnIntensity : UnityEvent<EmissionBasedOnIntensity> { }
}