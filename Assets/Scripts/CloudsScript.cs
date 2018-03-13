// https://github.com/Flafla2/Generic-Raymarch-Unity
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Clouds")]
public class CloudsScript : SceneViewFilter
{
    [Range(0.0f, 1.0f)]
    public float testFloat = 1.0f;
    [Range(0.0f, 1.0f)]
    public float testFloat2 = 1.0f;
    public Gradient testGradient;
    private Vector4 _testGradient;

    [HeaderAttribute("Debugging")]
    public bool debugNoLowFreqNoise = false;
    public bool debugNoHighFreqNoise = false;
    public bool debugDensityOnly = false;

    [HeaderAttribute("System")]
    public Shader cloudShader;
    public Shader postProcessCloudsShader;

    [HeaderAttribute("Performance")]
    [Range(1, 256)]
    public int steps = 128;
    public bool adjustDensity = true;
    public AnimationCurve stepDensityAdjustmentCurve = new AnimationCurve(new Keyframe(0.0f, 3.019f), new Keyframe(0.25f, 1.233f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.892f));
    public bool allowFlyingInClouds = false;
    public Texture2D blueNoiseTexture;

    [HeaderAttribute("Cloud modeling")]
    public Texture2D weatherTexture;
    public Texture2D curlNoise;
    public float startHeight = 1500.0f;
    public float thickness = 4000.0f;
    public float planetSize = 35000.0f;
    public Vector3 planetZeroCoordinate = new Vector3(0.0f, 0.0f, 0.0f);
    [Range(0.0f, 1.0f)]
    public float scale = 0.128f;
    [Range(0.0f, 32.0f)]
    public float erasionScale = 18.7f;
    [Range(0.0f, 1.0f)]
    public float lowFreqMin = 0.366f;
    [Range(0.0f, 1.0f)]
    public float lowFreqMax = 0.8f;
    [Range(0.0f, 1.0f)]
    public float highFreqModifier = 0.21f;
    [Range(0.0f, 10.0f)]
    public float curlDistortScale = 1.78f;
    [Range(0.0f, 1000.0f)]
    public float curlDistortAmount = 407.0f;
    [Range(0.0f, 1.0f)]
    public float weatheScale = 0.1f;
    [Range(0.0f, 2.0f)]
    public float coverage = 0.92f;
    [Range(0.0f, 2.0f)]
    public float cloudSampleMultiplier = 1.0f;

    [HeaderAttribute("Cloud Lighting")]
    public Light sunLight;
    public Color cloudBaseColor = new Color32(199, 220, 255, 255);
    public Color cloudTopColor = new Color32(255, 255, 255, 255);
    [Range(0.0f, 1.0f)]
    public float ambientLightFactor = 0.551f;
    [Range(0.0f, 1.0f)]
    public float sunLightFactor = 0.79f;
    public Color highSunColor = new Color32(255, 252, 210, 255);
    public Color lowSunColor = new Color32(255, 174, 0, 255);
    [Range(0.0f, 1.0f)]
    public float henyeyGreensteinGForward = 0.4f;
    [Range(0.0f, 1.0f)]
    public float henyeyGreensteinGBackward = 0.179f;
    [Range(0.0f, 200.0f)]
    public float lightStepLength = 64.0f;
    [Range(0.0f, 1.0f)]
    public float lightConeRadius = 0.4f;
    [Range(0.0f, 4.0f)]
    public float density = 1.0f;

    [HeaderAttribute("Animating")]
    public float globalMultiplier = 1.0f;
    public float windSpeed = 25.7f;
    public float windDirection = 0.0f;
    public float coverageWindSpeed = 0.0f;
    public float coverageWindDirection = 0.0f;
    private Vector3 _windOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 _coverageWindOffset = new Vector3(0.0f, 0.0f);
    private Vector3 _windDirectionVector;
    private float _multipliedWindSpeed;

    private Texture3D _cloudShapeTexture;
    private Texture3D _cloudErasionTexture;
    private RenderTexture _cloudRenderTexture;

