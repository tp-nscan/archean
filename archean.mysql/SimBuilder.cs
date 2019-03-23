using System;
using System.Diagnostics;
using archean.mysql.Entities;

namespace archean.mysql
{
    public static class SimBuilder
    {
        public static void NoCnxn()
        {
            try
            {
                using (ArcheanContext context = new ArcheanContext())
                {
                    // Interception/SQL logging
                    context.Database.Log = (string message) => { Debug.WriteLine(message); };
                    context.Sims.Add(new Sim { SimType = "Honda", Description = "Fit", SimParams = "2009" });

                    context.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}