using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.Common;

namespace SEV.Library
{
    public class RowMapper
    {
        private DataRow CurrentRow { get; set; }

        public RowMapper()
        {
            List<AttributeWithAlias> LocalAttributes = GetLocalAttributesWithAliases();
            foreach (AttributeWithAlias CurrentAttribute in LocalAttributes)
            {
                PropertyInfo AttributeToUpdate = GetType().GetProperty(CurrentAttribute.Name);
                //De acuerdo con el tipo de datos recuperado del atributo
                switch (CurrentAttribute.Type)
                {
                    //Si es Byte
                    case "Byte": AttributeToUpdate.SetValue(this, DefaulValues.setByte); break;
                    //Si es entero de 16 bits
                    case "Int16": AttributeToUpdate.SetValue(this, DefaulValues.setInt16); break;
                    //Si es entero de 32 bits
                    case "Int32": AttributeToUpdate.SetValue(this, DefaulValues.setInt32); break;
                    //Si es doble
                    case "Double": AttributeToUpdate.SetValue(this, DefaulValues.setDouble); break;
                    //Si es decimal
                    case "Decimal": AttributeToUpdate.SetValue(this, DefaulValues.setDecimal); break;
                    // Si es cadena
                    case "String": AttributeToUpdate.SetValue(this, DefaulValues.setString); break;
                    //Si es fecha
                    case "DateTime": AttributeToUpdate.SetValue(this, DefaulValues.setDateTime); break;
                    //Si es time
                    case "TimeSpan": AttributeToUpdate.SetValue(this, DefaulValues.setTime); break;
                    //Si es entero de 64 bits
                    case "Int64": AttributeToUpdate.SetValue(this, DefaulValues.setInt64); break;
                    //Si es Boolean
                    case "Boolean": AttributeToUpdate.SetValue(this, DefaulValues.setBoolean); break;
                    //Si es uniqueidentifier GUID
                    case "Guid": AttributeToUpdate.SetValue(this, DefaulValues.setGuid); break;
                    //En caso de no ser alguno de estos tipos se genera como nulo
                    //default: AttributeToUpdate.SetValue(this, null); break;
                    default: break;
                }
            }
        }
        // Metodo para cargar los datos de un reglon de la Tabla
        public void Populate(DataRow Data)
        {
            // Se almacenan los datos actuales de manera temporal
            CurrentRow = Data;
            // Se cargan los atributos actuales con su nombre tipo y alias
            List<AttributeWithAlias> LocalAttributes = GetLocalAttributesWithAliases();
            //Se recorre el listado de atributos
            foreach (AttributeWithAlias CurrentAttribute in LocalAttributes)
            {
                //Se comienza a revisar cada alias (El atributo como tal siempre es el primer alias)
                foreach (String CurrentAliasAttribute in CurrentAttribute.Aliases)
                {
                    //Se busca el atributo dentro del dataset
                    if (Data.Table.Columns.Contains(CurrentAliasAttribute))
                    {
                        //Si existe se obtiene el atributo del objeto a se actualizado desde el dataset
                        PropertyInfo AttributeToUpdate = GetType().GetProperty(CurrentAttribute.Name);
                        // Se establece contenedor para el resultado
                        var CurrentDBValue = Data[CurrentAliasAttribute];
                        Type CurrentDBValueType = CurrentDBValue.GetType();
                        //De acuerdo con el tipo de datos recuperado del atributo
                        switch (CurrentAttribute.Type)
                        {
                            //Si es Byte
                            case "Byte": 
                                        Byte ByteValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setByte : (Byte) CurrentDBValue;
                                        AttributeToUpdate.SetValue(this, ByteValue); 
                                    break;
                            //Si es entero de 32 bits
                            case "Int16":
                                        Int16 Int16Value = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setInt16 : (Int16)CurrentDBValue;
                                        AttributeToUpdate.SetValue(this, Int16Value);
                                    break;
                            //Si es entero de 32 bits
                            case "Int32": 
                                        Int32 Int32Value = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setInt32 : (Int32) CurrentDBValue;
                                        AttributeToUpdate.SetValue(this, Int32Value); 
                                    break;
                            //Si es doble
                            case "Double": 
                                        Double DoubleValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setDouble : (Double) CurrentDBValue;
                                        AttributeToUpdate.SetValue(this, DoubleValue); 
                                    break;
                            //Si es decimal
                            case "Decimal": 
                                        Decimal DecimalValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setDecimal : (Decimal) CurrentDBValue;
                                        AttributeToUpdate.SetValue(this, DecimalValue); 
                                    break;
                            // Si es cadena
                            case "String":
                                        String StringValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setString : (String) CurrentDBValue;
                                        String ValueToSet = CurrentAttribute.MustTrim ? StringValue.Trim() : StringValue;
                                        AttributeToUpdate.SetValue(this, ValueToSet); 
                                    break;
                            //Si es fecha
                            case "DateTime": 
                                        DateTime DateTimeValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setDateTime : (DateTime) CurrentDBValue;
                                        AttributeToUpdate.SetValue(this, DateTimeValue); 
                                    break;
                            //Si es tiempo
                            case "TimeSpan":
                                TimeSpan Hora = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setTime : (TimeSpan)CurrentDBValue;
                                AttributeToUpdate.SetValue(this, (TimeSpan)Data[CurrentAliasAttribute]);
                                break;
                            //Si es entero de 64 bits
                            case "Int64":
                                Int32 Int64Value = (int)(DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setInt64 : (Int64)CurrentDBValue);
                                AttributeToUpdate.SetValue(this, Int64Value);
                                break;
                            //Si es Boolean - BIT
                            case "Boolean":
                                Boolean BooleanValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setBoolean : (Boolean)CurrentDBValue;
                                AttributeToUpdate.SetValue(this, BooleanValue);
                                break;
                            //Si es uniqueidentifier GUID
                            case "Guid":
                                Guid GuidValue = DefaulValues.isNull.Equals(CurrentDBValueType) ? DefaulValues.setGuid : (Guid)CurrentDBValue;
                                AttributeToUpdate.SetValue(this, GuidValue);
                                break;
                            //En caso de no ser alguno de estos tipos se genera como nulo
                            //default: AttributeToUpdate.SetValue(this, null); break;
                            default: break;
                        }
                    }
                }
            }
            OnFinishPopulate();
            CurrentRow = null;
        }

