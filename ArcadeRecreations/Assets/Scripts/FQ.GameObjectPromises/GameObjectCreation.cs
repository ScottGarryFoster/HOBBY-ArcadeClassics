using UnityEngine;

namespace FQ.GameObjectPromises
{
    /// <summary>
    /// Ability to create game objects.
    /// </summary>
    public class GameObjectCreation : IObjectCreation
    {
        /// <summary>
        ///   <para>Clones the object original and returns the clone.</para>
        /// </summary>
        /// <param name="original">An existing object that you want to make a copy of.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <param name="instantiateInWorldSpace">When you assign a parent Object, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
        /// <returns>
        ///   <para>The instantiated clone.</para>
        /// </returns>
        public Object Instantiate(Object original, Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(original, position, rotation);
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
        {
            return Object.Instantiate(original, position, rotation);
        }

        /// <summary>
        ///   <para>Clones the object original and returns the clone.</para>
        /// </summary>
        /// <param name="original">An existing object that you want to make a copy of.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <param name="instantiateInWorldSpace">When you assign a parent Object, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
        /// <returns>
        ///   <para>The instantiated clone.</para>
        /// </returns>
        public Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return Object.Instantiate(original, position, rotation, parent);
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            return Object.Instantiate(original, position, rotation, parent);
        }

        /// <summary>
        ///   <para>Clones the object original and returns the clone.</para>
        /// </summary>
        /// <param name="original">An existing object that you want to make a copy of.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <param name="instantiateInWorldSpace">When you assign a parent Object, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
        /// <returns>
        ///   <para>The instantiated clone.</para>
        /// </returns>
        public Object Instantiate(Object original)
        {
            return Object.Instantiate(original);
        }

        public T Instantiate<T>(T original) where T : Object
        {
            return Object.Instantiate(original);
        }

        /// <summary>
        ///   <para>Clones the object original and returns the clone.</para>
        /// </summary>
        /// <param name="original">An existing object that you want to make a copy of.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <param name="instantiateInWorldSpace">When you assign a parent Object, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
        /// <returns>
        ///   <para>The instantiated clone.</para>
        /// </returns>
        public Object Instantiate(Object original, Transform parent)
        {
            return Object.Instantiate(original, parent);
        }

        public T Instantiate<T>(T original, Transform parent) where T : Object
        {
            return Object.Instantiate(original, parent);
        }

        /// <summary>
        ///   <para>Clones the object original and returns the clone.</para>
        /// </summary>
        /// <param name="original">An existing object that you want to make a copy of.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <param name="instantiateInWorldSpace">When you assign a parent Object, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
        /// <returns>
        ///   <para>The instantiated clone.</para>
        /// </returns>
        public Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace)
        {
            return Object.Instantiate(original, parent, instantiateInWorldSpace);
        }

        public T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
        {
            return Object.Instantiate(original, parent, worldPositionStays);
        }
    }
}