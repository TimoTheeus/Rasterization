using System.Diagnostics;
using OpenTK;
using System;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{

    class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh mesh,mesh2,mesh3,mesh4,mesh5,mesh6, floor;                       // a mesh to draw using OpenGL
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Texture wood;							// texture to use for rendering
        SceneGraph root;
        Matrix4 camtransMatrix;
        Matrix4 camrotMatrixX, camrotMatrixY;
        Vector3 camPos;
        Vector3 camRot;
        float moveSpeed = 0.2f;
        float rotSpeed = 0.03f;

        // initialize
        public void Init()
        {
            // load teapot
            mesh = new Mesh(new Vector3(0,0,0), 1,  "../../assets/teapot.obj");
            floor = new Mesh(new Vector3(0, 0, 0), 1, "../../assets/floor.obj");
            mesh2 = new Mesh(new Vector3(0, 8, 0), 0.1f, "../../assets/teapot.obj");
            mesh3 = new Mesh(new Vector3(2, 6, 2), 0.1f, "../../assets/teapot.obj");
            mesh4 = new Mesh(new Vector3(-2, 6, -2), 0.1f, "../../assets/teapot.obj");
            mesh5 = new Mesh(new Vector3(-2, 6, 2), 0.1f, "../../assets/teapot.obj");
            mesh6 = new Mesh(new Vector3(2, 6, -2), 0.1f, "../../assets/teapot.obj");
            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            root = new SceneGraph(new Vector3(0,1,0), 1);
            root.AddChildNode(mesh);
            mesh.AddChildNode(mesh2);
            mesh.AddChildNode(mesh3);
            mesh.AddChildNode(mesh4);
            mesh.AddChildNode(mesh5);
            mesh.AddChildNode(mesh6);
            mesh.AddChildNode(floor);
        }
        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            if (Keyboard.GetState().IsKeyDown(Key.S))
            {
                camPos.Z -= moveSpeed * timer.ElapsedMilliseconds * (float)Math.Cos(camRot.Y) * (float)Math.Cos(camRot.X);
                camPos.X -= moveSpeed * timer.ElapsedMilliseconds * (float)Math.Sin(camRot.Y) * -(float)Math.Cos(camRot.X);
                camPos.Y -= moveSpeed * timer.ElapsedMilliseconds * (float)Math.Sin(camRot.X);

            }
            if (Keyboard.GetState().IsKeyDown(Key.W))
            {
                camPos.Z += moveSpeed * timer.ElapsedMilliseconds * (float)Math.Cos(camRot.Y) * (float)Math.Cos(camRot.X);
                camPos.X += moveSpeed * timer.ElapsedMilliseconds * (float)Math.Sin(camRot.Y) * -(float)Math.Cos(camRot.X);
                camPos.Y += moveSpeed * timer.ElapsedMilliseconds * (float)Math.Sin(camRot.X);
            }
            if (Keyboard.GetState().IsKeyDown(Key.A))
            {
                camPos.X += moveSpeed * timer.ElapsedMilliseconds * (float)Math.Cos(camRot.Y) * (float)Math.Cos(camRot.X);
                camPos.Z += moveSpeed * timer.ElapsedMilliseconds * (float)Math.Sin(camRot.Y) * (float)Math.Cos(camRot.X);
            }               
            if (Keyboard.GetState().IsKeyDown(Key.D))
            {
                camPos.X -= moveSpeed * timer.ElapsedMilliseconds * (float)Math.Cos(camRot.Y) * (float)Math.Cos(camRot.X);
                camPos.Z -= moveSpeed * timer.ElapsedMilliseconds * (float)Math.Sin(camRot.Y) * (float)Math.Cos(camRot.X);
            }               
            camtransMatrix = Matrix4.CreateTranslation(0 + camPos.X, -4 + camPos.Y, -15 + camPos.Z);


            if (Keyboard.GetState().IsKeyDown(Key.Left))
                camRot.Y -= rotSpeed;
            if (Keyboard.GetState().IsKeyDown(Key.Right))
                camRot.Y += rotSpeed;
            if (Keyboard.GetState().IsKeyDown(Key.Up))
                camRot.X -= rotSpeed;
            if (Keyboard.GetState().IsKeyDown(Key.Down))
                camRot.X += rotSpeed;

            camrotMatrixY = Matrix4.CreateRotationY(camRot.Y);
            camrotMatrixX = Matrix4.CreateRotationX(camRot.X);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // prepare matrix for rotate
            Matrix4 transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            //camera thingy
            transform *= camtransMatrix;
            transform *= camrotMatrixY;
            transform *= camrotMatrixX;
            //fov aspect ratio, near Z plane, far Z plane
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000f);
            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            Matrix4 transform2 = Matrix4.CreateFromAxisAngle(new Vector3(0, 1f, 0), a);

            GL.Uniform3(shader.uniform_cpos, camPos);
            // render scene
            //mesh.Render( shader, transform, wood );
            //floor.Render( shader, transform, wood );
            floor.Update(transform2);
            root.Update(transform);
            root.Input( Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a));
            floor.Input(transform2);
            root.Render(shader, wood);
            //root.Input(transform);

        }
    }

} // namespace Template_P3