        //Metodo para obtener una lista con los atributos locales, su tipo de dato y los posibles aliases que se asignaron
        //El nombre original del atributo y que será generado se incluye como primer Alias.
        public List<AttributeWithAlias> GetLocalAttributesWithAliases()
        {
            // Se prepara el contenedor de salida
            List<AttributeWithAlias> Result = new List<AttributeWithAlias>();
            // Se obtienen los elementos actuales en el objeto
            PropertyInfo[] LocalProperties = GetType().GetProperties();

            //Se recorre cada atributo
            foreach (PropertyInfo LocalProperty in LocalProperties)
            {
                // Se prepara la respuesta con el primer atributo
                AttributeWithAlias AttributeToSend = new AttributeWithAlias
                {
                    Name = LocalProperty.Name,
                    Type = LocalProperty.PropertyType.Name,
                    Aliases = new List<String>
                        {
                            LocalProperty.Name
                        },
                    MustTrim = false,
                    DataParamName = "@" + LocalProperty.Name
                };
                //Se obtienen las anotaciones del tributo
                foreach (Attribute LocalAttribute in LocalProperty.GetCustomAttributes(false))
                {
                    if (LocalAttribute.GetType().Name == "Alias")
                    {
                        Alias CurrentAlias = (Alias)LocalAttribute;
                        foreach (String Name in CurrentAlias.Names)
                        {
                            AttributeToSend.Aliases.Add(Name);
                        }
                    }
                    if (LocalAttribute.GetType().Name == "MustTrim")
                    {
                        AttributeToSend.MustTrim = true;
                    }
                    if (LocalAttribute.GetType().Name == "DataParamName")
                    {
                        DataParamName CurrentDataParamName = (DataParamName) LocalAttribute;
                        AttributeToSend.DataParamName = CurrentDataParamName.Name.Equals(null) ? CurrentDataParamName.Name : "@" + CurrentDataParamName.Name;
                    }
                }
                //Se agrega el atributo a la lista de salida
                Result.Add(AttributeToSend);
            }
            //Se envia el resultado
            return Result;
        }

        public Boolean IsNullable()
        {
            return true;
        }

        public string ConcatFromOrigin(string Fields, string Glue) {
            string Result = "";
            List<string> FoundElements = new List<string>();
            string[] DataFields = Fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Array.ForEach(
                DataFields,
                Field =>
                    {
                        if (CurrentRow.Table.Columns.Contains(Field))
                        {
                            FoundElements.Add(CurrentRow[Field].ToString());
                        }
                    }
            );
            Result = String.Join(Glue, FoundElements);
            return Result;
        }

