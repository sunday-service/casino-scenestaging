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

	float g_flBurnAmount <Attribute("BurnAmount"); Default(.125f);>;

    float4 MainPs(PixelInput i, bool isFrontFace : SV_IsFrontFace)  : SV_Target0
    {
		float burnAmount = g_flBurnAmount;

		float burnPosition = i.vBurnLevel.z;
        float burnEdge = 1 - (2 * burnAmount);
        
		float burnMask = step(burnPosition, burnEdge);

		float3 backFace = float3(burnMask, burnMask, burnMask);
		float3 frontFace = float3(0.6,0.0,0.0);

		float4 result = float4(lerp(frontFace, backFace, isFrontFace), 1.0);
        
		clip(burnMask > 0 ? 1 : -1);

        return result;
    }
}