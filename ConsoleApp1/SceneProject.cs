using System;
using System.Collections.Generic;
using System.Diagnostics;
using Calculations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    public class SceneObject
    {
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();

        protected Calculations.Matrix3 localTransform = new Calculations.Matrix3();
        protected Calculations.Matrix3 globalTransform = new Calculations.Matrix3();
        public SceneObject Parent
        {
            get { return parent; }
        }

        public SceneObject()
        {

            if (parent != null)
            {
                parent.RemoveChild(this);
            }
            foreach (SceneObject so in children)
            {
                so.parent = null;
            }

        }
        /// <summary>
        /// How many children an object has
        /// </summary>
        /// <returns></returns>
        public int GetChildCount()
        {
            return children.Count;
        }
        /// <summary>
        /// Gets the child
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SceneObject GetChild(int index)
        {
            return children[index];
        }
        /// <summary>
        /// Adds a child
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(SceneObject child)
        {
            // make sure it doesn't have a parent already
            Debug.Assert(child.parent == null);
            // assign "this as parent
            child.parent = this;
            // add new child to collection
            children.Add(child);
        }
        /// <summary>
        /// Removes the child.
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(SceneObject child)
        {
            if (children.Remove(child) == true)
            {
                child.parent = null;
            }
        }
        public virtual void OnUpdate(float delatTime)
        {
        }
        public virtual void OnDraw()
        {
        }
        public void Update(float deltaTime)
        {
            // run OnUpdate behaviour
            OnUpdate(deltaTime);
            // update children
            foreach (SceneObject child in children)
            {
                child.Update(deltaTime);
            }
        }
        public void Draw()
        {
            // run OnDraw behaviour
            OnDraw();
            // draw children
            foreach (SceneObject child in children)
            {
                child.Draw();
            }
        }
        public Calculations.Matrix3 LocalTransform
        {
            get { return localTransform; }
        }
        public Matrix3 GlobalTransform
        {
            get { return globalTransform; }
        }
        public void UpdateTransform()
        {
            if (parent != null)
                globalTransform = parent.globalTransform * localTransform;
            else
                globalTransform = localTransform;

            foreach (SceneObject child in children)
                child.UpdateTransform();
        }
        public void SetPosition(float x, float y)
        {
            localTransform.SetTranslation(x, y);
            UpdateTransform();
        }
        public void SetRotate(float radians)
        {
            localTransform.SetRotateZ(radians);
            UpdateTransform();
        }
        public void SetScale(float width, float height)
        {
            localTransform.SetScaled(width, height, 1);
            UpdateTransform();
        }
        public void Translate(float x, float y)
        {
            localTransform.Translate(x, y);
            UpdateTransform();
        }
        public void Rotate(float radians)
        {
            localTransform.RotateZ(radians);
            UpdateTransform();
        }
        public void Scale(float width, float height)
        {
            localTransform.Scale(width, height, 1);
            UpdateTransform();
        }

    }
}
