// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Moonlay.Topics.MDM.DataSets
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class NewDataSetTopic : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"NewDataSetTopic\",\"namespace\":\"Moonlay.Topics.MDM.DataSet" +
				"s\",\"fields\":[{\"name\":\"Name\",\"type\":\"string\"},{\"name\":\"DomainName\",\"type\":\"string" +
				"\"},{\"name\":\"OrgName\",\"type\":\"string\"}]}");
		private string _Name;
		private string _DomainName;
		private string _OrgName;
		public virtual Schema Schema
		{
			get
			{
				return NewDataSetTopic._SCHEMA;
			}
		}
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}
		public string DomainName
		{
			get
			{
				return this._DomainName;
			}
			set
			{
				this._DomainName = value;
			}
		}
		public string OrgName
		{
			get
			{
				return this._OrgName;
			}
			set
			{
				this._OrgName = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.Name;
			case 1: return this.DomainName;
			case 2: return this.OrgName;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.Name = (System.String)fieldValue; break;
			case 1: this.DomainName = (System.String)fieldValue; break;
			case 2: this.OrgName = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
