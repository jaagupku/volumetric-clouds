// https://github.com/Flafla2/Generic-Raymarch-Unity
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Clouds")]
public class CloudsScript : SceneViewFilter
{
    [HeaderAttribute("Performance")]
    public int steps = 128;

    [HeaderAttribute("Cloud modeling")]
    public Shader cloudShader;
    public Texture2D cloudShapeSlices;
    public Texture2D cloudErasionSlices;
    public Texture2D weatherTexture;
    public float startHeight = 1500.0f;
    public float thickness = 4000.0f;
    public float planetSize = 35000.0f;
    [Range(0.0f, 1.0f)]
    public float scale = 0.00041f;
    [Range(0.0f, 1.0f)]
    public float weatheScale = 0.1f;
    [Range(0.0f, 1.0f)]
    public float coverage = 0.662f;

    [HeaderAttribute("Cloud Lighting")]
    public Light sunLight;
    public Color cloudBaseColor = new Color32(145, 192, 212, 255);
    public Color cloudTopColor = new Color32(255, 255, 255, 255);
    [Range(0.0f, 1.0f)]
    public float ambientLightFactor = 0.5f;
    [Range(0.0f, 1.0f)]
    public float sunLightFactor = 0.733f;
    public Color highSunColor = new Color32(255, 252, 210, 255);
    public Color lowSunColor = new Color32(255, 164, 0, 255);
    [Range(0.0f, 1.0f)]
    public float henyeyGreensteinGForward = 0.8f;
    [Range(0.0f, 1.0f)]
    public float henyeyGreensteinGBackward = 0.3f;
    [Range(0.0f, 200.0f)]
    public float lightStepLength = 64.0f;

    private Texture3D cloudShapeTexture;
    private Texture3D cloudErasionTexture;

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

    void Start()
    {
        if (_EffectMaterial)
            DestroyImmediate(_EffectMaterial);
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

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!EffectMaterial == null || cloudShapeSlices == null || cloudErasionSlices == null || weatherTexture == null)
        {
            Graphics.Blit(source, destination); // do nothing
            return;
        }

        if (cloudShapeTexture == null)
        {
            cloudShapeTexture = createTexture3DFrom2DSlices(cloudShapeSlices, 128);
        }

        if (cloudErasionTexture == null)
        {
            cloudErasionTexture = createTexture3DFrom2DSlices(cloudErasionSlices, 32);
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

        EffectMaterial.SetVector("_SunDir", sunLight.transform ? (-sunLight.transform.forward).normalized : Vector3.up);
        EffectMaterial.SetVector("_PlanetCenter", cameraPos - new Vector3(0, planetSize, 0));
        EffectMaterial.SetColor("_SunColor", sunColor);

        EffectMaterial.SetColor("_CloudBaseColor", cloudBaseColor);
        EffectMaterial.SetColor("_CloudTopColor", cloudTopColorMix);
        EffectMaterial.SetFloat("_AmbientLightFactor", ambientLightFactorUpdated);
        EffectMaterial.SetFloat("_SunLightFactor", sunLightFactorUpdated);

        EffectMaterial.SetTexture("_ShapeTexture", cloudShapeTexture);
        EffectMaterial.SetTexture("_ErasionTexture", cloudErasionTexture);
        EffectMaterial.SetTexture("_WeatherTexture", weatherTexture);
        
        EffectMaterial.SetFloat("_LightStepLength", lightStepLength);
        EffectMaterial.SetFloat("_SphereSize", planetSize);
        EffectMaterial.SetFloat("_StartHeight", startHeight);
        EffectMaterial.SetFloat("_Thickness", thickness);
        EffectMaterial.SetFloat("_Scale", scale);
        EffectMaterial.SetFloat("_WeatherScale", weatheScale * 0.001f);
        EffectMaterial.SetFloat("_Coverage", 1 - coverage);
        EffectMaterial.SetFloat("_HenyeyGreensteinGForward", henyeyGreensteinGForward);
        EffectMaterial.SetFloat("_HenyeyGreensteinGBackward", -henyeyGreensteinGBackwardLerp);

        EffectMaterial.SetInt("_Steps", steps);
        EffectMaterial.SetFloat("_InverseStep", 128.0f / steps);

        EffectMaterial.SetMatrix("_FrustumCornersES", GetFrustumCorners(CurrentCamera));
        EffectMaterial.SetMatrix("_CameraInvViewMatrix", CurrentCamera.cameraToWorldMatrix);
        EffectMaterial.SetVector("_CameraWS", cameraPos);

        CustomGraphicsBlit(source, destination, EffectMaterial, 0);
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

    // https://support.unity3d.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-
    Texture2D getReadableTexture(Texture2D texture)
    {
        // Create a temporary RenderTexture of the same size as the texture
        RenderTexture tmp = RenderTexture.GetTemporary(
                            texture.width,
                            texture.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

        // Blit the pixels on texture to the RenderTexture
        Graphics.Blit(texture, tmp);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmp;
        Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
        myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        myTexture2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmp);
        return myTexture2D;
    }

    Texture3D createTexture3DFrom2DSlices(Texture2D tex2d, int size)
    {
        Texture2D readableTexture2D = getReadableTexture(tex2d);

        Color[] colors = new Color[size * size * size];
        int idx = 0;
        for (int z = 0; z < size; ++z)
        {
            for (int y = 0; y < size; ++y)
            {
                for (int x = 0; x < size; ++x, ++idx)
                {
                    colors[idx] = readableTexture2D.GetPixel(x + z * size, y);
                }
            }
        }

        Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.ARGB32, true);
        texture3D.SetPixels(colors);
        texture3D.Apply();
        return texture3D;
    }

}
