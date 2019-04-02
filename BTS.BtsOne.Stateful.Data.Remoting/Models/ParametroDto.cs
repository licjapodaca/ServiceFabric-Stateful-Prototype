using Microsoft.ServiceFabric.Data;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace BTS.BtsOne.Stateful.Data.Remoting.Models
{
	public class ParametroDto //: IComparable<ParametroDto>, IEquatable<ParametroDto>
	{
		public string Parametro { get; set; }
		public string Usuario { get; set; }
		public string Email { get; set; }
		public string Departamento { get; set; }
		public string Puesto { get; set; }
		public string Certificacion { get; set; }

		//#region Object Overrides for GetHashCode, CompareTo and Equals
		//public int CompareTo(ParametroDto other)
		//{
		//	throw new NotImplementedException();
		//}

		//public bool Equals(ParametroDto other)
		//{
		//	throw new NotImplementedException();
		//}
		//#endregion
	}

	//public class ParametroDtoSerializer : IStateSerializer<ParametroDto>
	//{
	//	ParametroDto IStateSerializer<ParametroDto>.Read(BinaryReader reader)
	//	{
	//		var value = new ParametroDto();
	//		value.Parametro = reader.ReadString();
	//		value.Certificacion = reader.ReadString();
	//		value.Departamento = reader.ReadString();
	//		value.Email = reader.ReadString();
	//		value.Puesto = reader.ReadString();
	//		value.Usuario = reader.ReadString();

	//		return value;
	//	}

	//	void IStateSerializer<ParametroDto>.Write(ParametroDto value, BinaryWriter writer)
	//	{
	//		writer.Write(value.Parametro);
	//		writer.Write(value.Certificacion);
	//		writer.Write(value.Departamento);
	//		writer.Write(value.Email);
	//		writer.Write(value.Puesto);
	//		writer.Write(value.Usuario);
	//	}

	//	// Read overload for differential de-serialization
	//	ParametroDto IStateSerializer<ParametroDto>.Read(ParametroDto baseValue, BinaryReader reader)
	//	{
	//		return ((IStateSerializer<ParametroDto>)this).Read(reader);
	//	}

	//	// Write overload for differential serialization
	//	void IStateSerializer<ParametroDto>.Write(ParametroDto baseValue, ParametroDto newValue, BinaryWriter writer)
	//	{
	//		((IStateSerializer<ParametroDto>)this).Write(newValue, writer);
	//	}
	//}
}
