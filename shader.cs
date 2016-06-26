using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Template_P3 {

public class Shader
{
	// data members
	public int programID, vsID, fsID;
	public int attribute_vpos;
	public int attribute_vnrm;
	public int attribute_vuvs;
    public int uniform_mview;

    public int uniform_cpos;
    public int uniform_lpos;

    public int uniform_aint;
    public int uniform_dint;
    public int uniform_sint;

    public int uniform_arefl;
    public int uniform_drefl;
    public int uniform_srefl;
    public int uniform_mcol;
    public int uniform_mshiny;

        // constructor
        public Shader( String vertexShader, String fragmentShader )
	{
		// compile shaders
		programID = GL.CreateProgram();
		Load( vertexShader, ShaderType.VertexShader, programID, out vsID );
		Load( fragmentShader, ShaderType.FragmentShader, programID, out fsID );
		GL.LinkProgram( programID );
		Console.WriteLine( GL.GetProgramInfoLog( programID ) );

		// get locations of shader parameters
		attribute_vpos = GL.GetAttribLocation( programID, "vPosition" );
		attribute_vnrm = GL.GetAttribLocation( programID, "vNormal" );
		attribute_vuvs = GL.GetAttribLocation( programID, "vUV" );
		uniform_mview = GL.GetUniformLocation( programID, "transform" );

        uniform_cpos = GL.GetUniformLocation( programID, "cameraPosition");
        uniform_lpos = GL.GetUniformLocation( programID, "lightPosition");

        uniform_aint = GL.GetUniformLocation( programID, "lightAmbientIntensity");
        uniform_dint = GL.GetUniformLocation( programID, "lightDiffuseIntensity");
        uniform_sint = GL.GetUniformLocation( programID, "lightSpecularIntensity");

        //uniform_arefl = GL.GetUniformLocation( programID, "matAmbientReflectance");
        //uniform_drefl = GL.GetUniformLocation( programID, "matDiffuseReflectance");
        //uniform_srefl = GL.GetUniformLocation( programID, "matSpecularReflectance");
        uniform_mcol = GL.GetUniformLocation(programID, "matColor");
        uniform_mshiny = GL.GetUniformLocation( programID, "matShininess");


        }

	// loading shaders
	void Load( String filename, ShaderType type, int program, out int ID )
	{
		// source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
		ID = GL.CreateShader( type );
		using (StreamReader sr = new StreamReader( filename )) GL.ShaderSource( ID, sr.ReadToEnd() );
		GL.CompileShader( ID );
		GL.AttachShader( program, ID );
		Console.WriteLine( GL.GetShaderInfoLog( ID ) );
	}
}

} // namespace Template_P3
