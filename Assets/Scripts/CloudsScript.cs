// https://github.com/Flafla2/Generic-Raymarch-Unity
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera), typeof(CloudTemporalAntiAliasing))]
[AddComponentMenu("Effects/Clouds")]
public class CloudsScript : SceneViewFilter
{

    public enum RandomJitter
    {
        Off,
        Random,
        BlueNoise
    }

    [HeaderAttribute("Debugging")]
    public bool debugNoLowFreqNoise = false;
    public bool debugNoHighFreqNoise = false;
    public bool debugNoCurlNoise = false;

    [HeaderAttribute("Performance")]
    [Range(1, 256)]
    public int steps = 128;
    public bool adjustDensity = true;
    public AnimationCurve stepDensityAdjustmentCurve = new AnimationCurve(new Keyframe(0.0f, 3.019f), new Keyframe(0.25f, 1.233f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.892f));
    public bool allowFlyingInClouds = false;
    [Range(1, 8)]
    public int downSample = 1;
    public Texture2D blueNoiseTexture;
    public RandomJitter randomJitterNoise = RandomJitter.BlueNoise;
    public bool temporalAntiAliasing = true;

    [HeaderAttribute("Cloud modeling")]
    public Gradient gradientLow;
    public Gradient gradientMed;
    public Gradient gradientHigh;
    public Texture2D curlNoise;
    public TextAsset lowFreqNoise;
    public TextAsset highFreqNoise;
    public float startHeight = 1500.0f;
    public float thickness = 4000.0f;
    public float planetSize = 35000.0f;
    public Vector3 planetZeroCoordinate = new Vector3(0.0f, 0.0f, 0.0f);
    [Range(0.0f, 1.0f)]
    public float scale = 0.3f;
    [Range(0.0f, 32.0f)]
    public float erasionScale = 13.9f;
    [Range(0.0f, 1.0f)]
    public float lowFreqMin = 0.366f;
    [Range(0.0f, 1.0f)]
    public float lowFreqMax = 0.8f;
    [Range(0.0f, 1.0f)]
    public float highFreqModifier = 0.21f;
    [Range(0.0f, 10.0f)]
    public float curlDistortScale = 7.44f;
    [Range(0.0f, 1000.0f)]
    public float curlDistortAmount = 407.0f;
    [Range(0.0f, 1.0f)]
    public float weatheScale = 0.1f;
    [Range(0.0f, 2.0f)]
    public float coverage = 0.92f;
    [Range(0.0f, 2.0f)]
    public float cloudSampleMultiplier = 1.0f;

    [HeaderAttribute("High altitude clouds")]
    public Texture2D cloudsHighTexture;
    [Range(0.0f, 2.0f)]
    public float coverageHigh = 1.0f;
    [Range(0.0f, 2.0f)]
    public float highCoverageScale = 1.0f;
    [Range(0.0f, 1.0f)]
    public float highCloudsScale = 0.5f;

    [HeaderAttribute("Cloud Lighting")]
    public Light sunLight;
    public Color cloudBaseColor = new Color32(199, 220, 255, 255);
    public Color cloudTopColor = new Color32(255, 255, 255, 255);
    [Range(0.0f, 1.0f)]
    public float ambientLightFactor = 0.551f;
    [Range(0.0f, 1.5f)]
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
    public bool randomUnitSphere = true;
    [Range(0.0f, 4.0f)]
    public float density = 1.0f;
    public bool aLotMoreLightSamples = false;

    [HeaderAttribute("Animating")]
    public float globalMultiplier = 1.0f;
    public float windSpeed = 15.9f;
    public float windDirection = -22.4f;
    public float coverageWindSpeed = 25.0f;
    public float coverageWindDirection = 5.0f;
    public float highCloudsWindSpeed = 49.2f;
    public float highCloudsWindDirection = 77.8f;

    private Vector3 _windOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 _coverageWindOffset = new Vector3(0.0f, 0.0f);
    private Vector2 _highCloudsWindOffset = new Vector3(1500.0f, -900.0f);
    private Vector3 _windDirectionVector;
    private float _multipliedWindSpeed;

    private Texture3D _cloudShapeTexture;
    private Texture3D _cloudErasionTexture;

    private CloudTemporalAntiAliasing _temporalAntiAliasing;