    public Material EffectMaterial
    {
        get
        {
            if (!_EffectMaterial && cloudShader)
            {
                _EffectMaterial = new Material(cloudShader);
                _EffectMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _EffectMaterial;
        }
    }
    private Material _EffectMaterial;

    public Material UpscaleMaterial
    {
        get
        {
            if (!_UpscaleMaterial && postProcessCloudsShader)
            {
                _UpscaleMaterial = new Material(postProcessCloudsShader);
                _UpscaleMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _UpscaleMaterial;
        }
    }
    private Material _UpscaleMaterial;

    void Start()
    {
        if (_EffectMaterial)
            DestroyImmediate(_EffectMaterial);
        createRenderTexture();
    }

    private void OnDestroy()
    {
        if (_EffectMaterial)
            DestroyImmediate(_EffectMaterial);
        destroyRenderTexture();
    }

    public Camera CurrentCamera
    {
        get
        {
            if (!_CurrentCamera)
                _CurrentCamera = GetComponent<Camera>();
            return _CurrentCamera;
        }
    }
    private Camera _CurrentCamera;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Matrix4x4 corners = GetFrustumCorners(CurrentCamera);
        Vector3 pos = CurrentCamera.transform.position;

        for (int x = 0; x < 4; x++)
        {
            corners.SetRow(x, CurrentCamera.cameraToWorldMatrix * corners.GetRow(x));
            Gizmos.DrawLine(pos, pos + (Vector3)(corners.GetRow(x)));
        }

        /*
        // UNCOMMENT TO DEBUG RAY DIRECTIONS
        Gizmos.color = Color.red;
        int n = 10; // # of intervals
        for (int x = 1; x < n; x++) {
            float i_x = (float)x / (float)n;

            var w_top = Vector3.Lerp(corners.GetRow(0), corners.GetRow(1), i_x);
            var w_bot = Vector3.Lerp(corners.GetRow(3), corners.GetRow(2), i_x);
            for (int y = 1; y < n; y++) {
                float i_y = (float)y / (float)n;
                
                var w = Vector3.Lerp(w_top, w_bot, i_y).normalized;
                Gizmos.DrawLine(pos + (Vector3)w, pos + (Vector3)w * 1.2f);
            }
        }
        */   
    }

    private void createRenderTexture()
    {
        if (_cloudRenderTexture == null)
        {
            _cloudRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, CurrentCamera.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            _cloudRenderTexture.filterMode = FilterMode.Bilinear;
            _cloudRenderTexture.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private void destroyRenderTexture()
    {
        DestroyImmediate(_cloudRenderTexture);
        _cloudRenderTexture = null;
    }

    private Vector4 gradientToVector4( Gradient gradient )
    {
        if (gradient.colorKeys.Length != 4)
        {
            return new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        }
        float x = gradient.colorKeys[0].time;
        float y = gradient.colorKeys[1].time;
        float z = gradient.colorKeys[2].time;
        float w = gradient.colorKeys[3].time;
        return new Vector4(x, y, z, w);
    }

    [ImageEffectOpaque]
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!EffectMaterial == null || weatherTexture == null || curlNoise == null || blueNoiseTexture == null)
        {
            Graphics.Blit(source, destination); // do nothing
            return;
        }

        if (_cloudShapeTexture == null)
        {
            _cloudShapeTexture = TGALoader.load3DFromTGASlices("Assets/Textures/noiseShapePacked.tga");
        }

        if (_cloudErasionTexture == null)
        {
            _cloudErasionTexture = TGALoader.load3DFromTGASlices("Assets/Textures/noiseErosionPacked.tga");
        }

        // Set any custom shader variables here.  For example, you could do:
        // EffectMaterial.SetFloat("_MyVariable", 13.37f);
        // This would set the shader uniform _MyVariable to value 13.37
        Vector3 cameraPos = CurrentCamera.transform.position;
        // sunLight.rotation.x 364 -> 339, 175 -> 201

        float sunLightFactorUpdated = sunLightFactor;
        float ambientLightFactorUpdated = ambientLightFactor;
        float sunAngle = sunLight.transform.eulerAngles.x;
        Color sunColor = highSunColor;
        Color cloudTopColorMix = cloudBaseColor;
        float henyeyGreensteinGBackwardLerp = henyeyGreensteinGBackward;

        if (sunAngle > 180.0f)
        {
            float gradient = Mathf.Max(0.0f, (sunAngle - 330.0f) / 30.0f);
            float gradient2 = gradient * gradient;
            sunLightFactorUpdated *= gradient;
            ambientLightFactorUpdated *= gradient;
            henyeyGreensteinGBackwardLerp *= gradient2 * gradient;
            ambientLightFactorUpdated = Mathf.Max(0.02f, ambientLightFactorUpdated);
            sunColor = Color.Lerp(lowSunColor, highSunColor, gradient2);
            cloudTopColorMix = Color.Lerp(lowSunColor, cloudTopColor, gradient);
        }

        updateMaterialKeyword(debugNoLowFreqNoise, "DEBUG_NO_LOW_FREQ_NOISE");
        updateMaterialKeyword(debugNoHighFreqNoise, "DEBUG_NO_HIGH_FREQ_NOISE");
        updateMaterialKeyword(debugDensityOnly, "DEBUG_DENSITY");
        updateMaterialKeyword(allowFlyingInClouds, "ALLOW_IN_CLOUDS");

        EffectMaterial.SetVector("_SunDir", sunLight.transform ? (-sunLight.transform.forward).normalized : Vector3.up);
        EffectMaterial.SetVector("_PlanetCenter", planetZeroCoordinate - new Vector3(0, planetSize, 0));
        EffectMaterial.SetColor("_SunColor", sunColor);

        EffectMaterial.SetColor("_CloudBaseColor", cloudBaseColor);
        EffectMaterial.SetColor("_CloudTopColor", cloudTopColor);
        EffectMaterial.SetFloat("_AmbientLightFactor", ambientLightFactorUpdated);
        EffectMaterial.SetFloat("_SunLightFactor", sunLightFactorUpdated);

        EffectMaterial.SetTexture("_ShapeTexture", _cloudShapeTexture);
        EffectMaterial.SetTexture("_ErasionTexture", _cloudErasionTexture);
        EffectMaterial.SetTexture("_WeatherTexture", weatherTexture);
        EffectMaterial.SetTexture("_CurlNoise", curlNoise);
        EffectMaterial.SetTexture("_BlueNoise", blueNoiseTexture);
        EffectMaterial.SetVector("_Randomness", new Vector4(Random.value, Random.value, Random.value, Random.value));

        EffectMaterial.SetFloat("_CurlDistortAmount", curlDistortAmount);
        EffectMaterial.SetFloat("_CurlDistortScale", curlDistortScale); 

            
        EffectMaterial.SetFloat("_LightConeRadius", lightConeRadius);
        EffectMaterial.SetFloat("_LightStepLength", lightStepLength);
        EffectMaterial.SetFloat("_SphereSize", planetSize);
        EffectMaterial.SetVector("_CloudHeightMinMax", new Vector2(startHeight, startHeight + thickness));
        EffectMaterial.SetFloat("_Thickness", thickness);
        EffectMaterial.SetFloat("_Scale", 0.0001f + scale * 0.001f);
        EffectMaterial.SetFloat("_ErasionScale", erasionScale);
        EffectMaterial.SetVector("_LowFreqMinMax", new Vector4(lowFreqMin, lowFreqMax));
        EffectMaterial.SetFloat("_HighFreqModifier", highFreqModifier);
        EffectMaterial.SetFloat("_WeatherScale", weatheScale * 0.001f);
        EffectMaterial.SetFloat("_Coverage", 1 - coverage);
        EffectMaterial.SetFloat("_HenyeyGreensteinGForward", henyeyGreensteinGForward);
        EffectMaterial.SetFloat("_HenyeyGreensteinGBackward", -henyeyGreensteinGBackwardLerp);
        EffectMaterial.SetFloat("_SampleMultiplier", cloudSampleMultiplier);

        EffectMaterial.SetFloat("_Density", density);

        EffectMaterial.SetFloat("_WindSpeed", _multipliedWindSpeed);
        EffectMaterial.SetVector("_WindDirection", _windDirectionVector);
        EffectMaterial.SetVector("_WindOffset", _windOffset);
        EffectMaterial.SetVector("_CoverageWindOffset", _coverageWindOffset);

        // Test uniforms
        EffectMaterial.SetFloat("_TestFloat", testFloat);
        EffectMaterial.SetFloat("_TestFloat2", testFloat2);
        EffectMaterial.SetVector("_TestGradient", gradientToVector4(testGradient));

        EffectMaterial.SetInt("_Steps", steps);
        if (adjustDensity)
        {
            EffectMaterial.SetFloat("_InverseStep", stepDensityAdjustmentCurve.Evaluate(steps / 256.0f));
        }
        else
        {
            EffectMaterial.SetFloat("_InverseStep", 1.0f);
        }
        
        EffectMaterial.SetMatrix("_FrustumCornersES", GetFrustumCorners(CurrentCamera));
        EffectMaterial.SetMatrix("_CameraInvViewMatrix", CurrentCamera.cameraToWorldMatrix);
        EffectMaterial.SetVector("_CameraWS", cameraPos);
        EffectMaterial.SetFloat("_FarPlane", CurrentCamera.farClipPlane);

        CustomGraphicsBlit(source, destination, EffectMaterial, 0);
        //CustomGraphicsBlit(source, _cloudRenderTexture, EffectMaterial, 0);
        //Graphics.Blit(_cloudRenderTexture, destination, UpscaleMaterial);
    }
    private void Update()
    {
        //steps = 6 + ((int) ((Mathf.Sin(Time.time / 1f) + 1f) / 2f * 250f));
        _multipliedWindSpeed = windSpeed * globalMultiplier;
        float angleWind = windDirection * Mathf.Deg2Rad;
        _windDirectionVector = new Vector3(Mathf.Cos(angleWind), -0.01f, Mathf.Sin(angleWind));
        _windOffset += _multipliedWindSpeed * _windDirectionVector * Time.deltaTime;

        float angleCoverage = coverageWindDirection * Mathf.Deg2Rad;
        Vector2 coverageDirecton = new Vector2(Mathf.Cos(angleCoverage), Mathf.Sin(angleCoverage));
        _coverageWindOffset += coverageWindSpeed * globalMultiplier * coverageDirecton;
    }

    private void updateMaterialKeyword(bool b, string keyword)
    {
        if (b)
        {
            EffectMaterial.EnableKeyword(keyword);
        }
        else
        {
            EffectMaterial.DisableKeyword(keyword);
        }
    }

    /// \brief Stores the normalized rays representing the camera frustum in a 4x4 matrix.  Each row is a vector.
    /// 
    /// The following rays are stored in each row (in eyespace, not worldspace):
    /// Top Left corner:     row=0
    /// Top Right corner:    row=1
    /// Bottom Right corner: row=2
    /// Bottom Left corner:  row=3
    private Matrix4x4 GetFrustumCorners(Camera cam)
    {
        float camFov = cam.fieldOfView;
        float camAspect = cam.aspect;

        Matrix4x4 frustumCorners = Matrix4x4.identity;

        float fovWHalf = camFov * 0.5f;

        float tan_fov = Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 toRight = Vector3.right * tan_fov * camAspect;
        Vector3 toTop = Vector3.up * tan_fov;

        Vector3 topLeft = (-Vector3.forward - toRight + toTop);
        Vector3 topRight = (-Vector3.forward + toRight + toTop);
        Vector3 bottomRight = (-Vector3.forward + toRight - toTop);
        Vector3 bottomLeft = (-Vector3.forward - toRight - toTop);

        frustumCorners.SetRow(0, topLeft);
        frustumCorners.SetRow(1, topRight);
        frustumCorners.SetRow(2, bottomRight);
        frustumCorners.SetRow(3, bottomLeft);

        return frustumCorners;
    }

    /// \brief Custom version of Graphics.Blit that encodes frustum corner indices into the input vertices.
    /// 
    /// In a shader you can expect the following frustum cornder index information to get passed to the z coordinate:
    /// Top Left vertex:     z=0, u=0, v=0
    /// Top Right vertex:    z=1, u=1, v=0
    /// Bottom Right vertex: z=2, u=1, v=1
    /// Bottom Left vertex:  z=3, u=1, v=0
    /// 
    /// \warning You may need to account for flipped UVs on DirectX machines due to differing UV semantics
    ///          between OpenGL and DirectX.  Use the shader define UNITY_UV_STARTS_AT_TOP to account for this.
    static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
    {
        RenderTexture.active = dest;

        fxMaterial.SetTexture("_MainTex", source);
        
        GL.PushMatrix();
        GL.LoadOrtho(); // Note: z value of vertices don't make a difference because we are using ortho projection
        
        fxMaterial.SetPass(passNr);

        GL.Begin(GL.QUADS);

        // Here, GL.MultitexCoord2(0, x, y) assigns the value (x, y) to the TEXCOORD0 slot in the shader.
        // GL.Vertex3(x,y,z) queues up a vertex at position (x, y, z) to be drawn.  Note that we are storing
        // our own custom frustum information in the z coordinate.
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

        GL.End();
        GL.PopMatrix();
    }
}
