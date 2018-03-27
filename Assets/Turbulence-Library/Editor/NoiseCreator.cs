using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

enum NoiseType2D3D { Cell, Perlin, Simplex, Value };
enum NoiseType4D { Perlin, Value };
enum NoiseSubtype2D3D { Normal, Billowed, Ridged, DerivedIQ, DerivedSwiss, DerivedJordan };
enum NoiseSubtype4D { Normal, Billowed, Ridged };
enum NoiseSubtypeCell { Normal, Fast };
enum NoiseCellType { F1, F2, F3, F4, F2minF1, F3minF2, F1plusF2div2, F1mulF2, Crackle };
enum NoiseDistanceFunction { Euclidean, EuclideanSquared, Manhattan, Chebyshev, Quadratic, Minkowski, Star };

public class NoiseCreator : EditorWindow
{
    // Dimension
    int selectedDimension = 2;
    string[] dimensionNames = new string[] { "2D", "3D"/*, "4D"*/ };
    int[] dimensionSizes = new int[] { 2, 3/*, 4*/ };

    // Noise Types
    NoiseType2D3D noiseType23;
    NoiseType4D noiseType4;

    // Noise Subtypes
    NoiseSubtype2D3D noiseSubtype2D3D;
    NoiseSubtype4D noiseSubtype4D;
    NoiseSubtypeCell noiseSubtypeCell;
    NoiseCellType noiseCellType;
    NoiseDistanceFunction noiseDistanceFunction;

    // Material Properties
    static readonly string[] properties2D = new string[] { "Normalized", "Transparent", "Displaced", "Colored", "Textured" };
    bool[] toggles2D = new bool[] { false, false, false, false, false };
    static readonly string[] properties3D4D = new string[] { "Normalized", "Transparent", "Animated", "Displaced", "Colored", "Textured" };
    bool[] toggles3D4D = new bool[] { false, false, false, false, false, false };
    bool showProperties = true;

    // I/O
    string tempShaderLocation;
    string tempShader;
    string shaderLocation;
    string shaderComments = "//\n" +
                            "//	Code repository for GPU noise development blog\n" +
                            "//	http://briansharpe.wordpress.com\n" +
                            "//	https://github.com/BrianSharpe\n" +
                            "//\n" +
                            "//	I'm not one for copyrights.  Use the code however you wish.\n" +
                            "//	All I ask is that credit be given back to the blog or myself when appropriate.\n" +
                            "//	And also to let me know if you come up with any changes, improvements, thoughts or interesting uses for this stuff. :)\n" +
                            "//	Thanks!\n" +
                            "//\n" +
                            "//	Brian Sharpe\n" +
                            "//	brisharpe CIRCLE_A yahoo DOT com\n" +
                            "//	http://briansharpe.wordpress.com\n" +
                            "//	https://github.com/BrianSharpe\n" +
                            "//\n" +
                            "//===============================================================================\n" +
                            "//  Scape Software License\n" +
                            "//===============================================================================\n" +
                            "//\n" +
                            "//Copyright (c) 2007-2012, Giliam de Carpentier\n" +
                            "//All rights reserved.\n" +
                            "//\n" +
                            "//Redistribution and use in source and binary forms, with or without\n" +
                            "//modification, are permitted provided that the following conditions are met: \n" +
                            "//\n" +
                            "//1. Redistributions of source code must retain the above copyright notice, this\n" +
                            "//   list of conditions and the following disclaimer. \n" +
                            "//2. Redistributions in binary form must reproduce the above copyright notice,\n" +
                            "//   this list of conditions and the following disclaimer in the documentation\n" +
                            "//   and/or other materials provided with the distribution. \n" +
                            "//\n" +
                            "//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\" AND\n" +
                            "//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED\n" +
                            "//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE\n" +
                            "//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNERS OR CONTRIBUTORS BE LIABLE \n" +
                            "//FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL \n" +
                            "//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR \n" +
                            "//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER \n" +
                            "//CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, \n" +
                            "//OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE \n" +
                            "//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.;";
    string shaderName = "MyShader";
    string shaderProperties = "";
    string shaderTags = "";
    string shaderBlending = "";
    string shaderLighting = "";
    string shaderInclude = "";
    string shaderUniforms = "";
    string shaderInput = "";
    string shaderVertex = "";
    string shaderNoise = "";
    string shaderNormalize = "";
    string shaderColoringTexturing = "";
    string shaderAlpha = "";

