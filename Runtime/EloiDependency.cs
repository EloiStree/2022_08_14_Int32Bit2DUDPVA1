using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    public class EloiDependency 
    {
        [System.Serializable]
        public class ClassicUnityEvent_Texture2D : UnityEvent<Texture2D> { }

        [System.Serializable]
        public class ClassicUnityEvent_Texture : UnityEvent<Texture> { }

        [System.Serializable]
        public class ClassicUnityEvent_RenderTexture : UnityEvent<RenderTexture> { }
    }

