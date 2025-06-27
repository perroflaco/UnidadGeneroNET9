using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SEV.Library
{
    public class DataMapper<T> where T : RowMapper, new()
    {
        private string ConnectionString { get; set; }

        public DataMapper(String Profile)
        {
            String ProfilePath = String.Format("ConnectionStrings:{0}", Profile);
            ConnectionString = SEVConfigAssistant.Configuration[ProfilePath];
        }

        public List<T> FromStoredProcedure(String SPName, List<DataParam> SPParams = null, Boolean NoResult= false)
        {
            List<T> Result = new List<T>();
            Database db = new SqlDatabase(ConnectionString);
            using (DbCommand SPCommand = db.GetStoredProcCommand(SPName))
            {
                if(SPParams != null)
                {
                    SPParams.ForEach(
                        CurrentParam => {
                            db.AddInParameter(SPCommand, CurrentParam.Id, CurrentParam.Type, CurrentParam.Value);
                        }
                    );
                }
                try
                {
                    DataSet DBData = db.ExecuteDataSet(SPCommand);
                    if(!NoResult){
                        DataTable DBTabla = DBData.Tables[0];
                        foreach (DataRow DBRow in DBTabla.Rows)
                        {
                            T Item = new T();
                            Item.Populate(DBRow);
                            Result.Add(Item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {ex.ToString()}");
                    Console.WriteLine(SPName + " " + ex.Message.ToString());
                }
            }
            return Result;
        }

        public List<T> FromStoredProcedureData(String SPName, DataTable Items, String ParameterNames, Boolean NoResult = false)
        {
            List<T> Result = new List<T>();
            Database db = new SqlDatabase(ConnectionString);
            using (DbCommand SPCommand = db.GetStoredProcCommand(SPName))
            {
                if (Items != null)
                {
                    var parameter = new SqlParameter //Esta declaracion es Clave en este Metodo
                    {
                        SqlDbType = SqlDbType.Structured, //El tipo Structured es el que se tomara la estructura del Data Table para
                        ParameterName = ParameterNames, //Hacer match con el Tipo Table en SQL, respetando el nombre declarado en el StoreProcedure
                        Value = Items
                    };
                    SPCommand.Parameters.Add(parameter);
                }
                try
                {
                    DataSet DBData = db.ExecuteDataSet(SPCommand);
                    if (!NoResult)
                    {
                        DataTable DBTabla = DBData.Tables[0];
                        foreach (DataRow DBRow in DBTabla.Rows)
                        {
                            T Item = new T();
                            Item.Populate(DBRow);
                            Result.Add(Item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {ex.ToString()}");
                    Console.WriteLine(ex.Message.ToString()); 
                }
            }
            return Result;
        }
    }

    public class DataParam
    {
        public static String Ignore { get;} = null;
        public string Id { get; set; }
        public DbType Type { get; set; }
        public dynamic Value { get; set; }

        public static List<DataParam> Requerired(List<DataParam> Origin,String required){
            String[] ParamsToApply = required.Split(",",StringSplitOptions.None);
            List<DataParam> Result = new List<DataParam>();
            foreach(String ParamName in ParamsToApply){
                DataParam FoundItem = Origin.Find(
                    it => it.Id == "@"+ParamName
                );
                if(!FoundItem.Equals(null)){
                    Result.Add(FoundItem);
                }
            }
            return Result;
        }
    }
}
