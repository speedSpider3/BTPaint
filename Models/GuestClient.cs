﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Models
{
    public class GuestClient : Client
    {
        private Socket clientSocket;

        public void BeginConnect(IPEndPoint targetAddress)
        {
            StateObject state = new StateObject();
            state.workSocket = connectionSocket;

            connectionSocket.BeginConnect(targetAddress, DefaultConnectCallback, state);
        }

        public override void Send(byte[] buffer, SocketFlags flags = SocketFlags.None)
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                return;
            }

            StateObject state = new StateObject();
            state.workSocket = clientSocket;
            state.buffer = buffer;

            // Sends a packet to the remote end point, in this case the server
            clientSocket.BeginSend(state.buffer, 0, state.buffer.Length, flags, DefaultSendCallback, state);
        }

        public override void Close()
        {
            base.Close();

            if (clientSocket != null)
                clientSocket.Close();
        }

        protected void DefaultConnectCallback(IAsyncResult result)
        {
            StateObject state = (StateObject)result.AsyncState;
            state.workSocket.EndConnect(result);
            clientSocket = state.workSocket;

            StateObject state2 = new StateObject();
            state.workSocket = clientSocket;
            state.buffer = new byte[StateObject.BufferSize];

            // gets us ready to receive the first packet from the new client
            clientSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, OnPacketReceivedFromServer, state);
        }

        private void OnPacketReceivedFromServer(IAsyncResult result)
        {
            base.OnPacketReceived(result);

            StateObject state = (StateObject)result.AsyncState;

            int packetSize = state.workSocket.EndReceive(result);
            state.buffer = new byte[packetSize];

            // this "workSocket" is essentially the connection to the client
            // gets us ready to receive something from the client once again
            state.workSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, OnPacketReceivedFromServer, state);
        }
    }
}