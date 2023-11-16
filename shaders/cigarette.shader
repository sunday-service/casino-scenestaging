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

		i.vBurnLevel = normalize(mul(CalculateInstancingObjectToWorldMatrix(v), float4(v.vPositionOs.xyz, 0.0)));

		return FinalizeVertex( i );
	}
}

//=========================================================================================================================

PS
{
    #include "common/pixel.hlsl"
	#include "procedural.hlsl"

	float g_flAshLevel <Attribute("AshLevel"); Default(0.125);>;
	float g_flBurnLevel <Attribute("BurnLevel"); Range(0, 0.69f); Default(0.5f);>;
	float3 g_vDirection <Attribute("Direction");>;

	RenderState(CullMode, NONE);

	float3 Inverse(float3 input) 
	{
		return clamp(1 - (2 * input), 0, 1);
	}

	float3 BurnLevelMask(PixelInput i) 
	{
		float burnLevel = g_flBurnLevel;

		float burnPosition = dot(i.vBurnLevel, g_vDirection);
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
		Material m = Material::From( i );

		m.Albedo = lerp(float4(1,0,0,1), float4(m.Albedo, 1), isFrontFace);
		m.Metalness = m.Metalness * isFrontFace;
		m.Roughness = m.Roughness * isFrontFace;
		m.AmbientOcclusion = m.AmbientOcclusion * isFrontFace;
		m.Normal = lerp(normalize(NormalWorldToTangent(i, g_vDirection)), m.Normal , isFrontFace);
		m.Emission = lerp(float3(1,0,0), m.Emission, isFrontFace);
        
		clip(BurnLevelMask(i) > 0 ? 1 : -1);

        return ShadingModelStandard::Shade( i, m );
    }
}