using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Input
	{
		private static Gyroscope m_MainGyro = null;

		private static LocationService locationServiceInstance;

		private static Compass compassInstance;

		public static extern bool compensateSensors
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
		public static extern bool isGyroAvailable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static Gyroscope gyro
		{
			get
			{
				if (Input.m_MainGyro == null)
				{
					Input.m_MainGyro = new Gyroscope(Input.mainGyroIndex_Internal());
				}
				return Input.m_MainGyro;
			}
		}

		public static Vector3 mousePosition
		{
			get
			{
				Vector3 result;
				Input.INTERNAL_get_mousePosition(out result);
				return result;
			}
		}

		public static Vector2 mouseScrollDelta
		{
			get
			{
				Vector2 result;
				Input.INTERNAL_get_mouseScrollDelta(out result);
				return result;
			}
		}

		public static extern bool mousePresent
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool simulateMouseWithTouches
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool anyKey
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool anyKeyDown
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string inputString
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static Vector3 acceleration
		{
			get
			{
				Vector3 result;
				Input.INTERNAL_get_acceleration(out result);
				return result;
			}
		}

		public static AccelerationEvent[] accelerationEvents
		{
			get
			{
				int accelerationEventCount = Input.accelerationEventCount;
				AccelerationEvent[] array = new AccelerationEvent[accelerationEventCount];
				for (int i = 0; i < accelerationEventCount; i++)
				{
					array[i] = Input.GetAccelerationEvent(i);
				}
				return array;
			}
		}

		public static extern int accelerationEventCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static Touch[] touches
		{
			get
			{
				int touchCount = Input.touchCount;
				Touch[] array = new Touch[touchCount];
				for (int i = 0; i < touchCount; i++)
				{
					array[i] = Input.GetTouch(i);
				}
				return array;
			}
		}

		public static extern int touchCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
		public static extern bool eatKeyPressOnTextFieldFocus
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool touchPressureSupported
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool stylusTouchSupported
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool touchSupported
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool multiTouchEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static LocationService location
		{
			get
			{
				if (Input.locationServiceInstance == null)
				{
					Input.locationServiceInstance = new LocationService();
				}
				return Input.locationServiceInstance;
			}
		}

		public static Compass compass
		{
			get
			{
				if (Input.compassInstance == null)
				{
					Input.compassInstance = new Compass();
				}
				return Input.compassInstance;
			}
		}

		public static extern DeviceOrientation deviceOrientation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern IMECompositionMode imeCompositionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string compositionString
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool imeIsSelected
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static Vector2 compositionCursorPos
		{
			get
			{
				Vector2 result;
				Input.INTERNAL_get_compositionCursorPos(out result);
				return result;
			}
			set
			{
				Input.INTERNAL_set_compositionCursorPos(ref value);
			}
		}

		public static extern bool backButtonLeavesApp
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int mainGyroIndex_Internal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyInt(int key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyUpInt(int key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyUpString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyDownInt(int key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyDownString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetAxis(string axisName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetAxisRaw(string axisName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetButton(string buttonName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetButtonDown(string buttonName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetButtonUp(string buttonName);

		public static bool GetKey(string name)
		{
			return Input.GetKeyString(name);
		}

		public static bool GetKey(KeyCode key)
		{
			return Input.GetKeyInt((int)key);
		}

		public static bool GetKeyDown(string name)
		{
			return Input.GetKeyDownString(name);
		}

		public static bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDownInt((int)key);
		}

		public static bool GetKeyUp(string name)
		{
			return Input.GetKeyUpString(name);
		}

		public static bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUpInt((int)key);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetJoystickNames();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsJoystickPreconfigured(string joystickName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButton(int button);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButtonDown(int button);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButtonUp(int button);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetInputAxes();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_mousePosition(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_mouseScrollDelta(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_acceleration(out Vector3 value);

		public static AccelerationEvent GetAccelerationEvent(int index)
		{
			AccelerationEvent result;
			Input.INTERNAL_CALL_GetAccelerationEvent(index, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetAccelerationEvent(int index, out AccelerationEvent value);

		public static Touch GetTouch(int index)
		{
			Touch result;
			Input.INTERNAL_CALL_GetTouch(index, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetTouch(int index, out Touch value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_compositionCursorPos(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_compositionCursorPos(ref Vector2 value);
	}
}
