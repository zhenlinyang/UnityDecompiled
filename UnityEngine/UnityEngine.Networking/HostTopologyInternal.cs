using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	internal sealed class HostTopologyInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public HostTopologyInternal(HostTopology topology)
		{
			ConnectionConfigInternal config = new ConnectionConfigInternal(topology.DefaultConfig);
			this.InitWrapper(config, topology.MaxDefaultConnections);
			for (int i = 1; i <= topology.SpecialConnectionConfigsCount; i++)
			{
				ConnectionConfig specialConnectionConfig = topology.GetSpecialConnectionConfig(i);
				ConnectionConfigInternal config2 = new ConnectionConfigInternal(specialConnectionConfig);
				this.AddSpecialConnectionConfig(config2);
			}
			this.InitOtherParameters(topology);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InitWrapper(ConnectionConfigInternal config, int maxDefaultConnections);

		private int AddSpecialConnectionConfig(ConnectionConfigInternal config)
		{
			return this.AddSpecialConnectionConfigWrapper(config);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int AddSpecialConnectionConfigWrapper(ConnectionConfigInternal config);

		private void InitOtherParameters(HostTopology topology)
		{
			this.InitReceivedPoolSize(topology.ReceivedMessagePoolSize);
			this.InitSentMessagePoolSize(topology.SentMessagePoolSize);
			this.InitMessagePoolSizeGrowthFactor(topology.MessagePoolSizeGrowthFactor);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InitReceivedPoolSize(ushort pool);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InitSentMessagePoolSize(ushort pool);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InitMessagePoolSizeGrowthFactor(float factor);

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();

		~HostTopologyInternal()
		{
			this.Dispose();
		}
	}
}
