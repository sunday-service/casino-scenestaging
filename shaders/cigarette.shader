//=========================================================================================================================

HEADER
{
    Description = "Ciagrette Shader for S&Box";
}

//=========================================================================================================================

FEATURES
{
    #include "common/features.hlsl"
}

//=========================================================================================================================

MODES
{
    VrForward();                                               // Indicates this shader will be used for main rendering
    ToolsVis( S_MODE_TOOLS_VIS );                              // Ability to see in the editor
    ToolsWireframe("vr_tools_wireframe.shader");               // Allows for mat_wireframe to work
    ToolsShadingComplexity("tools_shading_complexity.shader"); // Shows how expensive drawing is in debug view
}

//=========================================================================================================================

COMMON
{
    #include "common/shared.hlsl"
}

//=========================================================================================================================

struct VertexInput
{
    #include "common/vertexinput.hlsl"
};

//=========================================================================================================================

struct PixelInput
{
    #include "common/pixelinput.hlsl"

	float3 vBurnLevel : POSITION < Semantic( PosXyz ); >;
};

//=========================================================================================================================

VS
{
    #include "common/vertex.hlsl"

	PixelInput MainVs( VertexInput v )
	{
		PixelInput i = ProcessVertex( v );

		float3 vPositionWs = normalize(mul(CalculateInstancingObjectToWorldMatrix(v), float4(v.vPositionOs.xyz, 0.0)));

		i.vBurnLevel = vPositionWs;

		return FinalizeVertex( i );
	}
}

//=========================================================================================================================

PS
{
    #include "common/pixel.hlsl"

	float g_flAshLevel <Attribute("AshLevel"); Default(0.125);>;
	float g_flBurnLevel <Attribute("BurnLevel"); Default(0.75f);>;

	float3 Inverse(float3 input) 
	{
		return clamp(1 - (2*input), 0, 1);
	}

	float3 BurnLevelMask(PixelInput i) 
	{
		float burnLevel = g_flBurnLevel;

		float burnPosition = i.vBurnLevel.z;
        float burnEdge = 1 - (2 * burnLevel);
        
		return step(burnPosition, burnEdge);
	}

	float3 AshLevelMask(PixelInput i) 
	{
		float3 mask = Inverse(BurnLevelMask(i));

		return mask;
	}

    float4 MainPs(PixelInput i, bool isFrontFace : SV_IsFrontFace)  : SV_Target0
    {
		float3 burnMaskColor = BurnLevelMask(i) * float3(1, 0, 0);
		float3 ashMaskColor = AshLevelMask(i) * float3(0, 1, 0);

		float3 backFace = burnMaskColor + ashMaskColor; 
		float3 frontFace = float3(0.6,0.0,0.0);

		float4 result = float4(lerp(frontFace, backFace, isFrontFace), 1.0);
        
		//clip(BurnLevelMask(i) > 0 ? 1 : -1);
		//clip(AshLevelMask(i) > 0 ? 1 : -1);

        return result;
    }
}