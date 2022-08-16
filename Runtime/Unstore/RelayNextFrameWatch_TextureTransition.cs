using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class RelayNextFrameWatch_TextureTransition : MonoBehaviour
    {
        public List<Texture> m_textureQueue = new List<Texture>();
        public Eloi.ClassicUnityEvent_Texture m_onTexturePush;
        public void PushNext(RenderTexture texture)
        {
            m_textureQueue.Add(texture);
        }
        public void PushNext(Texture2D texture)
        {
            m_textureQueue.Add(texture);
        }
        public void PushNext(Texture texture)
        {
            m_textureQueue.Add(texture);
        }

        private void Update()
        {
            while (m_textureQueue.Count > 0) {
                Texture t = m_textureQueue[0];
                m_textureQueue.RemoveAt(0);
                m_onTexturePush.Invoke(t);
            }
        }
    }
}
