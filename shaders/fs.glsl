#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal

uniform sampler2D pixels;		// texture sampler

uniform vec3 lightAmbientIntensity; // = vec3(0.6, 0.3, 0)
uniform vec3 lightDiffuseIntensity; // = vec3(1, 0.5, 0)
uniform vec3 lightSpecularIntensity; //// = vec3(0, 1, 0)

uniform vec3 matColor;
uniform vec3 matAmbientReflectance; // = vec3(1, 1, 1)
uniform vec3 matDiffuseReflectance; // = vec3(1, 1, 1)
uniform vec3 matSpecularReflectance; // = vec3(1, 1, 1)
uniform float matShininess; // = 64

in vec3 toLight;
in vec3 toCamera;


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
	vec3 cDiff = matColor;
	vec3 lDiff = lightDiffuseIntensity/dot(L,L);
	return  vec3(clamp(cDiff.x*NdotL*lDiff.x, 0, 1), clamp(cDiff.y*NdotL*lDiff.y, 0, 1), clamp(cDiff.z*NdotL*lDiff.z, 0, 1));
}

vec3 specularLighting(in vec3 N, in vec3 L, in vec3 V)
{
	float phongExp=1f;
	float NdotL = clamp(dot(normalize(-N),normalize(L)), 0, 1);
	vec3 R = normalize(L-2*NdotL*N);//normalize(reflect(-L,N))
	float RdotV = clamp(dot(R,normalize(V)),0,1);
	vec3 Is= matSpecularReflectance *lightSpecularIntensity*pow(RdotV,phongExp);
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

	vec3 temp = vec3(clamp((Iamb.x + Idif.x+Ispe.x)*diffuseColor.x,0,1),clamp((Iamb.y + Idif.y+Ispe.y)*diffuseColor.y,0,1),clamp((Iamb.z + Idif.z+Ispe.z)*diffuseColor.z,0,1));//diffuseColor.xyz * (Iamb + Idif+Ispe);
	outputColor =vec4(temp, 1.0);
}