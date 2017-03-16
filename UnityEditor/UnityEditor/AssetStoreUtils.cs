using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
	internal sealed class AssetStoreUtils
	{
		public delegate void DownloadDoneCallback(string package_id, string message, int bytes, int total);

		private const string kAssetStoreUrl = "https://shawarma.unity3d.com";

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK, [DefaultValue("null")] AssetStoreUtils.DownloadDoneCallback doneCallback);

		[ExcludeFromDocs]
		public static void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK)
		{
			AssetStoreUtils.DownloadDoneCallback doneCallback = null;
			AssetStoreUtils.Download(id, url, destination, key, jsonData, resumeOK, doneCallback);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CheckDownload(string id, string url, string[] destination, string key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RegisterDownloadDelegate(ScriptableObject d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnRegisterDownloadDelegate(ScriptableObject d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetLoaderPath();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UpdatePreloading();

		public static string GetOfflinePath()
		{
			return Uri.EscapeUriString(EditorApplication.applicationContentsPath + "/Resources/offline.html");
		}

		public static string GetAssetStoreUrl()
		{
			return "https://shawarma.unity3d.com";
		}

		public static string GetAssetStoreSearchUrl()
		{
			return AssetStoreUtils.GetAssetStoreUrl().Replace("https", "http");
		}
	}
}
