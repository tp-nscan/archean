namespace archean.wpf
{
    //public static class SimBuilder
    //{
    //public static void UseCnxn()
    //{
    //   // string connectionString = "server=localhost;port=3305;database=parking;uid=root";
    //    string connectionString = "server=localhost;port=3306;database=archean;uid=tom;password=barney";

    //    using (MySqlConnection connection = new MySqlConnection(connectionString))
    //    {
    //        // Create database if not exists
    //        using (ArcheanContext contextDB = new ArcheanContext(connection, false))
    //        {
    //            contextDB.Database.CreateIfNotExists();
    //        }

    //        connection.Open();
    //       // MySqlTransaction transaction = connection.BeginTransaction();

    //        try
    //        {
    //            // DbConnection that is already opened
    //            using (ArcheanContext context = new ArcheanContext(connection, false))
    //            {

    //                // Interception/SQL logging
    //                context.Database.Log = (string message) => { Console.WriteLine(message); };

    //                // Passing an existing transaction to the context
    //                //context.Database.UseTransaction(transaction);

    //                context.Sims.Add(new Sim { SimType = "Nissan", Description = "370Z", SimParams = "2012"});

    //                context.SaveChanges();
    //            }

    //            //transaction.Commit();
    //        }
    //        catch(Exception ex)
    //        {
    //           // transaction.Rollback();
    //            throw;
    //        }
    //    }
    //}



    //public static void NoCnxn()
    //{
    //    try
    //    {
    //        using (ArcheanContext context = new ArcheanContext())
    //        {
    //            // Interception/SQL logging
    //            context.Database.Log = (string message) => { Debug.WriteLine(message); };
    //            context.Sims.Add(new Sim { SimType = "Toyota", Description = "Yaris", SimParams = "2009" });

    //            context.SaveChanges();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}


    //}
}