Shader "Unlit/TienDoChucPhuc"
{
    Properties
    {
        [MainTexture][NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _TienDo("TienDo", Range(0, 100)) = 47.01
        [HDR]_UperColor("UperColor", Color) = (1, 0.4198113, 0.4198113, 0)
        [HDR]_MainColor("MainColor", Color) = (1, 0, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"=""
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEUNLIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _TienDo;
        float4 _UperColor;
        float4 _MainColor;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_56f1bacf51d144b3ba780cf016b1f498_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_56f1bacf51d144b3ba780cf016b1f498_Out_0.tex, _Property_56f1bacf51d144b3ba780cf016b1f498_Out_0.samplerstate, _Property_56f1bacf51d144b3ba780cf016b1f498_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_R_4 = _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0.r;
            float _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_G_5 = _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0.g;
            float _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_B_6 = _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0.b;
            float _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_A_7 = _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0.a;
            float _Multiply_46dfc18710d44864a330ed91c30be85e_Out_2;
            Unity_Multiply_float_float(4, 3.25, _Multiply_46dfc18710d44864a330ed91c30be85e_Out_2);
            float _Multiply_c22afa9e9a674a1b867598c9839f2073_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, -0.3, _Multiply_c22afa9e9a674a1b867598c9839f2073_Out_2);
            float _Property_a305005eef6840aebed90f0575c7ab6d_Out_0 = _TienDo;
            float _Divide_871b489b1767477d9a3e0d0ebe5173d5_Out_2;
            Unity_Divide_float(_Property_a305005eef6840aebed90f0575c7ab6d_Out_0, 100, _Divide_871b489b1767477d9a3e0d0ebe5173d5_Out_2);
            float _Add_12e7854d9e91438080d549aa781379e9_Out_2;
            Unity_Add_float(_Divide_871b489b1767477d9a3e0d0ebe5173d5_Out_2, -0.4, _Add_12e7854d9e91438080d549aa781379e9_Out_2);
            float _Multiply_0a88918607964f3d973ae6528f47b35a_Out_2;
            Unity_Multiply_float_float(_Add_12e7854d9e91438080d549aa781379e9_Out_2, -1, _Multiply_0a88918607964f3d973ae6528f47b35a_Out_2);
            float2 _Vector2_68f31be1c1d2472ca8d651b78ff5b815_Out_0 = float2(_Multiply_c22afa9e9a674a1b867598c9839f2073_Out_2, _Multiply_0a88918607964f3d973ae6528f47b35a_Out_2);
            float2 _TilingAndOffset_7a2552767e6e46db9afb89c3dcd80bb1_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Vector2_68f31be1c1d2472ca8d651b78ff5b815_Out_0, _TilingAndOffset_7a2552767e6e46db9afb89c3dcd80bb1_Out_3);
            float _Split_ed989f8a5fa949a4a589fcc416beada3_R_1 = _TilingAndOffset_7a2552767e6e46db9afb89c3dcd80bb1_Out_3[0];
            float _Split_ed989f8a5fa949a4a589fcc416beada3_G_2 = _TilingAndOffset_7a2552767e6e46db9afb89c3dcd80bb1_Out_3[1];
            float _Split_ed989f8a5fa949a4a589fcc416beada3_B_3 = 0;
            float _Split_ed989f8a5fa949a4a589fcc416beada3_A_4 = 0;
            float _Multiply_e8c26c1a1c1c46d1a3eacca999176bea_Out_2;
            Unity_Multiply_float_float(_Multiply_46dfc18710d44864a330ed91c30be85e_Out_2, _Split_ed989f8a5fa949a4a589fcc416beada3_R_1, _Multiply_e8c26c1a1c1c46d1a3eacca999176bea_Out_2);
            float _Sine_6543999985544d8597e1afda64bb1d57_Out_1;
            Unity_Sine_float(_Multiply_e8c26c1a1c1c46d1a3eacca999176bea_Out_2, _Sine_6543999985544d8597e1afda64bb1d57_Out_1);
            float _Multiply_610f2979180f496b90df79ab97841f91_Out_2;
            Unity_Multiply_float_float(_Sine_6543999985544d8597e1afda64bb1d57_Out_1, 0.04, _Multiply_610f2979180f496b90df79ab97841f91_Out_2);
            float _Remap_ec295589823c4e498a9a640648ae1b9e_Out_3;
            Unity_Remap_float(_Multiply_610f2979180f496b90df79ab97841f91_Out_2, float2 (-0.7, 1), float2 (0, 1), _Remap_ec295589823c4e498a9a640648ae1b9e_Out_3);
            float _Step_56a3378a6cdc4839b44f487ab09855f9_Out_2;
            Unity_Step_float(_Remap_ec295589823c4e498a9a640648ae1b9e_Out_3, _Split_ed989f8a5fa949a4a589fcc416beada3_G_2, _Step_56a3378a6cdc4839b44f487ab09855f9_Out_2);
            float _Remap_cad549dbcc95470b958cb1ba5766f9b7_Out_3;
            Unity_Remap_float(_Multiply_610f2979180f496b90df79ab97841f91_Out_2, float2 (-1, 1), float2 (0, 1), _Remap_cad549dbcc95470b958cb1ba5766f9b7_Out_3);
            float _Step_f4b42e180f27490ab7c403302fbfcb0d_Out_2;
            Unity_Step_float(_Remap_cad549dbcc95470b958cb1ba5766f9b7_Out_3, _Split_ed989f8a5fa949a4a589fcc416beada3_G_2, _Step_f4b42e180f27490ab7c403302fbfcb0d_Out_2);
            float _Subtract_c15f5ceddf11425582cd389172d75cda_Out_2;
            Unity_Subtract_float(_Step_56a3378a6cdc4839b44f487ab09855f9_Out_2, _Step_f4b42e180f27490ab7c403302fbfcb0d_Out_2, _Subtract_c15f5ceddf11425582cd389172d75cda_Out_2);
            float4 _Lerp_00dff7754ec74ef187573fba25693360_Out_3;
            Unity_Lerp_float4(float4(0, 0, 0, 0), _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0, (_Subtract_c15f5ceddf11425582cd389172d75cda_Out_2.xxxx), _Lerp_00dff7754ec74ef187573fba25693360_Out_3);
            float4 _Property_e315cca8237c4aeaa6006f1490ed4403_Out_0 = IsGammaSpace() ? LinearToSRGB(_UperColor) : _UperColor;
            float4 _Multiply_52cb6f8f19684735af5921b9bac1d729_Out_2;
            Unity_Multiply_float4_float4(_Lerp_00dff7754ec74ef187573fba25693360_Out_3, _Property_e315cca8237c4aeaa6006f1490ed4403_Out_0, _Multiply_52cb6f8f19684735af5921b9bac1d729_Out_2);
            float4 _Property_900e40c854da4418934d2640c908ebe6_Out_0 = IsGammaSpace() ? LinearToSRGB(_MainColor) : _MainColor;
            float4 _Multiply_4967fa438aea44ff8642a4d8b715060e_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_RGBA_0, _Property_900e40c854da4418934d2640c908ebe6_Out_0, _Multiply_4967fa438aea44ff8642a4d8b715060e_Out_2);
            float4 _Add_e8b5f8cd78e24c65b14a786a7448ae98_Out_2;
            Unity_Add_float4(_Multiply_52cb6f8f19684735af5921b9bac1d729_Out_2, _Multiply_4967fa438aea44ff8642a4d8b715060e_Out_2, _Add_e8b5f8cd78e24c65b14a786a7448ae98_Out_2);
            float _OneMinus_f9079ceb10894d77a8f2de2f7c1205bf_Out_1;
            Unity_OneMinus_float(_Step_f4b42e180f27490ab7c403302fbfcb0d_Out_2, _OneMinus_f9079ceb10894d77a8f2de2f7c1205bf_Out_1);
            float _Multiply_cb91a17f0eaf4ea68277e2a1a706dd24_Out_2;
            Unity_Multiply_float_float(_OneMinus_f9079ceb10894d77a8f2de2f7c1205bf_Out_1, _SampleTexture2D_7be96d2d57b74de1a75055736850ab1e_A_7, _Multiply_cb91a17f0eaf4ea68277e2a1a706dd24_Out_2);
            float4 _Lerp_974ca3534a624ecd8c768199ff3a3df0_Out_3;
            Unity_Lerp_float4(float4(0, 0, 0, 0), _Add_e8b5f8cd78e24c65b14a786a7448ae98_Out_2, (_Multiply_cb91a17f0eaf4ea68277e2a1a706dd24_Out_2.xxxx), _Lerp_974ca3534a624ecd8c768199ff3a3df0_Out_3);
            surface.BaseColor = (_Lerp_974ca3534a624ecd8c768199ff3a3df0_Out_3.xyz);
            surface.Alpha = _Multiply_cb91a17f0eaf4ea68277e2a1a706dd24_Out_2;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}
