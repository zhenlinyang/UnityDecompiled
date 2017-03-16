using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	[MovedFrom("UnityEngine")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class NavMeshPath
	{
		internal IntPtr m_Ptr;

		internal Vector3[] m_corners;

		public Vector3[] corners
		{
			get
			{
				this.CalculateCorners();
				return this.m_corners;
			}
		}

		public extern NavMeshPathStatus status
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NavMeshPath();

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DestroyNavMeshPath();

		~NavMeshPath()
		{
			this.DestroyNavMeshPath();
			this.m_Ptr = IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetCornersNonAlloc(Vector3[] results);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Vector3[] CalculateCornersInternal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearCornersInternal();

		public void ClearCorners()
		{
			this.ClearCornersInternal();
			this.m_corners = null;
		}

		private void CalculateCorners()
		{
			if (this.m_corners == null)
			{
				this.m_corners = this.CalculateCornersInternal();
			}
		}
	}
}
