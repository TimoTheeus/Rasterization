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
        public SceneGraph()
        {
            children = new List<SceneGraph>();
            viewMatrix = Matrix4.Identity;
            originalMatrix = viewMatrix;
        }
        //Update the viewmatrix of parent and children based on a transformMatrix
        public virtual void Update(Matrix4 transformMatrix)
        {
            //transform the matrix
            foreach(SceneGraph child in this.children)
            {
                //transform the viewMatrix of all children
                child.viewMatrix *= transformMatrix;
                child.Update(transformMatrix);
            }
        }
        //Add child and make this its parent
        public void AddChildNode(Mesh child)
        {
            children.Add(child);
            child.parent = this;
        }
        //Render the parent and childs
        public virtual void Render(Shader shader, Texture texture)
        {
            foreach (SceneGraph child in this.children)
            {
                child.Render(shader,texture);
            }
            //reset the viewmatrices to the original matrix
            ResetViewMatrices();
        }
        public void Input(Matrix4 transformMatrix)
        {
            foreach (SceneGraph child in this.children)
            {
                child.Input(transformMatrix);
            }
        }
        public void ResetViewMatrices()
        {
            this.viewMatrix = originalMatrix;
            foreach (SceneGraph child in this.children)
            {
                child.ResetViewMatrices();
            }
        }
    }
}
