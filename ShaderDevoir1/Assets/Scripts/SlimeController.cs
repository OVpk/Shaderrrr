using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("General")]
    public Renderer characterRenderer;
    private Material _instancedMaterial;

    [Header("Melting")]
    [Range(0f, 100f)]
    public float melting = 0f;
    
    [Header("Wobble")]
    public float staticWobbleIntensity = 0f;
    
    public bool useDynamicWobble = true;
    public float attackSpeed = 25f; 
    public float decaySpeed = 5f;
    private float _currentWobble = 0f;
    private float _targetPeak = 0f;
    private bool _isRising = false;
    
    void Start()
    {
        if (characterRenderer != null && characterRenderer.sharedMaterials.Length > 0)
        {
            _instancedMaterial = new Material(characterRenderer.sharedMaterials[0]);

            Material[] unifiedMaterials = new Material[characterRenderer.sharedMaterials.Length];

            for (int i = 0; i < unifiedMaterials.Length; i++)
            {
                unifiedMaterials[i] = _instancedMaterial;
            }

            characterRenderer.materials = unifiedMaterials;
        }
    }

    void Update()
    {
        if (_instancedMaterial == null) return;

        if (useDynamicWobble)
        {
            if (_isRising)
            {
                _currentWobble = Mathf.Lerp(_currentWobble, _targetPeak, Time.deltaTime * attackSpeed);

                if (_currentWobble >= _targetPeak * 0.95f)
                {
                    _isRising = false;
                }
            }
            else
            {
                if (_currentWobble > 0.0001f)
                {
                    _currentWobble = Mathf.Lerp(_currentWobble, 0f, Time.deltaTime * decaySpeed);
                }
                else if (_currentWobble != 0f)
                {
                    _currentWobble = 0f;
                }
            }
        }
        else
        {
            _currentWobble = staticWobbleIntensity;
        }

        _instancedMaterial.SetFloat("_WobbleIntensity", _currentWobble);
        _instancedMaterial.SetFloat("_Melting_in", melting);
    }

    public void TriggerWobble(float targetIntensity)
    {
        useDynamicWobble = true; 
        
        _targetPeak = targetIntensity;
        _isRising = true;
    }

    [ContextMenu("Test")]
    public void TestWobbleFromEditor()
    {
        TriggerWobble(0.003f);
    }

    void OnDestroy()
    {
        if (_instancedMaterial != null)
        {
            Destroy(_instancedMaterial);
        }
    }
}