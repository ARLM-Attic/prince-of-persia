sampler2D input : register(s0);

/// <summary>The color used to tint the input.</summary>
/// <defaultValue>White</defaultValue>
float4 FromColor : register(C0);

/// <summary>The color used to tint the input.</summary>
/// <defaultValue>Red</defaultValue>
float4 ToColor : register(C1);

/// <summary>Explain the purpose of this variable.</summary>
/// <minValue>05/minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>3.5</defaultValue>
float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
    float4 Color; 
    Color= tex2D(input , uv.xy); 
    if (Color.r == FromColor.r && Color.g == FromColor.g && Color.b == FromColor.b)
        return ToColor;
    return Color; 
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 main();
    }
}