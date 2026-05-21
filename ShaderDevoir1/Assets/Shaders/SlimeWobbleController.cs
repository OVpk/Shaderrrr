using UnityEngine;

public class SlimeWobbleController : MonoBehaviour
{
    public Renderer characterRenderer;
    public string shaderPropertyName = "_WobbleIntensity";
    
    public float attackSpeed = 25f; 
    
    public float decaySpeed = 5f;

    private Material _instancedMaterial;
    private float _currentWobble = 0f;
    private float _targetPeak = 0f;
    private bool _isRising = false;

    void Start()
    {
        if (characterRenderer != null)
        {
            _instancedMaterial = characterRenderer.material;
        }
    }

    void Update()
    {
        if (_instancedMaterial == null) return;

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
            if (_currentWobble > 0.001f)
            {
                _currentWobble = Mathf.Lerp(_currentWobble, 0f, Time.deltaTime * decaySpeed);
            }
            else if (_currentWobble != 0f)
            {
                _currentWobble = 0f;
            }
        }

        _instancedMaterial.SetFloat(shaderPropertyName, _currentWobble);
    }

    public void TriggerWobble(float targetIntensity)
    {
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