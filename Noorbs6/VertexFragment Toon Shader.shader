Shader "Custom/VertexFragment Toon Shader" {
	Properties {
	 	_MainTex ("Base (RGB)", 2D) = "white" {}
        _Bump ("Bump", 2D) = "bump" {}
		_Outline ("Outline", Range(0.0, 1.0)) = 0.0
		_Ramp ("Ramp", 2D) = "white" {}
		_ColorMerge ("ColorMerge", Range(0.1,20)) = 8
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			Tags { "LightMode"="ForwardBase" }  
			// 剔除模型正面，只渲染背面
			Cull Front
			Lighting Off
			//ZWrite Off
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct v2f
			{
				float4 pos : POSITION;
			};
			
			float _Outline;
			sampler2D _Ramp;
			sampler2D _MainTex;
			sampler2D _Bump;
			float _ColorMerge;
									
			//v2f vert(a2v v)
			//{
			//	v2f o;
			//	o.pos = mul( UNITY_MATRIX_MVP, v.vertex + (float4(v.normal, 0) * _Outline));
			//	return o;
			//}
			
			v2f vert (a2v v)
			{
     			v2f o;
     			float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
     			float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal); 
     			normal.z = -0.4;
     			pos = pos + float4(normalize(normal),0) * _Outline;
     			o.pos = mul(UNITY_MATRIX_P, pos);
 
     			return o;
 			}

			
			float4 frag(v2f i) : COLOR
			{ 
				return float4(0,0,0,1);
			}	
			
			ENDCG
		}
		
		 Pass {
 
            Tags { "LightMode"="ForwardBase" }
            Cull Back
            Lighting On
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            uniform float4 _LightColor0;
 
            sampler2D _MainTex;
            sampler2D _Bump;
            float _Outline;
			sampler2D _Ramp;
			float _ColorMerge;
            float4 _MainTex_ST;
            float4 _Bump_ST;
 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
 
            };
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 lightDirection : TEXCOORD2;
 
            };
 
            v2f vert (a2v v)
            {
                v2f o;
                TANGENT_SPACE_ROTATION;
 
                o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex); 
                o.uv2 = TRANSFORM_TEX (v.texcoord, _Bump);
                return o;
            }
 
            //float4 frag(v2f i) : COLOR 
            //{
            //    float4 c = tex2D (_MainTex, i.uv); 
            //    float3 n =  UnpackNormal(tex2D (_Bump, i.uv2));
 
            //    float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
 
            //    float lengthSq = dot(i.lightDirection, i.lightDirection);
            //    float atten = 1.0 / (1.0 + lengthSq);
                //光源的入射角                 
            //    float diff = saturate (dot (n, normalize(i.lightDirection)));  
            //    lightColor += _LightColor0.rgb * (diff * atten);
            //    c.rgb = lightColor * c.rgb * 2;
            //    return c;
 
            //}
            float4 frag(v2f i) : COLOR 
			{
    			// 根据uv坐标从纹理中获得对应像素值
   			 	float4 c = tex2D (_MainTex, i.uv); 
    			// 降低颜色种类
   				c.rgb = (floor(c.rgb*_ColorMerge)/_ColorMerge);
 
    			//从bump纹理中得到对应像素的法向
    			float3 n =  UnpackNormal(tex2D (_Bump, i.uv2));
 
    			//获得漫射光颜色
    			float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
 
    			//计算出光源距离
			    float lengthSq = dot(i.lightDirection, i.lightDirection);
		    	//根据计算出的光源位置计算光强的衰减
    			float atten = 1.0 / (1.0 + lengthSq);
    			//光的入射角
    			float diff = saturate (dot (n, normalize(i.lightDirection))); 
    			//利用渐变纹理
    			diff = tex2D(_Ramp, float2(diff, 0.5));
    			//根据入射角，光衰减得到最终光照亮度
    			lightColor += _LightColor0.rgb * (diff * atten);
    			//将光照亮度与本身颜色相乘，得到最终颜色
    			c.rgb = lightColor * c.rgb * 2;
    			return c;
			}
 
            ENDCG
        }
 
        Pass {
 
            Cull Back
            Lighting On
            Tags { "LightMode"="ForwardAdd" }
            Blend One One
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            uniform float4 _LightColor0;
 
            sampler2D _MainTex;
            sampler2D _Bump;
            float _Outline;
			sampler2D _Ramp;
			float _ColorMerge;
            float4 _MainTex_ST;
            float4 _Bump_ST;
 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
 
            };
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 lightDirection : TEXCOORD2;
            };
 
            v2f vert (a2v v)
            {
                v2f o;
                TANGENT_SPACE_ROTATION;
 
                o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex); 
                o.uv2 = TRANSFORM_TEX (v.texcoord, _Bump);
                return o;
            }
 
            //float4 frag(v2f i) : COLOR 
            //{
            //    float4 c = tex2D (_MainTex, i.uv); 
            //    float3 n =  UnpackNormal(tex2D (_Bump, i.uv2));
 
            //    float3 lightColor = float3(0.0, 0.0, 0.0);
 
            //    float lengthSq = dot(i.lightDirection, i.lightDirection);
            //    float atten = 1.0 / (1.0 + lengthSq * unity_LightAtten[0].z);
                //光源的入射角
            //    float diff = saturate (dot (n, normalize(i.lightDirection)));  
            //    lightColor += _LightColor0.rgb * (diff * atten);
            //    c.rgb = lightColor * c.rgb * 2;
            //    return c;
 
            //}
            float4 frag(v2f i) : COLOR 
			{
    			// 根据uv坐标从纹理中获得对应像素值
   			 	float4 c = tex2D (_MainTex, i.uv); 
    			// 降低颜色种类
   				c.rgb = (floor(c.rgb*_ColorMerge)/_ColorMerge);
 
    			//从bump纹理中得到对应像素的法向
    			float3 n =  UnpackNormal(tex2D (_Bump, i.uv2));
 
    			//获得漫射光颜色
    			float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
 
    			//计算出光源距离
			    float lengthSq = dot(i.lightDirection, i.lightDirection);
		    	//根据计算出的光源位置计算光强的衰减
    			float atten = 1.0 / (1.0 + lengthSq);
    			//光的入射角
    			float diff = saturate (dot (n, normalize(i.lightDirection))); 
    			//利用渐变纹理
    			diff = tex2D(_Ramp, float2(diff, 0.5));
    			//根据入射角，光衰减得到最终光照亮度
    			lightColor += _LightColor0.rgb * (diff * atten);
    			//将光照亮度与本身颜色相乘，得到最终颜色
    			c.rgb = lightColor * c.rgb * 2;
    			return c;
			}
 
            ENDCG
        }
	} 
	FallBack "Diffuse"
}
