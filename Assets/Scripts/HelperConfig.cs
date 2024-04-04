using UnityEngine;

[CreateAssetMenu(menuName = "HelperConfig/HelperConfig", fileName = "HelperConfig")]
public class HelperConfig : ScriptableObject
{
    [Header("Parameters")]
    [SerializeField]
    private float shadowDistance;

    [Header("Materials")]
    [SerializeField]
    private Material buidingsMat;

    [SerializeField]
    private Material terrain0Mat;

    [SerializeField]
    private Material terrain1Mat;

    [SerializeField]
    private Material terrain2Mat;

    [SerializeField]
    private Material treesMat;
    
    [SerializeField]
    private Material m113Mat;

    private static readonly int ShadowDistance = Shader.PropertyToID("_ShadowDistance");

    private void OnValidate()
    {
        buidingsMat.SetFloat(ShadowDistance, shadowDistance);
        terrain0Mat.SetFloat(ShadowDistance, shadowDistance);
        terrain1Mat.SetFloat(ShadowDistance, shadowDistance);
        terrain2Mat.SetFloat(ShadowDistance, shadowDistance);
        treesMat.SetFloat(ShadowDistance, shadowDistance);
        m113Mat.SetFloat(ShadowDistance, shadowDistance);
    }
}