using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class UdpBytesPayloadListenerThread : MonoBehaviour , IUDPByteListener
{
    public int m_portToListen=11000;
    public System.Threading.ThreadPriority m_priority = System.Threading.ThreadPriority.Normal;

    public bool m_threadStayAlive = true;
    public int m_bytesCount;
    public IUDPByteListener.PayloadListener m_listener;
    public Queue<byte[]> m_receivedOfUnityEvent= new Queue<byte[]>();

    public PayloadEvent m_onPayloadReceived;
    public byte[] m_lastReceived;
    [System.Serializable]
    public class PayloadEvent : UnityEvent<byte[]> { } 

    void Awake()
    {
        Thread t = new Thread(ThreadMethode);
        t.Priority = m_priority;
        t.Start();
        InvokeRepeating("PushQueue", 0, 0.01f);
    }
    public void PushQueue() {
        while (m_receivedOfUnityEvent != null && m_receivedOfUnityEvent.Count>0) {
            byte[] p = m_receivedOfUnityEvent.Dequeue();
            Thread.Sleep(1);
            m_onPayloadReceived.Invoke(p);
        }
    }
    private void OnDestroy()
    {
        m_threadStayAlive = false;
    }

    void ThreadMethode()
    {
        UdpClient udpServer = new UdpClient(m_portToListen);
        var remoteEP = new IPEndPoint(IPAddress.Any, m_portToListen);
        while (m_threadStayAlive)
        {
            byte [] receivedBytes = udpServer.Receive(ref remoteEP);
            m_bytesCount = receivedBytes.Length;
            if (m_listener!=null)
                m_listener.Invoke(in receivedBytes);
            m_receivedOfUnityEvent.Enqueue(receivedBytes);
            m_lastReceived = receivedBytes;
            Thread.Sleep(1);
        }
    }
    

    public void AddPayloadListener(IUDPByteListener.PayloadListener listener)
    {
        if (listener != null)
            m_listener += listener;
    }

    public void RemovePayloadListener(IUDPByteListener.PayloadListener listener)
    {
        if(listener!=null)
            m_listener -= listener;
    }
}
