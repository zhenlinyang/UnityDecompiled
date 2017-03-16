using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CapsuleCollider2D : Collider2D
	{
		public Vector2 size
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_size(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_size(ref value);
			}
		}

		public extern CapsuleDirection2D direction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_size(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_size(ref Vector2 value);
	}
}
