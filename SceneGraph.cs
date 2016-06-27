using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Template_P3
{
    public class SceneGraph
    {
        public List<SceneGraph> children;
        public SceneGraph parent;
        public Matrix4 transform;
        public Matrix4 viewMatrix;
        public Matrix4 originalMatrix;
        public Matrix4 rotationMatrix;
        public Light singleLight;

        public SceneGraph(Vector3 position, float scale)
        {
            children = new List<SceneGraph>();
            viewMatrix = Matrix4.CreateTranslation(position / scale) * Matrix4.CreateScale(scale);
            rotationMatrix = Matrix4.CreateTranslation(position / scale) * Matrix4.CreateScale(scale);
            originalMatrix = viewMatrix;
            singleLight = new Light(new Vector3(0f,7f,-5f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(200f, 200f, 200f), new Vector3(0.7f, 0.7f, 0.7f));
        }
        //Update the viewmatrix of parent and children based on a transformMatrix
        public virtual void Update(Matrix4 transformMatrix)
        {
            this.viewMatrix *= transformMatrix;
            //transform the matrix
            foreach (SceneGraph child in this.children)
            {
                //transform the viewMatrix of all children
                child.Update(transformMatrix);
            }
        }
        //Add child and make this its parent
        public void AddChildNode(SceneGraph child)
        {
            children.Add(child);
            child.parent = this;
        }
        //Render the parent and childs
        public virtual void Render(Shader shader, Texture texture)
        {
            foreach (SceneGraph child in this.children)
            {
                child.Render(shader, texture);
            }
            //reset the viewmatrices to the original matrix
            ResetViewMatrices();
        }
        public void Input(Matrix4 transformMatrix)
        {
            this.rotationMatrix *= transformMatrix;
            foreach (SceneGraph child in this.children)
            {
                child.Input(transformMatrix);
            }
        }
        public void ResetViewMatrices()
        {
            this.viewMatrix = originalMatrix;
            this.rotationMatrix = originalMatrix;
            foreach (SceneGraph child in this.children)
            {
                child.ResetViewMatrices();
            }
        }
    }
}