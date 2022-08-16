using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

public interface IUDPBytePusherSender {

    public void SetDestinationIP(in string ip);
    public void SetDestinationPort(in int port);
    public void PushPaylaod( byte[] payloadMax65527);
    public void PushPaylaodIn(in byte[] payloadMax65527);
}
public interface IUDPByteListener
{
    public delegate void PayloadListener(in byte[] paylaod);
    public void AddPayloadListener(PayloadListener listener);
    public void RemovePayloadListener(PayloadListener listener);
}

public class UDPBytePusherSender : MonoBehaviour , IUDPBytePusherSender
{
    public string m_destinationIP = "127.0.0.1";
    public int m_destinationPort = 11000;
    public UdpClient m_client = new UdpClient();
    public IPEndPoint m_endPoint;

    private void Awake()
    {
            Init();
    }

    private void Init()
    {
        m_client = new UdpClient();
        m_endPoint = new IPEndPoint(IPAddress.Parse(m_destinationIP), m_destinationPort);
        SetDestinationPort(m_destinationPort);
        SetDestinationIP(m_destinationIP);
    }

    public void SetDestinationPort(in int port) {
        m_destinationPort = port;
        m_endPoint.Port = m_destinationPort;
        m_client.Connect(m_endPoint);
    }
    public void SetDestinationIP(in string destinationIP) { 
        m_destinationIP = destinationIP;
        m_endPoint.Address = IPAddress.Parse(m_destinationIP);
        m_client.Connect(m_endPoint);
    } 
    public void PushPaylaod( byte[] packagePayloadToPush)
    {
        PushPaylaodIn(in packagePayloadToPush);
    }

    public void PushPaylaodIn(in byte[] packagePayloadToPush)
    {
        if (m_client == null)
            Init();
        m_client.Send(packagePayloadToPush, packagePayloadToPush.Length);
    }
}
