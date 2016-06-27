#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal

in vec3 toLight;
in vec3 toCamera;

uniform sampler2D pixels;		// texture sampler

uniform vec3 lightAmbientIntensity;
uniform vec3 lightDiffuseIntensity;
uniform vec3 lightSpecularIntensity;

uniform vec3 matAmbientReflectance;
uniform vec3 matDiffuseReflectance;
uniform vec3 matSpecularReflectance;
uniform float matShininess; // = 64


// shader output
out vec4 outputColor;


// fragment shader
vec3 ambientLighting()
{
	return matAmbientReflectance * lightAmbientIntensity;
}

vec3 diffuseLighting(in vec3 L, in vec3 N)
{
	float NdotL = clamp(dot(normalize(-N),normalize(L)), 0, 1);
	vec3 cDiff = matDiffuseReflectance;
	vec3 lDiff = lightDiffuseIntensity/dot(L,L);
	return  vec3(clamp(cDiff.x*NdotL*lDiff.x, 0, 1), clamp(cDiff.y*NdotL*lDiff.y, 0, 1), clamp(cDiff.z*NdotL*lDiff.z, 0, 1));
}

vec3 specularLighting(in vec3 N, in vec3 L, in vec3 V)
{
	float NdotL = clamp(dot(normalize(-N),normalize(L)), 0, 1);
	vec3 R = normalize(reflect(-L,N));
	float RdotV = max(dot(R,normalize(V)),0);
	vec3 Is= matSpecularReflectance *lightSpecularIntensity*pow(RdotV,matShininess);
	return Is;
}

void main()
{
	vec3 L = toLight;
	vec3 V = toCamera;
	vec3 N = normal.xyz;

	vec3 Iamb = ambientLighting();
	vec3 Idif = diffuseLighting(L, N);
	vec3 Ispe = specularLighting(N, L, V);

	vec4 diffuseColor = texture( pixels, uv );

	vec3 temp = vec3(clamp((Iamb.x + Idif.x+Ispe.x),0,1),clamp((Iamb.y + Idif.y+Ispe.y),0,1),clamp((Iamb.z + Idif.z+Ispe.z),0,1));
	outputColor=vec4(temp, 1.0f);
}