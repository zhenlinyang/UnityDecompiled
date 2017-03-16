using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.WSA
{
	public sealed class Launcher
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LaunchFile(Folder folder, string relativeFilePath, bool showWarning);

		public static void LaunchFileWithPicker(string fileExtension)
		{
			Process.Start("explorer.exe");
		}

		public static void LaunchUri(string uri, bool showWarning)
		{
			Process.Start(uri);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalLaunchFileWithPicker(string fileExtension);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalLaunchUri(string uri, bool showWarning);
	}
}
