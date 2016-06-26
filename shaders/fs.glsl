#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 fPosition;

uniform sampler2D pixels;		// texture sampler



uniform vec3 lightAmbientIntensity; // = vec3(0.6, 0.3, 0)
uniform vec3 lightDiffuseIntensity; // = vec3(1, 0.5, 0)
uniform vec3 lightSpecularIntensity; //// = vec3(0, 1, 0)

uniform vec3 matColor;
//uniform vec3 matAmbientReflectance; // = vec3(1, 1, 1)
//uniform vec3 matDiffuseReflectance; // = vec3(1, 1, 1)
//uniform vec3 matSpecularReflectance; // = vec3(1, 1, 1)
uniform float matShininess; // = 64

in vec3 toLight;
in vec3 toCamera;



// shader output
out vec4 outputColor;

// fragment shader

vec3 ambientLighting()
{
	return matColor * lightAmbientIntensity;
}

vec3 diffuseLighting(in vec3 L, in vec3 V, in vec3 N)
{

	float NdotL = clamp(dot(normalize(-N),normalize(L)), 0, 1);
	vec3 R = normalize(L-2*NdotL*N);
	float phongExp = 2;

	vec3 cDiff = matColor+matColor*pow(dot(-V,R), phongExp);
	float lDiff = 1/dot(L,L);
	return  vec3(clamp(cDiff.x*NdotL*lDiff, 0, 1), clamp(cDiff.y*NdotL*lDiff, 0, 1), clamp(cDiff.z*NdotL*lDiff, 0, 1));
}

//vec3 specularLighting(in vec3 N, in vec3 L, in vec3 V)
//{
//	float specularTerm = 0;
//
//	if(dot(N, L) > 0)
//	{
//		vec3 H = normalize(L + V);
//		specularTerm = pow(dot(N, H), matShininess);
//	}
//	return matSpecularReflectance * lightSpecularIntensity * specularTerm;
//}

void main()
{
	vec3 L = toLight;
	vec3 V = toCamera;
	vec3 N = normal.xyz;

	vec3 Iamb = ambientLighting();
	vec3 Idif = diffuseLighting(L, V, N);
	//vec3 Ispe = specularLighting(N, L, V);

	vec4 diffuseColor = texture( pixels, uv ) + 0.5f * (normal.xyz, 1);

	outputColor.xyz = diffuseColor.xyz * (Iamb + Idif);
	outputColor.a = 1;
}