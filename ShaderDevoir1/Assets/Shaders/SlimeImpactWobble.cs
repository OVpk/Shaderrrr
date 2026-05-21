using UnityEngine;

public class SlimeImpactWobble : MonoBehaviour
{
    public Renderer characterRenderer;

    public float impactSensitivity = 0.5f;
    public float maxWobble = 10f;
    public float wobbleDecaySpeed = 5f;
    public string shaderPropertyName = "_WobbleIntensity";

    private Material _instancedMaterial;
    private float _currentWobble = 0f;

    void Start()
    {
        _instancedMaterial = characterRenderer.material;
    }

    void Update()
    {
        if (_instancedMaterial == null) return;

        if (_currentWobble > 0.001f)
        {
            _currentWobble = Mathf.Lerp(_currentWobble, 0f, Time.deltaTime * wobbleDecaySpeed);
            _instancedMaterial.SetFloat(shaderPropertyName, _currentWobble);
        }
        else if (_currentWobble != 0f)
        {
            _currentWobble = 0f;
            _instancedMaterial.SetFloat(shaderPropertyName, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BOUM ! J'ai touché : " + collision.gameObject.name + " avec une force de : " + collision.relativeVelocity.magnitude);
        float impactForce = collision.relativeVelocity.magnitude;

        _currentWobble += impactForce * impactSensitivity;

        _currentWobble = Mathf.Clamp(_currentWobble, 0f, maxWobble);
    }

    void OnDestroy()
    {
        if (_instancedMaterial != null)
        {
            Destroy(_instancedMaterial);
        }
    }
}