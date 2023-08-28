using System.Collections.Generic;
using UnityEngine;

namespace FQ.GameObjectPromises
{
    /// <summary>
    /// Ability to create game objects and capture the creations.
    /// </summary>
    public class StubObjectCreation : IObjectCreation
    {
        /// <summary>
        /// All the game objects created.
        /// </summary>
        public readonly List<Object> CreatedGameObjects;

        public StubObjectCreation()
        {
            CreatedGameObjects = new List<Object>(16);
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
        public Object Instantiate(Object original, Vector3 position, Quaternion rotation)
        {
            Object created = Object.Instantiate(original, position, rotation);
            CreatedGameObjects.Add(created);
            return created;
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
        {
            T created = Object.Instantiate(original, position, rotation);
            CreatedGameObjects.Add(created);
            return created;
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
            Object created = Object.Instantiate(original, position, rotation, parent);
            CreatedGameObjects.Add(created);
            return created;
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T created = Object.Instantiate(original, position, rotation, parent);
            CreatedGameObjects.Add(created);
            return created;
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
            Object created = Object.Instantiate(original);
            CreatedGameObjects.Add(created);
            return created;
        }

        public T Instantiate<T>(T original) where T : Object
        {
            T created = Object.Instantiate(original);
            CreatedGameObjects.Add(created);
            return created;
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
            Object created = Object.Instantiate(original, parent);
            CreatedGameObjects.Add(created);
            return created;
        }

        public T Instantiate<T>(T original, Transform parent) where T : Object
        {
            T created = Object.Instantiate(original, parent);
            CreatedGameObjects.Add(created);
            return created;
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
            Object created = Object.Instantiate(original, parent, instantiateInWorldSpace);
            CreatedGameObjects.Add(created);
            return created;
        }

        public T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
        {
            T created = Object.Instantiate(original, parent, worldPositionStays);
            CreatedGameObjects.Add(created);
            return created;
        }
    }
}