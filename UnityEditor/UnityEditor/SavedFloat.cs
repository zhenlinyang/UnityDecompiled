using System;

namespace UnityEditor
{
	internal class SavedFloat
	{
		private float m_Value;

		private string m_Name;

		private bool m_Loaded;

		public float value
		{
			get
			{
				this.Load();
				return this.m_Value;
			}
			set
			{
				this.Load();
				if (this.m_Value != value)
				{
					this.m_Value = value;
					EditorPrefs.SetFloat(this.m_Name, value);
				}
			}
		}

		public SavedFloat(string name, float value)
		{
			this.m_Name = name;
			this.m_Loaded = false;
			this.m_Value = value;
		}

		private void Load()
		{
			if (!this.m_Loaded)
			{
				this.m_Loaded = true;
				this.m_Value = EditorPrefs.GetFloat(this.m_Name, this.m_Value);
			}
		}

		public static implicit operator float(SavedFloat s)
		{
			return s.value;
		}
	}
}
