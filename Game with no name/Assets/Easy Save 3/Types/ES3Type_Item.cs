using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("itemType", "amount")]
	public class ES3Type_Item : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_Item() : base(typeof(Item)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Item)obj;
			
			writer.WriteProperty("itemType", instance.itemType);
			writer.WriteProperty("amount", instance.amount, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Item)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "itemType":
						instance.itemType = reader.Read<Item.ItemType>();
						break;
					case "amount":
						instance.amount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Item();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_ItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_ItemArray() : base(typeof(Item[]), ES3Type_Item.Instance)
		{
			Instance = this;
		}
	}
}