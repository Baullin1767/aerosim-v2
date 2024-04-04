using UnityEngine;

namespace UI.Game
{
    public class Grain : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer grainTexture;

        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        /// <summary>
        /// Best start 0.25, End - 0.85
        /// </summary>
        /// <param name="intensity"></param>
        public void SetIntensity(float intensity)
        {
            if (this == null || gameObject == null) return;
            var correctValue = Mathf.Lerp(0.15f, 1f, intensity);
            gameObject.SetActive(intensity > 0.05f);
            grainTexture.material.SetFloat(Alpha, correctValue);
        }
    }
}