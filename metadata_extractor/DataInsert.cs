using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace metadata_extractor
{
	internal class DataInsert

	{
		private NpgsqlConnection conn;

		public DataInsert(string connString)
		{
			var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
			var dataSource = dataSourceBuilder.Build();

			conn = dataSource.OpenConnection();
		}
		public string[] noQuote(Dictionary<string,string> data, string[] insertList)
		{
			var insertString = $"";
			var valuesString = $"";
			foreach (var item in insertList)
			{
				try
				{
					valuesString += $"{data[item]},";
					insertString += $"{item},";
				}
				catch (Exception)
				{
					;
				}	
				
			}
			return new string[] { insertString, valuesString };
		}
		public string[] withQuote(Dictionary<string, string> data, string[] insertList)
		{
			var insertString = $"";
			var valuesString = $"";
			foreach (var item in insertList)
			{
				try{
					valuesString += $"'{data[item]}',";
					insertString += $"{item},";
				}	catch (Exception)
				{
					;
				}
			}
			valuesString = valuesString.Remove(valuesString.Length - 1);
			insertString = insertString.Remove(insertString.Length - 1);
			return new string[] { insertString, valuesString };
		}	

		public void insertData(Dictionary<string,string> data, string[] quoteList, string[] noQuoteList, string owner)
		{
			var arr1 = noQuote(data, noQuoteList);
			var arr2 = withQuote(data, quoteList);
			var command = $"INSERT INTO test3 (Owner, {arr1[0] + arr2[0]}) VALUES " +
						$"('@owner', {arr1[1] + arr2[1]});";
			Console.WriteLine(command);
			using (var cmd = new NpgsqlCommand(
						command, conn))
			{
				cmd.Parameters.AddWithValue("Owner", owner);
				cmd.ExecuteNonQuery();
			};
			conn.Close();
		}
	}
}

