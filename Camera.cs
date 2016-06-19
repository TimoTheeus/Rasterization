using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template_P3
{
    class Camera
    {
        //member variables
        public Vector3 position;
        public Vector3 direction;
        public Vector3 viewDirection;
        Vector3 left, right, up, down;
        public Vector3 Left
        {
            get { return Vector3.Normalize(-Vector3.Cross(viewDirection, up)); }
        }
        public Vector3 Up
        {
            get { return Vector3.Cross(viewDirection, left); }
        }
        public Vector3 Right
        {
            get { return -Left; }
        }
        public Vector3 Down
        {
            get { return -Up; }
        }
        //half of total FOV
        public double fieldOfView;
        float appliedFieldOfView;
        float displayWidth;
        float displayHeight;

        public Camera(Vector3 pos, Vector3 dir, float screenWidth, float screenHeight)
        {
            position = pos;
            direction = dir;
            viewDirection = Vector3.Normalize(direction - position);
            //half of total FOV
            fieldOfView = Math.PI * 0.25;
            appliedFieldOfView = (float)(1 / Math.Tan(fieldOfView));
            displayWidth = screenWidth;
            displayHeight = screenHeight;
            left = new Vector3(-1, 0, 0);
            right = new Vector3(1, 0, 0);
            up = new Vector3(0, -1, 0);
            down = new Vector3(0, 1, 0);

        }
        public Vector3 Upperleft
        {
            get { return (Center + Up + Left); }
        }
        public Vector3 Upperright
        {
            get { return (Center + Up + Right); }
        }
        public Vector3 Downleft
        {
            get { return (Center + Down + Left); }
        }
        public Vector3 Center
        {
            get { return (position + appliedFieldOfView * viewDirection); }
        }
       
        //rotate the camera in different directions
        public void RotateRight(double timeElapsed)
        {
            Vector3 target = position + viewDirection;
            target += Right * 0.1f;
            viewDirection = Vector3.Normalize(target - position);
        }
        public void RotateUp(double timeElapsed)
        {
            Vector3 target = position + viewDirection;
            target += Up * 0.1f;
            viewDirection = Vector3.Normalize(target - position);
        }
        public void RotateLeft(double timeElapsed)
        {
            Vector3 target = position + viewDirection;
            target += Left * 0.1f;
            viewDirection = Vector3.Normalize(target - position);
        }
        public void RotateDown(double timeElapsed)
        {
            Vector3 target = position + viewDirection;
            target += Down * 0.1f;
            viewDirection = Vector3.Normalize(target - position);
        }
    }
}
