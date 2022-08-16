using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchContextTextureWrapperMono : MonoBehaviour
{
    public List<DispathItem> m_dispatcher = new List<DispathItem>();
    [System.Serializable]
    public class DispathItem
    {

        public uint m_contextId;
        public EloiDependency.ClassicUnityEvent_Texture m_onChangedFound;
    }

    public void Push(Eloi.TextureSourceToInt32BitsArray2DWrapper textureWrapper)
    {
        for (int i = 0; i < m_dispatcher.Count; i++)
        {
            if (m_dispatcher[i].m_contextId == textureWrapper.m_data.m_contextId.m_contextId)
            {
                m_dispatcher[i].m_onChangedFound.Invoke(textureWrapper.m_data.m_textureReference);
            }
        }
    }
}
