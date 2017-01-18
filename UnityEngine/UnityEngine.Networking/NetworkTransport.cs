using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
	public sealed class NetworkTransport
	{
		public static extern bool IsStarted
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		private NetworkTransport()
		{
		}

		internal static bool DoesEndPointUsePlatformProtocols(EndPoint endPoint)
		{
			if (endPoint.GetType().FullName == "UnityEngine.PS4.SceEndPoint")
			{
				SocketAddress socketAddress = endPoint.Serialize();
				if (socketAddress[8] != 0 || socketAddress[9] != 0)
				{
					return true;
				}
			}
			return false;
		}

		public static int ConnectEndPoint(int hostId, EndPoint endPoint, int exceptionConnectionId, out byte error)
		{
			error = 0;
			byte[] array = new byte[]
			{
				95,
				36,
				19,
				246
			};
			if (endPoint == null)
			{
				throw new NullReferenceException("Null EndPoint provided");
			}
			if (endPoint.GetType().FullName != "UnityEngine.XboxOne.XboxOneEndPoint" && endPoint.GetType().FullName != "UnityEngine.PS4.SceEndPoint")
			{
				throw new ArgumentException("Endpoint of type XboxOneEndPoint or SceEndPoint  required");
			}
			if (endPoint.GetType().FullName == "UnityEngine.XboxOne.XboxOneEndPoint")
			{
				if (endPoint.AddressFamily != AddressFamily.InterNetworkV6)
				{
					throw new ArgumentException("XboxOneEndPoint has an invalid family");
				}
				SocketAddress socketAddress = endPoint.Serialize();
				if (socketAddress.Size != 14)
				{
					throw new ArgumentException("XboxOneEndPoint has an invalid size");
				}
				if (socketAddress[0] != 0 || socketAddress[1] != 0)
				{
					throw new ArgumentException("XboxOneEndPoint has an invalid family signature");
				}
				if (socketAddress[2] != array[0] || socketAddress[3] != array[1] || socketAddress[4] != array[2] || socketAddress[5] != array[3])
				{
					throw new ArgumentException("XboxOneEndPoint has an invalid signature");
				}
				byte[] array2 = new byte[8];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = socketAddress[6 + i];
				}
				IntPtr intPtr = new IntPtr(BitConverter.ToInt64(array2, 0));
				if (intPtr == IntPtr.Zero)
				{
					throw new ArgumentException("XboxOneEndPoint has an invalid SOCKET_STORAGE pointer");
				}
				byte[] array3 = new byte[2];
				Marshal.Copy(intPtr, array3, 0, array3.Length);
				AddressFamily addressFamily = (AddressFamily)(((int)array3[1] << 8) + (int)array3[0]);
				if (addressFamily != AddressFamily.InterNetworkV6)
				{
					throw new ArgumentException("XboxOneEndPoint has corrupt or invalid SOCKET_STORAGE pointer");
				}
				return NetworkTransport.Internal_ConnectEndPoint(hostId, intPtr, 128, exceptionConnectionId, out error);
			}
			else
			{
				SocketAddress socketAddress2 = endPoint.Serialize();
				if (socketAddress2.Size != 16)
				{
					throw new ArgumentException("EndPoint has an invalid size");
				}
				if ((int)socketAddress2[0] != socketAddress2.Size)
				{
					throw new ArgumentException("EndPoint has an invalid size value");
				}
				if (socketAddress2[1] != 2)
				{
					throw new ArgumentException("EndPoint has an invalid family value");
				}
				byte[] array4 = new byte[16];
				for (int j = 0; j < array4.Length; j++)
				{
					array4[j] = socketAddress2[j];
				}
				IntPtr intPtr2 = Marshal.AllocHGlobal(array4.Length);
				Marshal.Copy(array4, 0, intPtr2, array4.Length);
				int result = NetworkTransport.Internal_ConnectEndPoint(hostId, intPtr2, 16, exceptionConnectionId, out error);
				Marshal.FreeHGlobal(intPtr2);
				return result;
			}
		}

		public static void Init()
		{
			NetworkTransport.InitWithNoParameters();
		}

		public static void Init(GlobalConfig config)
		{
			NetworkTransport.InitWithParameters(new GlobalConfigInternal(config));
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitWithNoParameters();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitWithParameters(GlobalConfigInternal config);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Shutdown();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetAssetId(GameObject go);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void AddSceneId(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNextSceneId();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ConnectAsNetworkHost(int hostId, string address, int port, NetworkID network, SourceID source, NodeID node, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DisconnectNetworkHost(int hostId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NetworkEventType ReceiveRelayEventFromHost(int hostId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, int bytesPerSec, float bucketSizeFactor, out byte error);

		public static int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, out byte error)
		{
			return NetworkTransport.ConnectToNetworkPeer(hostId, address, port, exceptionConnectionId, relaySlotId, network, source, node, 0, 0f, out error);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCurrentIncomingMessageAmount();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCurrentOutgoingMessageAmount();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCurrentRtt(int hostId, int connectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNetworkLostPacketNum(int hostId, int connectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetPacketSentRate(int hostId, int connectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetPacketReceivedRate(int hostId, int connectionId, out byte error);

		[Obsolete("GetRemotePacketReceivedRate has been made obsolete. Please do not use this function."), WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetRemotePacketReceivedRate(int hostId, int connectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNetIOTimeuS();

		public static void GetConnectionInfo(int hostId, int connectionId, out string address, out int port, out NetworkID network, out NodeID dstNode, out byte error)
		{
			ulong num;
			ushort num2;
			address = NetworkTransport.GetConnectionInfo(hostId, connectionId, out port, out num, out num2, out error);
			network = (NetworkID)num;
			dstNode = (NodeID)num2;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetConnectionInfo(int hostId, int connectionId, out int port, out ulong network, out ushort dstNode, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNetworkTimestamp();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetRemoteDelayTimeMS(int hostId, int connectionId, int remoteTime, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool StartSendMulticast(int hostId, int channelId, byte[] buffer, int size, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SendMulticast(int hostId, int connectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool FinishSendMulticast(int hostId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxPacketSize();

		private static void CheckTopology(HostTopology topology)
		{
			int maxPacketSize = NetworkTransport.GetMaxPacketSize();
			if ((int)topology.DefaultConfig.PacketSize > maxPacketSize)
			{
				throw new ArgumentOutOfRangeException("Default config: packet size should be less than packet size defined in global config: " + maxPacketSize.ToString());
			}
			for (int i = 0; i < topology.SpecialConnectionConfigs.Count; i++)
			{
				if ((int)topology.SpecialConnectionConfigs[i].PacketSize > maxPacketSize)
				{
					throw new ArgumentOutOfRangeException("Special config " + i.ToString() + ": packet size should be less than packet size defined in global config: " + maxPacketSize.ToString());
				}
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddWsHostWrapper(HostTopologyInternal topologyInt, string ip, int port);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddWsHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port);

		[ExcludeFromDocs]
		public static int AddWebsocketHost(HostTopology topology, int port)
		{
			string ip = null;
			return NetworkTransport.AddWebsocketHost(topology, port, ip);
		}

		public static int AddWebsocketHost(HostTopology topology, int port, [DefaultValue("null")] string ip)
		{
			if (topology == null)
			{
				throw new NullReferenceException("topology is not defined");
			}
			if (ip == null)
			{
				return NetworkTransport.AddWsHostWrapperWithoutIp(new HostTopologyInternal(topology), port);
			}
			return NetworkTransport.AddWsHostWrapper(new HostTopologyInternal(topology), ip, port);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddHostWrapper(HostTopologyInternal topologyInt, string ip, int port, int minTimeout, int maxTimeout);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port, int minTimeout, int maxTimeout);

		[ExcludeFromDocs]
		public static int AddHost(HostTopology topology, int port)
		{
			string ip = null;
			return NetworkTransport.AddHost(topology, port, ip);
		}

		[ExcludeFromDocs]
		public static int AddHost(HostTopology topology)
		{
			string ip = null;
			int port = 0;
			return NetworkTransport.AddHost(topology, port, ip);
		}

		public static int AddHost(HostTopology topology, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
		{
			if (topology == null)
			{
				throw new NullReferenceException("topology is not defined");
			}
			if (ip == null)
			{
				return NetworkTransport.AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, 0, 0);
			}
			return NetworkTransport.AddHostWrapper(new HostTopologyInternal(topology), ip, port, 0, 0);
		}

		[ExcludeFromDocs]
		public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, int port)
		{
			string ip = null;
			return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
		}

		[ExcludeFromDocs]
		public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout)
		{
			string ip = null;
			int port = 0;
			return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
		}

		public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
		{
			if (topology == null)
			{
				throw new NullReferenceException("topology is not defined");
			}
			if (ip == null)
			{
				return NetworkTransport.AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, minTimeout, maxTimeout);
			}
			return NetworkTransport.AddHostWrapper(new HostTopologyInternal(topology), ip, port, minTimeout, maxTimeout);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool RemoveHost(int hostId);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Connect(int hostId, string address, int port, int exeptionConnectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_ConnectEndPoint(int hostId, IntPtr sockAddrStorage, int sockAddrStorageLen, int exceptionConnectionId, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ConnectWithSimulator(int hostId, string address, int port, int exeptionConnectionId, out byte error, ConnectionSimulatorConfig conf);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Disconnect(int hostId, int connectionId, out byte error);

		public static bool Send(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error)
		{
			if (buffer == null)
			{
				throw new NullReferenceException("send buffer is not initialized");
			}
			return NetworkTransport.SendWrapper(hostId, connectionId, channelId, buffer, size, out error);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SendWrapper(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NetworkEventType Receive(out int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NetworkEventType ReceiveFromHost(int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPacketStat(int direction, int packetStatId, int numMsgs, int numBytes);

		public static bool StartBroadcastDiscovery(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error)
		{
			if (buffer != null)
			{
				if (buffer.Length < size)
				{
					throw new ArgumentOutOfRangeException(string.Concat(new object[]
					{
						"Size: ",
						size,
						" > buffer.Length ",
						buffer.Length
					}));
				}
				if (size == 0)
				{
					throw new ArgumentOutOfRangeException("Size is zero while buffer exists, please pass null and 0 as buffer and size parameters");
				}
			}
			if (buffer == null)
			{
				return NetworkTransport.StartBroadcastDiscoveryWithoutData(hostId, broadcastPort, key, version, subversion, timeout, out error);
			}
			return NetworkTransport.StartBroadcastDiscoveryWithData(hostId, broadcastPort, key, version, subversion, buffer, size, timeout, out error);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StartBroadcastDiscoveryWithoutData(int hostId, int broadcastPort, int key, int version, int subversion, int timeout, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StartBroadcastDiscoveryWithData(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StopBroadcastDiscovery();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsBroadcastDiscoveryRunning();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBroadcastCredentials(int hostId, int key, int version, int subversion, out byte error);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetBroadcastConnectionInfo(int hostId, out int port, out byte error);

		public static void GetBroadcastConnectionInfo(int hostId, out string address, out int port, out byte error)
		{
			address = NetworkTransport.GetBroadcastConnectionInfo(hostId, out port, out error);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetBroadcastConnectionMessage(int hostId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);
	}
}
