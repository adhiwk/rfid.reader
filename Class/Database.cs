using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Rfid.Reader.Class
{
    class Databases
    {
        public static MySqlConnection OpenMysqlDatabase()
        {

            string strMyServer = Properties.Settings.Default.DbHost.Trim();
            string strMyDb = Security.Decrypt(Properties.Settings.Default.DbName.Trim());
            string strMyUser = Security.Decrypt(Properties.Settings.Default.DbUser.Trim());
            string strMyPass = Security.Decrypt(Properties.Settings.Default.DbPassword.Trim());
            string strMyPort = Properties.Settings.Default.DbPort.Trim();


            string strKoneksi = "Data Source='" + strMyServer.Trim() +
                "';Initial Catalog='" + strMyDb.Trim() +
                "';User ID='" + strMyUser.Trim() +
                "'; Password='" + strMyPass.Trim() +
                "'; Port = '" + strMyPort.Trim() + "'";

            return new MySqlConnection(strKoneksi);
        }

    }
}
