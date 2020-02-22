using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParseApp
{
    public class MangoDBConnect
    {
        public void Connect()
        {
            using (var connection = new MySqlConnection("Server=127.0.0.1;User ID=root;Password=123456;Database=cigars"))
            {
                connection.Open();

                using (var command = new MySqlCommand("SELECT * FROM brands;", connection))
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                        Console.WriteLine(reader.GetString(1));
                    }
                        

            }
        }

        public void Insert(List<Parameters> _item)
        {
            List<Parameters> _dbIns = _item;
            
            string connString = "Server=127.0.0.1;User ID=root;Password=123456;Database=cigars";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();

            foreach(var x in _dbIns)
            {
                comm.CommandText = "INSERT INTO items(ID,CIGAR,LENGTH,RING,COUNTRY,FILLER,WRAPPER,COLOR,STRENGTH) VALUES(@id,@cigar,@length,@ring,@country,@filler,@wrapper,@color,@strength)";

                for(int i = 0; i < x.Cigar.Length; i++)
                {
                    comm.Parameters.AddWithValue("@id", Guid.NewGuid());
                    comm.Parameters.AddWithValue("@cigar", x.Cigar[i].ToString());
                    comm.Parameters.AddWithValue("@length", x.Length[i].ToString());
                    comm.Parameters.AddWithValue("@ring", x.Ring[i].ToString());
                    comm.Parameters.AddWithValue("@country", x.Country[i].ToString());
                    comm.Parameters.AddWithValue("@filler", x.Filler[i].ToString());
                    comm.Parameters.AddWithValue("@wrapper", x.Wrapper[i].ToString());
                    comm.Parameters.AddWithValue("@color", x.Color[i].ToString());
                    comm.Parameters.AddWithValue("@strength", x.Strength[i].ToString());

                    comm.ExecuteNonQuery();
                    comm.Parameters.Clear();
                }
                
            }
            
            conn.Close();

            _dbIns.Clear();
        }
    }
}
