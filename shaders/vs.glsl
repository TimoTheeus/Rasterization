#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position
			
uniform mat4 transform;

uniform vec3 viewDirection;
uniform vec3 lightPosition;
uniform mat4 rotationMatrix;

// shader output
out vec4 normal;			// transformed vertex normal
out vec2 uv;

out vec3 toLight;
out vec3 toCamera;
 
// vertex shader
void main()
{
	// transform vertex using supplied matrix
	gl_Position = transform * vec4(vPosition, 1.0);

	// forward normal and uv coordinate; will be interpolated over triangle
	normal = transform * vec4( vNormal, 0.0f );
	uv = vUV;

	toLight = (rotationMatrix * vec4( vPosition , 1.0 ) - vec4(lightPosition, 1.0)).xyz;
    toCamera = viewDirection;
}