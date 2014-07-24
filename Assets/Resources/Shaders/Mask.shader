Shader "Custom/Mask" {
	Properties 
    {
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _Cutoff ("Base Alpha cutoff", Range (0, .9)) = .5
    }
    SubShader {
        // Render the mask after regular geometry, but before masked geometry and
        // transparent things. 
        Tags {"Queue" = "Geometry+10"}
        // Don't draw in the RGBA channels; just the depth buffer
        ColorMask 0
        ZWrite On
        Pass 
        {
            AlphaTest LEqual [_Cutoff]       
            SetTexture [_MainTex] {
                combine texture * primary, texture
            }
        }
    }
}
