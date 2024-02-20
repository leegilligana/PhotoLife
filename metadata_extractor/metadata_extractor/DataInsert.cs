using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetadataExtractor;
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
        public string[] noQuote(Dictionary<string, string> data, string[] insertList)
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
                try
                {
                    valuesString += $"'{data[item]}',";
                    insertString += $"{item},";
                }
                catch (Exception)
                {
                    ;
                }
            }
            valuesString = valuesString.Remove(valuesString.Length - 1);
            insertString = insertString.Remove(insertString.Length - 1);
            return new string[] { insertString, valuesString };
        }

        public void insertDataHelper(Dictionary<string, string> data, string[] quoteList, string[] noQuoteList, string owner)
        {
            var arr1 = noQuote(data, noQuoteList);
            var arr2 = withQuote(data, quoteList);
            var command = $"INSERT INTO photolife (Owner, {arr1[0] + arr2[0]}) VALUES " +
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

        public void InsertData(string path, string user)
		        {
                    IEnumerable<MetadataExtractor.Directory> directories =
            ImageMetadataReader.ReadMetadata(path);
                    Dictionary<string, string> dataDict = new Dictionary<string, string>();
                    DataCleaner dataCleaner = new DataCleaner();
                    DataInsert dataInsert = new DataInsert(Constants.connString);
                    int count = 0;
                    string owner = user;

                    foreach (var directory in directories)
                    {
                        if (Constants.relevantDir.Contains(directory.Name))
                            foreach (var tag in directory.Tags)

                                if (Constants.relevantFields.Contains(tag.Name))
                                {
                                    var columnName = tag.Name.Replace(" ", "_");
                                    columnName = columnName.Replace("/", "_");
                                    try
                                    {
                                        dataDict.Add(columnName, tag.Description);
                                        count++;
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                }
                    }

                    dataCleaner.formatData(dataDict, Constants.removeUnits, Constants.pointToArrayFields, Constants.doubles);
                    foreach (KeyValuePair<string, string> author in dataDict)
                    {
                        Console.WriteLine("Key: {0}, Value: {1}",
                            author.Key, author.Value);
                    }
                    dataInsert.insertDataHelper(dataDict, Constants.quoteType, Constants.noQuoteType, owner);
                }
	        }
        }

