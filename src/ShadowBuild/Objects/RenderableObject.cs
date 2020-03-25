﻿using ShadowBuild.Exceptions;
using ShadowBuild.Input.Mouse;
using ShadowBuild.Objects.Animationing;
using ShadowBuild.Objects.Texturing;
using ShadowBuild.Objects.Texturing.Image;
using ShadowBuild.Rendering;
using System.Collections.Generic;
using System.Windows;

namespace ShadowBuild.Objects
{
    public abstract class RenderableObject : _2DobjectResizable
    {
        public static List<RenderableObject> All { get; private set; } = new List<RenderableObject>();

        public string Name;

        
        public bool Visible = true;
        public Layer RenderLayer { get; protected set; }

        public RenderableObject Parent { get; private set; }
        public List<RenderableObject> Children
        {
            get
            {
                List<RenderableObject> toReturn = new List<RenderableObject>();
                foreach (RenderableObject obj in All)
                {
                    if (obj.Parent == this) toReturn.Add(obj);
                }
                return toReturn;
            }
            private set { }
        }
        public bool MouseOver
        {
            get
            {
                if (Mouse.LockCurosr) return false;
                Point p = new Point(
                    Mouse.Position.X + Camera.Default.StartPosition.X,
                    Mouse.Position.Y + Camera.Default.StartPosition.Y
                );
                return CheckPointInside(p);
            }
        }


        public RenderableObject(string name)
        {
            this.Name = name;
            this.RenderLayer = Layer.Default;
            this.SetPosition(0, 0);
            this.Visible = true;
            All.Add(this);
        }
        public RenderableObject(string name, Layer layer)
        {
            this.Name = name;
            this.RenderLayer = layer;
            this.SetPosition(0, 0);
            this.Visible = true;
            All.Add(this);

        }
        protected RenderableObject() { }

        public static RenderableObject Get(string name)
        {
            foreach (RenderableObject o in All)
                if (o.Name == name) return o;
            throw new ObjectException("Could not find object \"" + name + "\"");
        }
        public static T Get<T>(string name) where T : RenderableObject
        {
            foreach (RenderableObject o in All)
                if (o.Name == name && o is T) return (T)o;
            throw new ObjectException("Could not find object \"" + name + "\" with type \"" + typeof(T).FullName + "\"");
        }
        public void SetParent(RenderableObject obj)
        {
            this.Parent = obj;
        }
        public List<RenderableObject> GetAllGrandchildren()
        {
            List<RenderableObject> objs = new List<RenderableObject>();

            foreach (RenderableObject child in this.Children)
            {
                List<RenderableObject> toMerge = child.GetAllGrandchildren();
                toMerge.Add(child);
                foreach (RenderableObject toMergeObj in toMerge)
                    objs.Add(toMergeObj);

            }
            return objs;
        }
        public bool IsChildOf(RenderableObject parent)
        {
            if (this.Parent == parent) return true;
            else if (this.Parent == null) return false;
            else return this.Parent.IsChildOf(parent);
        }

        public Point GetGlobalPosition()
        {
            Point tmpPosition = this.Position;
            if (this.Parent != null)
            {
                tmpPosition = new Point(tmpPosition.X + this.Parent.GetGlobalPosition().X, tmpPosition.Y + this.Parent.GetGlobalPosition().Y);
            }
            return tmpPosition;

        }

        public abstract Point GetStartPosition();
        public abstract void Render(System.Drawing.Graphics g, Point startPosition);

        public abstract bool CheckPointInside(Point p);
    }
}