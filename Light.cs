using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template_P3
{
    class Light
    {
        //member variables
        public Vector3 location;
        public Vector3 intensity;
       

        //constructor
        public Light(Vector3 loc, Vector3 i)
        {
            location = loc;
            intensity = i;
        }
    }
}