    public Material CloudMaterial
    {
        get
        {
            if (!_CloudMaterial)
            {
                _CloudMaterial = new Material(Shader.Find("Hidden/Clouds"));
                _CloudMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _CloudMaterial;
        }
    }
    private Material _CloudMaterial;

    public Material UpscaleMaterial
    {
        get
        {
            if (!_UpscaleMaterial)
            {
                _UpscaleMaterial = new Material(Shader.Find("Hidden/CloudBlender"));
                _UpscaleMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _UpscaleMaterial;
        }
    }
    private Material _UpscaleMaterial;

    void Reset()
    {
        _temporalAntiAliasing = GetComponent<CloudTemporalAntiAliasing>();
        _temporalAntiAliasing.SetCamera(CurrentCamera);
    }

    void Awake()
    {
        Reset();
    }

    void Start()
    {
        if (_CloudMaterial)
            DestroyImmediate(_CloudMaterial);
        if (_UpscaleMaterial)
            DestroyImmediate(_UpscaleMaterial);
        QualitySettings.vSyncCount = 1;
    }

    private void Update()
    {
        //steps = 6 + ((int) ((Mathf.Sin(Time.time / 1f) + 1f) / 2f * 250f));
        _multipliedWindSpeed = windSpeed * globalMultiplier;
        float angleWind = windDirection * Mathf.Deg2Rad;
        _windDirectionVector = new Vector3(Mathf.Cos(angleWind), -0.25f, Mathf.Sin(angleWind));
        _windOffset += _multipliedWindSpeed * _windDirectionVector * Time.deltaTime;

        float angleCoverage = coverageWindDirection * Mathf.Deg2Rad;
        Vector2 coverageDirecton = new Vector2(Mathf.Cos(angleCoverage), Mathf.Sin(angleCoverage));
        _coverageWindOffset += coverageWindSpeed * globalMultiplier * coverageDirecton * Time.deltaTime;

        float angleHighClodus = highCloudsWindDirection * Mathf.Deg2Rad;
        Vector2 highCloudsDirection = new Vector2(Mathf.Cos(angleHighClodus), Mathf.Sin(angleHighClodus));
        _highCloudsWindOffset += highCloudsWindSpeed * globalMultiplier * highCloudsDirection * Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (_CloudMaterial)
            DestroyImmediate(_CloudMaterial);
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


        // UNCOMMENT TO DEBUG RAY DIRECTIONS
        Gizmos.color = Color.red;
        int n = 10; // # of intervals
        for (int x = 1; x < n; x++)
        {
            float i_x = (float)x / (float)n;

            var w_top = Vector3.Lerp(corners.GetRow(0), corners.GetRow(1), i_x);
            var w_bot = Vector3.Lerp(corners.GetRow(3), corners.GetRow(2), i_x);
            for (int y = 1; y < n; y++)
            {
                float i_y = (float)y / (float)n;

                var w = Vector3.Lerp(w_top, w_bot, i_y).normalized;
                Gizmos.DrawLine(pos + (Vector3)w, pos + (Vector3)w * 1.2f);
            }
        }

    }

    private Vector4 gradientToVector4(Gradient gradient)
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
        if (CloudMaterial == null || curlNoise == null || blueNoiseTexture == null || lowFreqNoise == null || highFreqNoise == null)
        {
            Graphics.Blit(source, destination); // do nothing
            return;
        }

        if (_cloudShapeTexture == null)
        {
            _cloudShapeTexture = TGALoader.load3DFromTGASlices(lowFreqNoise);
        }

        if (_cloudErasionTexture == null)
        {
            _cloudErasionTexture = TGALoader.load3DFromTGASlices(highFreqNoise);
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

        if (sunAngle > 170.0f)
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
        updateMaterialKeyword(debugNoCurlNoise, "DEBUG_NO_CURL");
        updateMaterialKeyword(allowFlyingInClouds, "ALLOW_IN_CLOUDS");
        updateMaterialKeyword(randomUnitSphere, "RANDOM_UNIT_SPHERE");
        updateMaterialKeyword(aLotMoreLightSamples, "SLOW_LIGHTING");

        switch (randomJitterNoise)
        {
            case RandomJitter.Off:
                updateMaterialKeyword(false, "RANDOM_JITTER_WHITE");
                updateMaterialKeyword(false, "RANDOM_JITTER_BLUE");
                break;
            case RandomJitter.Random:
                updateMaterialKeyword(true, "RANDOM_JITTER_WHITE");
                updateMaterialKeyword(false, "RANDOM_JITTER_BLUE");
                break;
            case RandomJitter.BlueNoise:
                updateMaterialKeyword(false, "RANDOM_JITTER_WHITE");
                updateMaterialKeyword(true, "RANDOM_JITTER_BLUE");
                break;
        }

        CloudMaterial.SetVector("_SunDir", sunLight.transform ? (-sunLight.transform.forward).normalized : Vector3.up);
        CloudMaterial.SetVector("_PlanetCenter", planetZeroCoordinate - new Vector3(0, planetSize, 0));
        CloudMaterial.SetVector("_ZeroPoint", planetZeroCoordinate);
        CloudMaterial.SetColor("_SunColor", sunColor);
        //CloudMaterial.SetColor("_SunColor", sunLight.color);

        CloudMaterial.SetColor("_CloudBaseColor", cloudBaseColor);
        CloudMaterial.SetColor("_CloudTopColor", cloudTopColor);
        CloudMaterial.SetFloat("_AmbientLightFactor", ambientLightFactorUpdated);
        CloudMaterial.SetFloat("_SunLightFactor", sunLightFactorUpdated);
        //CloudMaterial.SetFloat("_AmbientLightFactor", sunLight.intensity * ambientLightFactor * 0.3f);
        //CloudMaterial.SetFloat("_SunLightFactor", sunLight.intensity * sunLightFactor);

        CloudMaterial.SetTexture("_ShapeTexture", _cloudShapeTexture);
        CloudMaterial.SetTexture("_ErasionTexture", _cloudErasionTexture);
        CloudMaterial.SetTexture("_CurlNoise", curlNoise);
        CloudMaterial.SetTexture("_BlueNoise", blueNoiseTexture);
        CloudMaterial.SetVector("_Randomness", new Vector4(Random.value, Random.value, Random.value, Random.value));
        CloudMaterial.SetTexture("_AltoClouds", cloudsHighTexture);

        CloudMaterial.SetFloat("_CoverageHigh", 1.0f - coverageHigh);
        CloudMaterial.SetFloat("_CoverageHighScale", highCoverageScale * weatheScale * 0.001f);
        CloudMaterial.SetFloat("_HighCloudsScale", highCloudsScale * 0.002f);

        CloudMaterial.SetFloat("_CurlDistortAmount", 150.0f + curlDistortAmount);
        CloudMaterial.SetFloat("_CurlDistortScale", curlDistortScale);

        CloudMaterial.SetFloat("_LightConeRadius", lightConeRadius);
        CloudMaterial.SetFloat("_LightStepLength", lightStepLength);
        CloudMaterial.SetFloat("_SphereSize", planetSize);
        CloudMaterial.SetVector("_CloudHeightMinMax", new Vector2(startHeight, startHeight + thickness));
        CloudMaterial.SetFloat("_Thickness", thickness);
        CloudMaterial.SetFloat("_Scale", 0.00001f + scale * 0.0004f);
        CloudMaterial.SetFloat("_ErasionScale", erasionScale);
        CloudMaterial.SetVector("_LowFreqMinMax", new Vector4(lowFreqMin, lowFreqMax));
        CloudMaterial.SetFloat("_HighFreqModifier", highFreqModifier);
        CloudMaterial.SetFloat("_WeatherScale", weatheScale * 0.00025f);
        CloudMaterial.SetFloat("_Coverage", 1.0f - coverage);
        CloudMaterial.SetFloat("_HenyeyGreensteinGForward", henyeyGreensteinGForward);
        CloudMaterial.SetFloat("_HenyeyGreensteinGBackward", -henyeyGreensteinGBackwardLerp);
        CloudMaterial.SetFloat("_SampleMultiplier", cloudSampleMultiplier);

        CloudMaterial.SetFloat("_Density", density);

        CloudMaterial.SetFloat("_WindSpeed", _multipliedWindSpeed);
        CloudMaterial.SetVector("_WindDirection", _windDirectionVector);
        CloudMaterial.SetVector("_WindOffset", _windOffset);
        CloudMaterial.SetVector("_CoverageWindOffset", _coverageWindOffset);
        CloudMaterial.SetVector("_HighCloudsWindOffset", _highCloudsWindOffset);
        
        CloudMaterial.SetVector("_Gradient1", gradientToVector4(gradientLow));
        CloudMaterial.SetVector("_Gradient2", gradientToVector4(gradientMed));
        CloudMaterial.SetVector("_Gradient3", gradientToVector4(gradientHigh));

        CloudMaterial.SetInt("_Steps", steps);
        if (adjustDensity)
        {
            CloudMaterial.SetFloat("_InverseStep", stepDensityAdjustmentCurve.Evaluate(steps / 256.0f));
        }
        else
        {
            CloudMaterial.SetFloat("_InverseStep", 1.0f);
        }

        CloudMaterial.SetMatrix("_FrustumCornersES", GetFrustumCorners(CurrentCamera));
        CloudMaterial.SetMatrix("_CameraInvViewMatrix", CurrentCamera.cameraToWorldMatrix);
        CloudMaterial.SetVector("_CameraWS", cameraPos);
        CloudMaterial.SetFloat("_FarPlane", CurrentCamera.farClipPlane);

        RenderTexture rtClouds = RenderTexture.GetTemporary((int)(source.width / ((float)downSample)), (int)(source.height / ((float)downSample)), 0, source.format, RenderTextureReadWrite.Default, source.antiAliasing);
        CustomGraphicsBlit(source, rtClouds, CloudMaterial, 0);

        if (temporalAntiAliasing)
        {
            RenderTexture rtTemporal = RenderTexture.GetTemporary(rtClouds.width, rtClouds.height, 0, rtClouds.format, RenderTextureReadWrite.Default, source.antiAliasing);
            _temporalAntiAliasing.TemporalAntiAliasing(rtClouds, rtTemporal);
            UpscaleMaterial.SetTexture("_Clouds", rtTemporal);
            RenderTexture.ReleaseTemporary(rtTemporal);
        }
        else
        {
            UpscaleMaterial.SetTexture("_Clouds", rtClouds);
        }

        Graphics.Blit(source, destination, UpscaleMaterial, 0);
        RenderTexture.ReleaseTemporary(rtClouds);
    }

    private void updateMaterialKeyword(bool b, string keyword)
    {
        if (b != CloudMaterial.IsKeywordEnabled(keyword))
        {
            if (b)
            {
                CloudMaterial.EnableKeyword(keyword);
            }
            else
            {
                CloudMaterial.DisableKeyword(keyword);
            }
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

        //fxMaterial.SetTexture("_MainTex", source);

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