    [MenuItem("Window/Noise Creator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(NoiseCreator));
    }

    void OnEnable()
    {
        tempShaderLocation = Application.dataPath + "/Turbulence-Library/Resources/shader.template";
        tempShader = File.ReadAllText(tempShaderLocation);
    }

    void OnGUI()
    {
        selectedDimension = EditorGUILayout.IntPopup("Dimension: ", selectedDimension, dimensionNames, dimensionSizes);

        EditorGUILayout.Space();

        ShowTypes();

        EditorGUILayout.Space();

        ShowSubtypes();

        EditorGUILayout.Space();

        ShowProperties();

        EditorGUILayout.Space();

        ShowButtons();
    }

    void ShowTypes()
    {
        switch (selectedDimension)
        {
            case 2:
                noiseType23 = (NoiseType2D3D)EditorGUILayout.EnumPopup("Noise Type: ", noiseType23);
                break;
            case 3:
                noiseType23 = (NoiseType2D3D)EditorGUILayout.EnumPopup("Noise Type: ", noiseType23);
                break;
            case 4:
                noiseType4 = (NoiseType4D)EditorGUILayout.EnumPopup("Noise Type: ", noiseType4);
                break;
        }
    }

    void ShowSubtypes()
    {
        if (selectedDimension == 2 || selectedDimension == 3)
        {
            if (noiseType23 == NoiseType2D3D.Cell)
            {
                noiseSubtypeCell = (NoiseSubtypeCell)EditorGUILayout.EnumPopup("Subtype: ", noiseSubtypeCell);
            }
            else
            {
                noiseSubtype2D3D = (NoiseSubtype2D3D)EditorGUILayout.EnumPopup("Subtype: ", noiseSubtype2D3D);
            }
        }
        else if (selectedDimension == 4)
        {
            noiseSubtype4D = (NoiseSubtype4D)EditorGUILayout.EnumPopup("Subtype: ", noiseSubtype4D);
        }
    }

    void ShowProperties()
    {
        showProperties = EditorGUILayout.Foldout(showProperties, "Material properties");
        if (showProperties)
        {
            if (selectedDimension == 2)
            {
                for (int i = 0; i < properties2D.Length; i++)
                {
                    toggles2D[i] = EditorGUILayout.Toggle(properties2D[i], toggles2D[i]);
                }
            }
            else
            {
                for (int i = 0; i < properties3D4D.Length; i++)
                {
                    toggles3D4D[i] = EditorGUILayout.Toggle(properties3D4D[i], toggles3D4D[i]);
                }
            }
        }
    }

    void ShowButtons()
    {
        if (GUILayout.Button("Create Shader"))
        {
            BuildShader();
        }
    }

    void BuildShader()
    {
        if (selectedDimension == 2)
        {
            AssignShaderStrings2D();
        }
        else if (selectedDimension == 3)
        {
            AssignShaderStrings3D();
        }

        string source = tempShader;
        source = source.Replace("${Comments}", shaderComments);
        source = source.Replace("${Name}", shaderName);
        source = source.Replace("${Properties}", shaderProperties);
        source = source.Replace("${Tags}", shaderTags);
        source = source.Replace("${Blending}", shaderBlending);
        source = source.Replace("${Lighting}", shaderLighting);
        source = source.Replace("${Include}", shaderInclude);
        source = source.Replace("${Uniforms}", shaderUniforms);
        source = source.Replace("${Input}", shaderInput);
        source = source.Replace("${Vertex}", shaderVertex);
        source = source.Replace("${Noise}", shaderNoise);
        source = source.Replace("${Normalize}", shaderNormalize);
        source = source.Replace("${ColoringTexturing}", shaderColoringTexturing);
        source = source.Replace("${Alpha}", shaderAlpha);

        shaderLocation = EditorUtility.SaveFilePanelInProject("Save shader", shaderName, "shader", "");

        File.WriteAllText(shaderLocation, source);
        AssetDatabase.Refresh();

        ClearProperties();
    }

    #region Shader
    void AssignShaderStrings2D()
    {
        string noiseFunction = noiseType23.ToString() + (noiseType23 == NoiseType2D3D.Cell ? noiseSubtypeCell.ToString() : noiseSubtype2D3D.ToString());

        // Basic settings
        shaderLighting = NoiseStrings2D.LightingLambert;
        shaderProperties += NoiseStrings2D.PropertiesNormal;
        shaderUniforms += NoiseStrings2D.UniformsNormal;
        shaderInput = NoiseStrings2D.InputNormal;

        // Noise
        switch (noiseFunction)
        {
            case "PerlinNormal":
                shaderInclude += NoiseStrings2D.IncludePerlin;
                shaderInclude += NoiseStrings2D.IncludeFbmPerlinNormal;
                shaderNoise = NoiseStrings2D.NoisePerlinNormal;
                break;
            case "PerlinBillowed":
                shaderInclude += NoiseStrings2D.IncludePerlin;
                shaderInclude += NoiseStrings2D.IncludeFbmPerlinBillowed;
                shaderNoise = NoiseStrings2D.NoisePerlinBillowed;
                break;
            case "PerlinRidged":
                shaderProperties += NoiseStrings2D.PropertiesRidged;
                shaderUniforms += NoiseStrings2D.UniformsRidged;
                shaderInclude += NoiseStrings2D.IncludePerlin;
                shaderInclude += NoiseStrings2D.IncludeFbmPerlinRidged;
                shaderNoise = NoiseStrings2D.NoisePerlinRidged;
                break;
            case "PerlinDerivedIQ":
                shaderInclude += NoiseStrings2D.IncludePerlinDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmPerlinDerivedIQ;
                shaderNoise = NoiseStrings2D.NoisePerlinDerivedIQ;
                break;
            case "PerlinDerivedSwiss":
                shaderProperties += NoiseStrings2D.PropertiesDerivedSwiss;
                shaderUniforms += NoiseStrings2D.UniformsDerivedSwiss;
                shaderInclude += NoiseStrings2D.IncludePerlinDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmPerlinDerivedSwiss;
                shaderNoise = NoiseStrings2D.NoisePerlinDerivedSwiss;
                break;
            case "PerlinDerivedJordan":
                shaderProperties += NoiseStrings2D.PropertiesDerivedJordan;
                shaderUniforms += NoiseStrings2D.UniformsDerivedJordan;
                shaderInclude += NoiseStrings2D.IncludePerlinDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmPerlinDerivedJordan;
                shaderNoise = NoiseStrings2D.NoisePerlinDerivedJordan;
                break;
            case "SimplexNormal":
                shaderInclude += NoiseStrings2D.IncludeSimplex;
                shaderInclude += NoiseStrings2D.IncludeFbmSimplexNormal;
                shaderNoise = NoiseStrings2D.NoiseSimplexNormal;
                break;
            case "SimplexBillowed":
                shaderInclude += NoiseStrings2D.IncludeSimplex;
                shaderInclude += NoiseStrings2D.IncludeFbmSimplexBillowed;
                shaderNoise = NoiseStrings2D.NoiseSimplexBillowed;
                break;
            case "SimplexRidged":
                shaderProperties += NoiseStrings2D.PropertiesRidged;
                shaderUniforms += NoiseStrings2D.UniformsRidged;
                shaderInclude += NoiseStrings2D.IncludeSimplex;
                shaderInclude += NoiseStrings2D.IncludeFbmSimplexRidged;
                shaderNoise = NoiseStrings2D.NoiseSimplexRidged;
                break;
            case "SimplexDerivedIQ":
                shaderInclude += NoiseStrings2D.IncludeSimplexDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmSimplexDerivedIQ;
                shaderNoise = NoiseStrings2D.NoiseSimplexDerivedIQ;
                break;
            case "SimplexDerivedSwiss":
                shaderProperties += NoiseStrings2D.PropertiesDerivedSwiss;
                shaderUniforms += NoiseStrings2D.UniformsDerivedSwiss;
                shaderInclude += NoiseStrings2D.IncludeSimplexDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmSimplexDerivedSwiss;
                shaderNoise = NoiseStrings2D.NoiseSimplexDerivedSwiss;
                break;
            case "SimplexDerivedJordan":
                shaderProperties += NoiseStrings2D.PropertiesDerivedJordan;
                shaderUniforms += NoiseStrings2D.UniformsDerivedJordan;
                shaderInclude += NoiseStrings2D.IncludeSimplexDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmSimplexDerivedJordan;
                shaderNoise = NoiseStrings2D.NoiseSimplexDerivedJordan;
                break;
            case "ValueNormal":
                shaderInclude += NoiseStrings2D.IncludeValue;
                shaderInclude += NoiseStrings2D.IncludeFbmValueNormal;
                shaderNoise = NoiseStrings2D.NoiseValueNormal;
                break;
            case "ValueBillowed":
                shaderInclude += NoiseStrings2D.IncludeValue;
                shaderInclude += NoiseStrings2D.IncludeFbmValueBillowed;
                shaderNoise = NoiseStrings2D.NoiseValueBillowed;
                break;
            case "ValueRidged":
                shaderProperties += NoiseStrings2D.PropertiesRidged;
                shaderUniforms += NoiseStrings2D.UniformsRidged;
                shaderInclude += NoiseStrings2D.IncludeValue;
                shaderInclude += NoiseStrings2D.IncludeFbmValueRidged;
                shaderNoise = NoiseStrings2D.NoiseValueRidged;
                break;
            case "ValueDerivedIQ":
                shaderInclude += NoiseStrings2D.IncludeValueDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmValueDerivedIQ;
                shaderNoise = NoiseStrings2D.NoiseValueDerivedIQ;
                break;
            case "ValueDerivedSwiss":
                shaderProperties += NoiseStrings2D.PropertiesDerivedSwiss;
                shaderUniforms += NoiseStrings2D.UniformsDerivedSwiss;
                shaderInclude += NoiseStrings2D.IncludeValueDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmValueDerivedSwiss;
                shaderNoise = NoiseStrings2D.NoiseValueDerivedSwiss;
                break;
            case "ValueDerivedJordan":
                shaderProperties += NoiseStrings2D.PropertiesDerivedJordan;
                shaderUniforms += NoiseStrings2D.UniformsDerivedJordan;
                shaderInclude += NoiseStrings2D.IncludeValueDerived;
                shaderInclude += NoiseStrings2D.IncludeFbmValueDerivedJordan;
                shaderNoise = NoiseStrings2D.NoiseValueDerivedJordan;
                break;
            case "CellNormal":
                shaderProperties += NoiseStrings2D.PropertiesCell;
                shaderUniforms += NoiseStrings2D.UniformsCell;
                shaderInclude += NoiseStrings2D.IncludeCellNormal;
                shaderInclude += NoiseStrings2D.IncludeFbmCellNormal;
                shaderNoise = NoiseStrings2D.NoiseCellNormal;
                break;
            case "CellFast":
                shaderInclude += NoiseStrings2D.IncludeCellFast;
                shaderInclude += NoiseStrings2D.IncludeFbmCellFast;
                shaderNoise = NoiseStrings2D.NoiseCellFast;
                break;
        }

        // Normalization
        if (toggles2D[0])
        {
            shaderNormalize = NoiseStrings2D.NormalizeOn;
        }

        // Transparency
        if (toggles2D[1])
        {
            shaderTags = NoiseStrings2D.TagsTransparent;
            shaderBlending = NoiseStrings2D.BlendingAlpha;
            shaderAlpha = NoiseStrings2D.AlphaOn;
            shaderProperties += NoiseStrings2D.PropertiesTransparency;
            shaderUniforms += NoiseStrings2D.UniformsTransparency;
        }
        else
        {
            shaderAlpha = NoiseStrings2D.AlphaOff;
        }

        // Displacement
        if (toggles2D[2])
        {
            shaderProperties += NoiseStrings2D.PropertiesDisplacement;
            shaderUniforms += NoiseStrings2D.UniformsDisplacement;
            shaderVertex = NoiseStrings2D.VertexLocalDispOn;
            shaderVertex = shaderVertex.Replace("${VertexNoise}", shaderNoise.Replace("IN.pos", "OUT.pos"));
        }
        else
        {
            shaderVertex = NoiseStrings2D.VertexLocalDispOff;
        }

        // Coloring only
        if (toggles2D[3] && !toggles2D[4])
        {
            shaderColoringTexturing = NoiseStrings2D.ColoringOnTexturingOff;
            shaderProperties += NoiseStrings2D.PropertiesColoring;
            shaderUniforms += NoiseStrings2D.UniformsColoring;
        }
        // Coloring and Texturing
        else if (toggles2D[3] && toggles2D[4])
        {
            shaderColoringTexturing = NoiseStrings2D.ColoringOnTexturingOn;
            shaderInput += NoiseStrings2D.InputTexturing;
            shaderProperties += NoiseStrings2D.PropertiesColoring;
            shaderProperties += NoiseStrings2D.PropertiesTexturing;
            shaderUniforms += NoiseStrings2D.UniformsColoring;
            shaderUniforms += NoiseStrings2D.UniformsTexturing;
        }
        // Texturing only
        else if (!toggles2D[3] && toggles2D[4])
        {
            shaderColoringTexturing = NoiseStrings2D.ColoringOffTexturingOn;
            shaderInput += NoiseStrings2D.InputTexturing;
            shaderProperties += NoiseStrings2D.PropertiesTexturing;
            shaderUniforms += NoiseStrings2D.UniformsTexturing;
        }
        else
        {
            shaderColoringTexturing = NoiseStrings2D.ColoringOffTexturingOff;
        }
    }

    void AssignShaderStrings3D()
    {
        string noiseFunction = noiseType23.ToString() + (noiseType23 == NoiseType2D3D.Cell ? noiseSubtypeCell.ToString() : noiseSubtype2D3D.ToString());

        // Basic settings
        shaderLighting = NoiseStrings3D.LightingLambert;
        shaderProperties += NoiseStrings3D.PropertiesNormal;
        shaderUniforms += NoiseStrings3D.UniformsNormal;
        shaderInput = NoiseStrings3D.InputNormal;

        // Noise
        switch (noiseFunction)
        {
            case "PerlinNormal":
                shaderInclude += NoiseStrings3D.IncludePerlin;
                shaderInclude += NoiseStrings3D.IncludeFbmPerlinNormal;
                shaderNoise = NoiseStrings3D.NoisePerlinNormal;
                break;
            case "PerlinBillowed":
                shaderInclude += NoiseStrings3D.IncludePerlin;
                shaderInclude += NoiseStrings3D.IncludeFbmPerlinBillowed;
                shaderNoise = NoiseStrings3D.NoisePerlinBillowed;
                break;
            case "PerlinRidged":
                shaderProperties += NoiseStrings3D.PropertiesRidged;
                shaderUniforms += NoiseStrings3D.UniformsRidged;
                shaderInclude += NoiseStrings3D.IncludePerlin;
                shaderInclude += NoiseStrings3D.IncludeFbmPerlinRidged;
                shaderNoise = NoiseStrings3D.NoisePerlinRidged;
                break;
            case "PerlinDerivedIQ":
                shaderInclude += NoiseStrings3D.IncludePerlinDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmPerlinDerivedIQ;
                shaderNoise = NoiseStrings3D.NoisePerlinDerivedIQ;
                break;
            case "PerlinDerivedSwiss":
                shaderProperties += NoiseStrings3D.PropertiesDerivedSwiss;
                shaderUniforms += NoiseStrings3D.UniformsDerivedSwiss;
                shaderInclude += NoiseStrings3D.IncludePerlinDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmPerlinDerivedSwiss;
                shaderNoise = NoiseStrings3D.NoisePerlinDerivedSwiss;
                break;
            case "PerlinDerivedJordan":
                shaderProperties += NoiseStrings3D.PropertiesDerivedJordan;
                shaderUniforms += NoiseStrings3D.UniformsDerivedJordan;
                shaderInclude += NoiseStrings3D.IncludePerlinDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmPerlinDerivedJordan;
                shaderNoise = NoiseStrings3D.NoisePerlinDerivedJordan;
                break;
            case "SimplexNormal":
                shaderInclude += NoiseStrings3D.IncludeSimplex;
                shaderInclude += NoiseStrings3D.IncludeFbmSimplexNormal;
                shaderNoise = NoiseStrings3D.NoiseSimplexNormal;
                break;
            case "SimplexBillowed":
                shaderInclude += NoiseStrings3D.IncludeSimplex;
                shaderInclude += NoiseStrings3D.IncludeFbmSimplexBillowed;
                shaderNoise = NoiseStrings3D.NoiseSimplexBillowed;
                break;
            case "SimplexRidged":
                shaderProperties += NoiseStrings3D.PropertiesRidged;
                shaderUniforms += NoiseStrings3D.UniformsRidged;
                shaderInclude += NoiseStrings3D.IncludeSimplex;
                shaderInclude += NoiseStrings3D.IncludeFbmSimplexRidged;
                shaderNoise = NoiseStrings3D.NoiseSimplexRidged;
                break;
            case "SimplexDerivedIQ":
                shaderInclude += NoiseStrings3D.IncludeSimplexDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmSimplexDerivedIQ;
                shaderNoise = NoiseStrings3D.NoiseSimplexDerivedIQ;
                break;
            case "SimplexDerivedSwiss":
                shaderProperties += NoiseStrings3D.PropertiesDerivedSwiss;
                shaderUniforms += NoiseStrings3D.UniformsDerivedSwiss;
                shaderInclude += NoiseStrings3D.IncludeSimplexDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmSimplexDerivedSwiss;
                shaderNoise = NoiseStrings3D.NoiseSimplexDerivedSwiss;
                break;
            case "SimplexDerivedJordan":
                shaderProperties += NoiseStrings3D.PropertiesDerivedJordan;
                shaderUniforms += NoiseStrings3D.UniformsDerivedJordan;
                shaderInclude += NoiseStrings3D.IncludeSimplexDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmSimplexDerivedJordan;
                shaderNoise = NoiseStrings3D.NoiseSimplexDerivedJordan;
                break;
            case "ValueNormal":
                shaderInclude += NoiseStrings3D.IncludeValue;
                shaderInclude += NoiseStrings3D.IncludeFbmValueNormal;
                shaderNoise = NoiseStrings3D.NoiseValueNormal;
                break;
            case "ValueBillowed":
                shaderInclude += NoiseStrings3D.IncludeValue;
                shaderInclude += NoiseStrings3D.IncludeFbmValueBillowed;
                shaderNoise = NoiseStrings3D.NoiseValueBillowed;
                break;
            case "ValueRidged":
                shaderProperties += NoiseStrings3D.PropertiesRidged;
                shaderUniforms += NoiseStrings3D.UniformsRidged;
                shaderInclude += NoiseStrings3D.IncludeValue;
                shaderInclude += NoiseStrings3D.IncludeFbmValueRidged;
                shaderNoise = NoiseStrings3D.NoiseValueRidged;
                break;
            case "ValueDerivedIQ":
                shaderInclude += NoiseStrings3D.IncludeValueDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmValueDerivedIQ;
                shaderNoise = NoiseStrings3D.NoiseValueDerivedIQ;
                break;
            case "ValueDerivedSwiss":
                shaderProperties += NoiseStrings3D.PropertiesDerivedSwiss;
                shaderUniforms += NoiseStrings3D.UniformsDerivedSwiss;
                shaderInclude += NoiseStrings3D.IncludeValueDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmValueDerivedSwiss;
                shaderNoise = NoiseStrings3D.NoiseValueDerivedSwiss;
                break;
            case "ValueDerivedJordan":
                shaderProperties += NoiseStrings3D.PropertiesDerivedJordan;
                shaderUniforms += NoiseStrings3D.UniformsDerivedJordan;
                shaderInclude += NoiseStrings3D.IncludeValueDerived;
                shaderInclude += NoiseStrings3D.IncludeFbmValueDerivedJordan;
                shaderNoise = NoiseStrings3D.NoiseValueDerivedJordan;
                break;
            case "CellNormal":
                shaderProperties += NoiseStrings3D.PropertiesCell;
                shaderUniforms += NoiseStrings3D.UniformsCell;
                shaderInclude += NoiseStrings3D.IncludeCellNormal;
                shaderInclude += NoiseStrings3D.IncludeFbmCellNormal;
                shaderNoise = NoiseStrings3D.NoiseCellNormal;
                break;
            case "CellFast":
                shaderInclude += NoiseStrings3D.IncludeCellFast;
                shaderInclude += NoiseStrings3D.IncludeFbmCellFast;
                shaderNoise = NoiseStrings3D.NoiseCellFast;
                break;
        }

        // Normalization
        if (toggles3D4D[0])
        {
            shaderNormalize = NoiseStrings3D.NormalizeOn;
        }

        // Transparency
        if (toggles3D4D[1])
        {
            shaderTags = NoiseStrings3D.TagsTransparent;
            shaderBlending = NoiseStrings3D.BlendingAlpha;
            shaderAlpha = NoiseStrings3D.AlphaOn;
            shaderProperties += NoiseStrings3D.PropertiesTransparency;
            shaderUniforms += NoiseStrings3D.UniformsTransparency;
        }
        else
        {
            shaderAlpha = NoiseStrings3D.AlphaOff;
        }

        // Displacement only
        if (toggles3D4D[3] && !toggles3D4D[2])
        {
            shaderProperties += NoiseStrings3D.PropertiesDisplacement;
            shaderUniforms += NoiseStrings3D.UniformsDisplacement;
            shaderVertex = NoiseStrings3D.VertexLocalDispOnAnimOff;
            shaderVertex = shaderVertex.Replace("${VertexNoise}", shaderNoise.Replace("IN.pos", "OUT.pos"));
        }
        // Displacement and animation
        else if(toggles3D4D[3] && toggles3D4D[2])
        {
            shaderProperties += NoiseStrings3D.PropertiesDisplacement;
            shaderProperties += NoiseStrings3D.PropertiesAnimation;
            shaderUniforms += NoiseStrings3D.UniformsDisplacement;
            shaderUniforms += NoiseStrings3D.UniformsAnimation;
            shaderVertex = NoiseStrings3D.VertexLocalDispOnAnimOn;
            shaderVertex = shaderVertex.Replace("${VertexNoise}", shaderNoise.Replace("IN.pos", "OUT.pos"));
        }
        // Animation only
        else if (!toggles3D4D[3] && toggles3D4D[2])
        {
            shaderProperties += NoiseStrings3D.PropertiesAnimation;
            shaderUniforms += NoiseStrings3D.UniformsAnimation;
            shaderVertex = NoiseStrings3D.VertexLocalDispOffAnimOn;
            shaderVertex = shaderVertex.Replace("${VertexNoise}", shaderNoise.Replace("IN.pos", "OUT.pos"));
        }
        else
        {
            shaderVertex = NoiseStrings3D.VertexLocalDispOffAnimOff;
        }

        // Coloring only
        if (toggles3D4D[4] && !toggles3D4D[5])
        {
            shaderColoringTexturing = NoiseStrings3D.ColoringOnTexturingOff;
            shaderProperties += NoiseStrings3D.PropertiesColoring;
            shaderUniforms += NoiseStrings3D.UniformsColoring;
        }
        // Coloring and Texturing
        else if (toggles3D4D[4] && toggles3D4D[5])
        {
            shaderColoringTexturing = NoiseStrings3D.ColoringOnTexturingOn;
            shaderInput += NoiseStrings3D.InputTexturing;
            shaderProperties += NoiseStrings3D.PropertiesColoring;
            shaderProperties += NoiseStrings3D.PropertiesTexturing;
            shaderUniforms += NoiseStrings3D.UniformsColoring;
            shaderUniforms += NoiseStrings3D.UniformsTexturing;
        }
        // Texturing only
        else if (!toggles3D4D[4] && toggles3D4D[5])
        {
            shaderColoringTexturing = NoiseStrings3D.ColoringOffTexturingOn;
            shaderInput += NoiseStrings3D.InputTexturing;
            shaderProperties += NoiseStrings3D.PropertiesTexturing;
            shaderUniforms += NoiseStrings3D.UniformsTexturing;
        }
        else
        {
            shaderColoringTexturing = NoiseStrings3D.ColoringOffTexturingOff;
        }

    }
    #endregion

    void ClearProperties()
    {
        shaderAlpha = "";
        shaderBlending = "";
        shaderInclude = "";
        shaderInput = "";
        shaderLighting = "";
        shaderNoise = "";
        shaderNormalize = "";
        shaderProperties = "";
        shaderTags = "";
        shaderUniforms = "";
    }
}
