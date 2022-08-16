using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dirty_PushCameraAsRendererMono : MonoBehaviour
{
    public Camera m_target;
    public int m_width = 512;
    public int m_height = 256;
    public uint m_contentId=5;
    public RenderTexture m_texture;

    public RenderTextureEvent m_renderTextureEvent;
    [System.Serializable]
    public class RenderTextureEvent : UnityEvent<RenderTexture> { };

    public TextureWithIdEvent m_textureEvent;
    [System.Serializable]
    public class TextureWithIdEvent : UnityEvent<Texture, uint> { };

    void Awake()
    {
        m_texture = new RenderTexture(m_width, m_height,0);
        if (m_target != null)
            m_target.targetTexture = m_texture;
       
            m_texture = new RenderTexture(m_width, m_height, 0);
            m_texture.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_texture);
        
    }

    [ContextMenu("Push")]
    public void Push()
    {
        if (m_texture == null) { 
            m_texture = new RenderTexture(m_width, m_height, 0);
            m_texture.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_texture);
        }

        if (m_target != null)
            m_target.targetTexture = m_texture;
        m_renderTextureEvent.Invoke(m_texture);
        m_textureEvent.Invoke(m_texture, m_contentId);

    }
}
