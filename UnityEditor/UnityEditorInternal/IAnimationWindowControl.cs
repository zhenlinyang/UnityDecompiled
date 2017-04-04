using System;
using UnityEngine;

namespace UnityEditorInternal
{
	internal abstract class IAnimationWindowControl : ScriptableObject
	{
		public abstract AnimationKeyTime time
		{
			get;
		}

		public abstract bool canPlay
		{
			get;
		}

		public abstract bool playing
		{
			get;
		}

		public abstract bool canRecord
		{
			get;
		}

		public abstract bool recording
		{
			get;
		}

		public virtual void OnEnable()
		{
			base.hideFlags = HideFlags.HideAndDontSave;
		}

		public abstract void OnSelectionChanged();

		public abstract void GoToTime(float time);

		public abstract void GoToFrame(int frame);

		public abstract void StartScrubTime();

		public abstract void ScrubTime(float time);

		public abstract void EndScrubTime();

		public abstract void GoToPreviousFrame();

		public abstract void GoToNextFrame();

		public abstract void GoToPreviousKeyframe();

		public abstract void GoToNextKeyframe();

		public abstract void GoToFirstKeyframe();

		public abstract void GoToLastKeyframe();

		public abstract void StartPlayback();

		public abstract void StopPlayback();

		public abstract bool PlaybackUpdate();

		public abstract void StartRecording(UnityEngine.Object targetObject);

		public abstract void StopRecording();

		public abstract void ResampleAnimation();
	}
}
