#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 fPosition;

uniform sampler2D pixels;		// texture sampler

uniform vec4 ambientCol;
uniform vec4 materialCol;
uniform vec4 specularCol;
uniform vec4 lightPosition;
uniform vec3 cameraPosition;
const vec4 lightCol=vec4(50f,50f,50f,1.0f);

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
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
}