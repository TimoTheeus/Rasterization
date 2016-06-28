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
        Mesh mesh, text, floor;                       // a mesh to draw using OpenGL
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Texture wood;							// texture to use for rendering
        SceneGraph root;
        Matrix4 camtransMatrix;
        Matrix4 camrotMatrixX, camrotMatrixY,fullcamRot;
        Vector3 camPos;
        Vector3 camRot;
        public static Vector3 viewDirection;
        float moveSpeed = 0.2f;
        float rotSpeed = 0.03f;

        // initialize
        public void Init()
        {
            // load teapot
            mesh = new Mesh(new Vector3(0,0,0), 1,  "../../assets/teapot.obj");
            floor = new Mesh(new Vector3(0, 0, 0), 1, "../../assets/floor.obj");
            text = new Mesh(new Vector3(0, 10, 0), 1, "../../assets/text.obj");
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
            mesh.AddChildNode(floor);
            mesh.AddChildNode(text);
            AddTeapots(mesh, 1);
        }

        public void AddTeapots(SceneGraph parent, int children)
        {
            int childs = children;
            Mesh m1 = new Mesh(parent.pos + new Vector3(-13/childs, 0, 0), 0.5f/childs, "../../assets/teapot.obj");
            Mesh m2 = new Mesh(parent.pos + new Vector3(13/childs, 0, 0), 0.5f/childs, "../../assets/teapot.obj");
            Mesh m3 = new Mesh(parent.pos + new Vector3(0, 0, -13/childs), 0.5f/childs, "../../assets/teapot.obj");
            Mesh m4 = new Mesh(parent.pos + new Vector3(0, 0, 13/childs), 0.5f/childs, "../../assets/teapot.obj");

            parent.AddChildNode(m1);
            parent.AddChildNode(m2);
            parent.AddChildNode(m3);
            parent.AddChildNode(m4);

            if (childs < 2)
            {
                AddTeapots(m1, childs+1);
                AddTeapots(m2, childs+1);
                AddTeapots(m3, childs+1);
                AddTeapots(m4, childs+1);
            }
        }
        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);

            //camera movement
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

            //camera rotation
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
            fullcamRot = camrotMatrixX * camrotMatrixY;
            viewDirection = fullcamRot.Row2.Xyz;
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

            Matrix4 transform2 = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            // render scene
            //foreach (Mesh m in mesh.children)
            //{
            //    m.Update(transform2);
            //}

            root.Update(transform);
            floor.Input(transform2);
            root.Input( Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a));
            root.Render(shader, wood);
        }
    }

} // namespace Template_P3