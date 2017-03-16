using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SpriteRenderer : Renderer
	{
		public Sprite sprite
		{
			get
			{
				return this.GetSprite_INTERNAL();
			}
			set
			{
				this.SetSprite_INTERNAL(value);
			}
		}

		public Color color
		{
			get
			{
				Color result;
				this.INTERNAL_get_color(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_color(ref value);
			}
		}

		public extern bool flipX
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool flipY
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Sprite GetSprite_INTERNAL();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetSprite_INTERNAL(Sprite sprite);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_color(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_color(ref Color value);

		internal Bounds GetSpriteBounds()
		{
			Bounds result;
			SpriteRenderer.INTERNAL_CALL_GetSpriteBounds(this, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetSpriteBounds(SpriteRenderer self, out Bounds value);
	}
}
