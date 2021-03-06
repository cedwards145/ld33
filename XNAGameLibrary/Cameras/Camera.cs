﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAGameLibrary.Cameras
{
    public class Camera
    {
        private Vector2 position;
        public Vector2 Position 
        {
            get
            {
                return position;
            }
            protected set
            {
                position = value;
            }
        }

        public float MoveSpeed { get; set; }

        public float ZoomSpeed { get; set; }
        public float Zoom { get; set; }

        public Matrix TransformationMatrix
        {
            get
            {
                return Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(position.X * -1, position.Y * -1, 0);
            }
        }

        public Camera()
        {
            Zoom = 1;
            ZoomSpeed = 0.02f;
            MoveSpeed = 3f;
        }

        public void Move(Point moveValue)
        {
            Move(moveValue.X, moveValue.Y);    
        }

        public void Move(int x, int y)
        {
            position.X += x * MoveSpeed;
            position.Y += y * MoveSpeed;
        }

        public void ZoomIn()
        {
            Zoom = Zoom + ZoomSpeed;
        }

        public void ZoomOut()
        {
            Zoom = Zoom - ZoomSpeed;
        }

        public void ZoomAmount(int direction)
        {
            Zoom = Zoom + ZoomSpeed * direction;
        }
    }
}
