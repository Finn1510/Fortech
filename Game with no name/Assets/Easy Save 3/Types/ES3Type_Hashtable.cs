using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("keys", "values")]
	public class ES3Type_Hashtable : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_Hashtable() : base(typeof(System.Collections.Hashtable)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (System.Collections.Hashtable)obj;
			
			writer.WritePrivateField("keys", instance);
			writer.WritePrivateField("values", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (System.Collections.Hashtable)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "keys":
					reader.SetPrivateField("keys", reader.Read<System.Collections.ICollection>(), instance);
					break;
					case "values":
					reader.SetPrivateField("values", reader.Read<System.Collections.ICollection>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new System.Collections.Hashtable();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_HashtableArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_HashtableArray() : base(typeof(System.Collections.Hashtable[]), ES3Type_Hashtable.Instance)
		{
			Instance = this;
		}
	}
}