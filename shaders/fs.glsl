#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 fPosition;

uniform sampler2D pixels;		// texture sampler

<<<<<<< HEAD


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


=======
uniform vec4 ambientCol;
uniform vec4 materialCol;
uniform vec4 specularCol;
uniform vec4 lightPosition;
uniform vec3 cameraPosition;
const vec4 lightCol=vec4(50f,50f,50f,1.0f);
>>>>>>> origin/master

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
<<<<<<< HEAD
	vec3 L = toLight;
	vec3 V = toCamera;
	vec3 N = normal.xyz;

	vec3 Iamb = ambientLighting();
	vec3 Idif = diffuseLighting(L, V, N);
	//vec3 Ispe = specularLighting(N, L, V);

	vec4 diffuseColor = texture( pixels, uv ) + 0.5f * (normal.xyz, 1);

	outputColor.xyz = diffuseColor.xyz * (Iamb + Idif);
	outputColor.a = 1;
=======
	float phongExponent=2;
	// L
	vec3 L= (fPosition - lightPosition).xyz;
	// V
	vec3 V= cameraPosition-fPosition.xyz;

	//NdotL
	float NdotL=dot(normal.xyz,L);
        //cAmbient
	vec4 cAmbient=materialCol*ambientCol;

	//R
   	vec3 R=L-2*NdotL*normal.xyz;

  	//cDiff (PHONG BRDF)
	vec4 cDiff=materialCol+materialCol*pow(dot(V,R),phongExponent);

	//lDiff(DISTANCE ATTENUATION)
	float lDiff= dot(L,L);
	vec4 diffuseCol= cDiff*NdotL*lDiff;

    	outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
    	outputColor = cAmbient;  	
>>>>>>> origin/master
}