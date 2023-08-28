using System;
using UnityEngine;

[HelpURL("https://github.com/ScottGarryFoster/" +
         "PROJECT-ArcadeClassics#readme")]
[RequireComponent(typeof(Rigidbody2D))]
public class Person : MonoBehaviour
{
   [Header("Head")]

   [SerializeField]
   [Tooltip("Empty")]
   private Brain brain;

   [Space]
   [Range(2,10)]
   public int SpaceBetweenEyes;

   [TextArea]
   [Delayed]
   public string Speech;
}

[Serializable]
public struct Brain
{
   public string h;
}