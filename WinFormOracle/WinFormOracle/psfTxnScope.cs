using System;
using Oracle.DataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Oracle.DataAccess;

public class psfTxnScope
{
   public static void MainOracle()
    {
        int retVal = 0;
        string providerName = "Oracle.DataAccess.Client";
        string constr =
               @"User Id=icropuser;Password=icropuser;Data Source=icrop4db;enlist=true";
       //
      


        // Get the provider factory.
        DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

        try
        {
            // Create a TransactionScope object, (It will start an ambient
            // transaction automatically).
            using (TransactionScope scope = new TransactionScope())
            {
                // Create first connection object.
                using (DbConnection conn1 = factory.CreateConnection())
                {
                    // Set connection string and open the connection. this connection 
                    // will be automatically enlisted in a promotable local transaction.
                    conn1.ConnectionString = constr;
                    conn1.Open();

                    // Create a command to execute the sql statement.
                    DbCommand cmd1 = factory.CreateCommand();
                    cmd1.Connection = conn1;
                    cmd1.CommandText = @"select * from tb_m_word where word_val like '%55%'";

                    // Execute the SQL statement to insert one row in DB.
                    retVal = cmd1.ExecuteNonQuery();
                    Console.WriteLine("Rows to be affected by cmd1: {0}", retVal);

                    // Close the connection and dispose the command object.
                    conn1.Close();
                    conn1.Dispose();
                    cmd1.Dispose();
                }

                // The Complete method commits the transaction. If an exception has
                // been thrown or Complete is not called then the transaction is 
                // rolled back.
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
}