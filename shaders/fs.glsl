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

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	// L
	vec4 L= fPosition - lightPosition;
	// V
	vec4 V= fPosition - vec4(cameraPosition, 1.0);

    outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );

    outputColor *= V;
    vec3 LtoVector3= L.xyz;
    vec3 reflectedDirection=LtoVector3-2*dot(LtoVector3,normal.xyz)*normal.xyz;
    vec4 R = vec4(reflectedDirection.xyz,1);
}