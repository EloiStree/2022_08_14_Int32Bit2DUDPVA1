using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugBytesPushMono : MonoBehaviour
{
    public byte[] m_bytesToPush = new byte[10];

    public ByteToPushEvent m_onPayloadToPush;
    [System.Serializable]
    public class ByteToPushEvent : UnityEvent<byte[]> { }

    [ContextMenu("Push")]
    public void Push() {
        m_onPayloadToPush.Invoke(m_bytesToPush);
    }
}