        public T GetAttributeFromOrigin<T>(string AttributteName){
            T Result = default(T);
            try{
                Result = (T) CurrentRow[AttributteName];
            }catch{
                Result = default(T);
            }
                return Result;
        }

        public virtual void OnFinishPopulate() {

        }

        public List<DataParam> AsDataParamList(){
            List<DataParam> Params = new List<DataParam>();
            List<AttributeWithAlias> LocalAttributes = GetLocalAttributesWithAliases();
            foreach (AttributeWithAlias CurrentAttribute in LocalAttributes)
            {
                PropertyInfo AttributeToUpdate = GetType().GetProperty(CurrentAttribute.Name);
                if(!CurrentAttribute.DataParamName.Equals(DataParam.Ignore)){
                    DbType CurrentType = System.Data.DbType.String;
                    dynamic CurrentValue = GetType().GetProperty(CurrentAttribute.Name).GetValue(this);
                    switch (CurrentAttribute.Type)
                    {
                        //Si es Byte
                        case "Byte": 
                                    CurrentType = System.Data.DbType.Byte;
                                break;
                        //Si es entero de 16 bits
                        case "Int16":
                            CurrentType = System.Data.DbType.Int16;
                            break;
                        //Si es entero de 32 bits
                        case "Int32": 
                                    CurrentType = System.Data.DbType.Int32;
                                break;
                        //Si es doble
                        case "Double": 
                                    CurrentType = System.Data.DbType.Double;
                                break;
                        //Si es decimal
                        case "Decimal": 
                                    CurrentType = System.Data.DbType.Decimal;
                                break;
                        // Si es cadena
                        case "String":
                                    CurrentType = System.Data.DbType.String;
                                break;
                        //Si es fecha
                        case "DateTime": 
                                    CurrentType = System.Data.DbType.DateTime;
                                break;
                        //Si es tiempo
                        case "TimeSpan":
                            CurrentType = System.Data.DbType.Time;
                            break;
                        //Si es entero de 64 bits
                        case "Int64":
                            CurrentType = System.Data.DbType.Int64;
                            break;
                        //Si es Boolean
                        case "Boolean":
                            CurrentType = System.Data.DbType.Boolean;
                            break;
                        //Si es Guid
                        case "Guid":
                            CurrentType = System.Data.DbType.Guid;
                            break;
                        //En caso de no ser alguno de estos tipos se agrega como nulo
                        default: break;
                    }
                    Params.Add(
                        new DataParam(){
                            Id = CurrentAttribute.DataParamName,
                            Value = CurrentValue,
                            Type = CurrentType
                        }
                    );
                }
            }
            return Params;
        }
    }

    // Clase para resultados nulos
    class ResultLess : RowMapper {
        public const Boolean Activate = true;
    }

    //Clase apra administrar los atributos y sus alias
    public class AttributeWithAlias
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public List<String> Aliases { get; set; }
        public Boolean MustTrim { get; set; }
        public String DataParamName { get; set; }
    }

    public class DefaulValues
    {
        //Default Types
        public static Type isNull {get;} = typeof(DBNull);
        public static Type isString {get;} = typeof(String);
        public static Type isInt16 { get; } = typeof(Int16);
        public static Type isInt32 {get;} = typeof(Int32);
        public static Type isDouble {get;} = typeof(Double);
        public static Type isDecimal {get;} = typeof(Decimal);
        public static Type isDateTime {get;} = typeof(DateTime);
        public static Type isTime { get; } = typeof(TimeSpan);
        public static Type isByte {get;} = typeof(Byte);
        public static Type isInt64 { get; } = typeof(Int64);
        public static Type isBoolean { get; } = typeof(Boolean);
        public static Type isGuid { get; } = typeof(Guid);

        //Default values to set
        public static String setString {get;} = "";
        public static Int16 setInt16 {get;} = 0;
        public static Int32 setInt32 { get; } = 0;
        public static Double setDouble {get;} = 0.0;
        public static Decimal setDecimal {get;} = 0.0M;
        public static DateTime setDateTime {get;} = new DateTime(1970,1,1);
        public static TimeSpan setTime { get; } = new TimeSpan(0,0,0,0,0);
        public static Byte setByte {get;} = 0;
        public static Int64 setInt64 { get; } = 0;
        public static Boolean setBoolean { get; } = false;
        //public static Guid setGuid { get; } = new Guid("3C79EB26-2604-447F-8833-3669ADA39F8B");
        public static Guid setGuid { get; } = new Guid("00000000-0000-0000-0000-000000000000");
    }

}
