using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AndroidJNI
	{
		private AndroidJNI()
		{
		}

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int AttachCurrentThread();

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int DetachCurrentThread();

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetVersion();

		[ThreadAndSerializationSafe]
		public static IntPtr FindClass(string name)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_FindClass(name, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_FindClass(string name, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr FromReflectedMethod(IntPtr refMethod)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_FromReflectedMethod(refMethod, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_FromReflectedMethod(IntPtr refMethod, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr FromReflectedField(IntPtr refField)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_FromReflectedField(refField, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_FromReflectedField(IntPtr refField, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToReflectedMethod(IntPtr clazz, IntPtr methodID, bool isStatic)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToReflectedMethod(clazz, methodID, isStatic, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToReflectedMethod(IntPtr clazz, IntPtr methodID, bool isStatic, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToReflectedField(IntPtr clazz, IntPtr fieldID, bool isStatic)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToReflectedField(clazz, fieldID, isStatic, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToReflectedField(IntPtr clazz, IntPtr fieldID, bool isStatic, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetSuperclass(IntPtr clazz)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetSuperclass(clazz, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetSuperclass(IntPtr clazz, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsAssignableFrom(IntPtr clazz1, IntPtr clazz2);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Throw(IntPtr obj);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ThrowNew(IntPtr clazz, string message);

		[ThreadAndSerializationSafe]
		public static IntPtr ExceptionOccurred()
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ExceptionOccurred(out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ExceptionOccurred(out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExceptionDescribe();

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExceptionClear();

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FatalError(string message);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int PushLocalFrame(int capacity);

		[ThreadAndSerializationSafe]
		public static IntPtr PopLocalFrame(IntPtr ptr)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_PopLocalFrame(ptr, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_PopLocalFrame(IntPtr ptr, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewGlobalRef(IntPtr obj)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewGlobalRef(obj, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewGlobalRef(IntPtr obj, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteGlobalRef(IntPtr obj);

		[ThreadAndSerializationSafe]
		public static IntPtr NewLocalRef(IntPtr obj)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewLocalRef(obj, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewLocalRef(IntPtr obj, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteLocalRef(IntPtr obj);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsSameObject(IntPtr obj1, IntPtr obj2);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int EnsureLocalCapacity(int capacity);

		[ThreadAndSerializationSafe]
		public static IntPtr AllocObject(IntPtr clazz)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_AllocObject(clazz, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_AllocObject(IntPtr clazz, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewObject(IntPtr clazz, IntPtr methodID, jvalue[] args)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewObject(clazz, methodID, args, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewObject(IntPtr clazz, IntPtr methodID, jvalue[] args, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetObjectClass(IntPtr obj)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetObjectClass(obj, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetObjectClass(IntPtr obj, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsInstanceOf(IntPtr obj, IntPtr clazz);

		[ThreadAndSerializationSafe]
		public static IntPtr GetMethodID(IntPtr clazz, string name, string sig)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetMethodID(clazz, name, sig, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetMethodID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetFieldID(IntPtr clazz, string name, string sig)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetFieldID(clazz, name, sig, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetFieldID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetStaticMethodID(IntPtr clazz, string name, string sig)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetStaticMethodID(clazz, name, sig, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetStaticMethodID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetStaticFieldID(IntPtr clazz, string name, string sig)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetStaticFieldID(clazz, name, sig, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetStaticFieldID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewStringUTF(string bytes)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewStringUTF(bytes, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewStringUTF(string bytes, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetStringUTFLength(IntPtr str);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStringUTFChars(IntPtr str);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CallStringMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		public static IntPtr CallObjectMethod(IntPtr obj, IntPtr methodID, jvalue[] args)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_CallObjectMethod(obj, methodID, args, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_CallObjectMethod(IntPtr obj, IntPtr methodID, jvalue[] args, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CallIntMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CallBooleanMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short CallShortMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte CallByteMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char CallCharMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CallFloatMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double CallDoubleMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long CallLongMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CallVoidMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStringField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		public static IntPtr GetObjectField(IntPtr obj, IntPtr fieldID)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetObjectField(obj, fieldID, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetObjectField(IntPtr obj, IntPtr fieldID, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetBooleanField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetByteField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char GetCharField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short GetShortField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetIntField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetLongField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloatField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double GetDoubleField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStringField(IntPtr obj, IntPtr fieldID, string val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetObjectField(IntPtr obj, IntPtr fieldID, IntPtr val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBooleanField(IntPtr obj, IntPtr fieldID, bool val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetByteField(IntPtr obj, IntPtr fieldID, byte val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCharField(IntPtr obj, IntPtr fieldID, char val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetShortField(IntPtr obj, IntPtr fieldID, short val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetIntField(IntPtr obj, IntPtr fieldID, int val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLongField(IntPtr obj, IntPtr fieldID, long val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetFloatField(IntPtr obj, IntPtr fieldID, float val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDoubleField(IntPtr obj, IntPtr fieldID, double val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CallStaticStringMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		public static IntPtr CallStaticObjectMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_CallStaticObjectMethod(clazz, methodID, args, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_CallStaticObjectMethod(IntPtr clazz, IntPtr methodID, jvalue[] args, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CallStaticIntMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CallStaticBooleanMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short CallStaticShortMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte CallStaticByteMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char CallStaticCharMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CallStaticFloatMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double CallStaticDoubleMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long CallStaticLongMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CallStaticVoidMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStaticStringField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		public static IntPtr GetStaticObjectField(IntPtr clazz, IntPtr fieldID)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetStaticObjectField(clazz, fieldID, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetStaticObjectField(IntPtr clazz, IntPtr fieldID, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetStaticBooleanField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetStaticByteField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char GetStaticCharField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short GetStaticShortField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetStaticIntField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetStaticLongField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetStaticFloatField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double GetStaticDoubleField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticStringField(IntPtr clazz, IntPtr fieldID, string val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticObjectField(IntPtr clazz, IntPtr fieldID, IntPtr val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticBooleanField(IntPtr clazz, IntPtr fieldID, bool val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticByteField(IntPtr clazz, IntPtr fieldID, byte val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticCharField(IntPtr clazz, IntPtr fieldID, char val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticShortField(IntPtr clazz, IntPtr fieldID, short val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticIntField(IntPtr clazz, IntPtr fieldID, int val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticLongField(IntPtr clazz, IntPtr fieldID, long val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticFloatField(IntPtr clazz, IntPtr fieldID, float val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticDoubleField(IntPtr clazz, IntPtr fieldID, double val);

		[ThreadAndSerializationSafe]
		public static IntPtr ToBooleanArray(bool[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToBooleanArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToBooleanArray(bool[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToByteArray(byte[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToByteArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToByteArray(byte[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToCharArray(char[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToCharArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToCharArray(char[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToShortArray(short[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToShortArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToShortArray(short[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToIntArray(int[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToIntArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToIntArray(int[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToLongArray(long[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToLongArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToLongArray(long[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToFloatArray(float[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToFloatArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToFloatArray(float[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToDoubleArray(double[] array)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToDoubleArray(array, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToDoubleArray(double[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToObjectArray(IntPtr[] array, IntPtr arrayClass)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_ToObjectArray(array, arrayClass, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ToObjectArray(IntPtr[] array, IntPtr arrayClass, out IntPtr value);

		public static IntPtr ToObjectArray(IntPtr[] array)
		{
			return AndroidJNI.ToObjectArray(array, IntPtr.Zero);
		}

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool[] FromBooleanArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] FromByteArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char[] FromCharArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short[] FromShortArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int[] FromIntArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long[] FromLongArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float[] FromFloatArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double[] FromDoubleArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr[] FromObjectArray(IntPtr array);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetArrayLength(IntPtr array);

		[ThreadAndSerializationSafe]
		public static IntPtr NewBooleanArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewBooleanArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewBooleanArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewByteArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewByteArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewByteArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewCharArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewCharArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewCharArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewShortArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewShortArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewShortArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewIntArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewIntArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewIntArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewLongArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewLongArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewLongArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewFloatArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewFloatArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewFloatArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewDoubleArray(int size)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewDoubleArray(size, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewDoubleArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewObjectArray(int size, IntPtr clazz, IntPtr obj)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_NewObjectArray(size, clazz, obj, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_NewObjectArray(int size, IntPtr clazz, IntPtr obj, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetBooleanArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetByteArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char GetCharArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short GetShortArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetIntArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetLongArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloatArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double GetDoubleArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		public static IntPtr GetObjectArrayElement(IntPtr array, int index)
		{
			IntPtr result;
			AndroidJNI.INTERNAL_CALL_GetObjectArrayElement(array, index, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetObjectArrayElement(IntPtr array, int index, out IntPtr value);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBooleanArrayElement(IntPtr array, int index, byte val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetByteArrayElement(IntPtr array, int index, sbyte val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCharArrayElement(IntPtr array, int index, char val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetShortArrayElement(IntPtr array, int index, short val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetIntArrayElement(IntPtr array, int index, int val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLongArrayElement(IntPtr array, int index, long val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetFloatArrayElement(IntPtr array, int index, float val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDoubleArrayElement(IntPtr array, int index, double val);

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetObjectArrayElement(IntPtr array, int index, IntPtr obj);
	}
}
