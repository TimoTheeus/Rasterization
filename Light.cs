using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template_P3
{
    public class Light
    {
        //member variables
        public Vector3 location;
        public Vector3 ambientIntensity;
        public Vector3 diffuseIntensity;
        public Vector3 specularIntensity;


        //constructor
        public Light(Vector3 loc, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            location = loc;
            ambientIntensity = ambient;
            diffuseIntensity = diffuse;
            specularIntensity = specular;
        }
    }
}