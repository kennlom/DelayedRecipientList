using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedRecipientList
{
    public static class DBUtils
    {
        /// <summary>
        /// Returns list of zip codes from sample table - [SampleRecipientListPluginData]
        /// </summary>
        public static IEnumerable<string> GetZipCodes()
        {
            const string Sql =
                @"SELECT DISTINCT ZipCode 
                  FROM [RecipientAddress]
                  ORDER BY ZipCode ASC";

            List<string> zipCodes = new List<string>();

            Database database = DatabaseFactory.CreateDatabase();

            using (IDataReader reader = database.ExecuteReader(CommandType.Text, Sql))
            {
                while (reader.Read())
                {
                    zipCodes.Add((string)reader[0]);
                }
            }

            return zipCodes;
        }

        /// <summary>
        /// Returns zip codes length from sample table - [SampleRecipientListPluginData]
        /// </summary>
        public static int GetRecipientListSize(string zipCode)
        {
            Database database = DatabaseFactory.CreateDatabase();

            DbCommand command = database.GetSqlStringCommand(
                @"SELECT COUNT(*) 
                  FROM [RecipientAddress]
                  WHERE @ZipCode IS NULL OR ZipCode = @ZipCode");

            database.AddInParameter(command, "@ZipCode", DbType.String, zipCode);

            return (int)database.ExecuteScalar(command);
        }

        /// <summary>
        /// Returns recipient list from sample table - [SampleRecipientListPluginData]
        /// </summary>
        public static DataSet GetRecipientList(string zipCode, int maxNumber)
        {
            Database database = DatabaseFactory.CreateDatabase();

            DbCommand command = database.GetSqlStringCommand(
                @"SELECT TOP " + maxNumber +
                @" *
                  FROM [RecipientAddress]
                  WHERE @ZipCode IS NULL OR ZipCode = @ZipCode");

            database.AddInParameter(command, "@ZipCode", DbType.String, zipCode);

            DataSet ds = database.ExecuteDataSet(command);
            ds.Tables[0].TableName = "RecipitentList"; //backward compatibility formatting
            return ds;
        }
    }
}

