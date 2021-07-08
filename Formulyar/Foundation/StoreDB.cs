using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulyar.ViewModel;
using System.Data;
using System.Data.SqlClient;
using Formulyar.Model;
using System.Collections.ObjectModel;

namespace Formulyar.Foundation
{
    class StoreDB
    {
        /// <summary>
        /// получение ОТИ из БД
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<OperTechInform> GetInfo()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.OtiCodesQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<OperTechInform> otiCollect = new MyObservableCollection<OperTechInform>();
            using (var connection = new SqlConnection(connString))
            {
               
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        var dcsour = "";
                        if (reader[7] != System.DBNull.Value)
                            dcsour =(string)reader[7];
                        var txt = "";
                        if (reader[8] != System.DBNull.Value)
                            txt = (string)reader[8];
                        var frm = "";
                        if (reader[9] != System.DBNull.Value)
                            frm = (string)reader[9];
                        var tt = "";
                        if (reader[10] != System.DBNull.Value)
                            tt = (string)reader[10];
                        var tn = "";
                        if (reader[11] != System.DBNull.Value)
                            tn = (string)reader[11];
                        var mt = "";
                        if (reader[12] != System.DBNull.Value)
                            mt = (string)reader[12];
                        var triggerOIK = false;
                        if (reader[13] != System.DBNull.Value)                        
                                triggerOIK = (bool)reader[13];                                                   
                        var triggerAIP = false;
                        if (reader[14] != System.DBNull.Value)
                            triggerAIP = (bool)reader[14];
                        var triggerCSPA = false;
                        if (reader[15] != System.DBNull.Value )
                            triggerCSPA = (bool)reader[15];
                        var triggerKOSMOS= false;
                        if (reader[16] != System.DBNull.Value )
                            triggerKOSMOS = (bool)reader[16];

                        otiCollect.Add(new OperTechInform() { NumberOI = (int)reader[0], NameOI = (string)reader[1],
                            TypeOI = (string)reader[2], TypeTI = (string)reader[3], Category = (string)reader[4],
                            EnergyObject = (string)reader[5], DispatchCenter = (string)reader[6], DispatchCenterSour=dcsour,
                            Formula =txt, FormulaCode=frm, CurrentTransform=tt, VoltagetTransform=tn, MeasureTransduse=mt,
                            TriggerOIK=triggerOIK, TriggerAIP=triggerAIP, TriggerCSPA=triggerCSPA, TriggerKOSMOS=triggerKOSMOS
                        });
                    }
                }
                return otiCollect;
            }
        }

        /// <summary>
        /// Запрос данных в аип
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<OperTechInform> GetOtiAIP()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.OtiAIPQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<OperTechInform> otiCollect = new MyObservableCollection<OperTechInform>();
            using (var connection = new SqlConnection(connString))
            {

                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {                        
                        otiCollect.Add(new OperTechInform()
                        {
                            NumberOI = (int)reader[0],
                            NameOI = (string)reader[1],
                            TypeOI = (string)reader[2],
                            TypeTI = (string)reader[3],
                            Category = (string)reader[4],
                            EnergyObject = (string)reader[5],
                            DispatchCenter = (string)reader[6]                            
                        });
                    }
                }
                return otiCollect;
            }
        }
        /// <summary>
        /// ОИ в ИМ
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<CIMObject> GetOtiCIM()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = @"select * from LSA_test..OIK_NP";
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CIMObject> CIMobjCollect = new MyObservableCollection<CIMObject>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        string str = (string)reader[2];
                        string type = "";
                        int id = 0;
                        if (str == "")
                        {
                            str = (string)reader[1];
                        }
                        str = str.Replace("Calc", string.Empty).Replace("Agr", string.Empty).Replace("aggr", string.Empty);
                        switch (str.Substring(0, 1))
                            {
                                case "I":
                                    type = str.Replace("I", "ТИ").Substring(0, 2);
                                    break;
                                case "S":
                                    type = str.Replace("S", "ТС").Substring(0, 2);
                                    break;
                                case "J":
                                    type = str.Replace("J", "ИС").Substring(0, 2);
                                    break;
                                case "C":
                                    type = str.Replace("C", "СП").Substring(0, 2);
                                    break;
                                case "H":
                                    type = str.Replace("H", "ПВ").Substring(0, 2);
                                    break;
                                case "M":
                                    type = str.Replace("M", "МСК").Substring(0, 2);
                                    break;
                                case "P":
                                    type = str.Replace("P", "ПЛ").Substring(0, 2);
                                    break;
                                case "U":
                                    type = str.Replace("U", "ЕИ").Substring(0, 2);
                                    break;
                                case "W":
                                    type = str.Replace("W", "СВ").Substring(0, 2);
                                    break;
                                case "Л":
                                    type = str.Replace("Л", "ЧАС").Substring(0, 2);
                                    break;
                                case "Б":
                                    type = str.Replace("Б", "МИН").Substring(0, 2);
                                    break;
                                case "Г":
                                    type = str.Replace("Г", "ПМИН").Substring(0, 2);
                                    break;
                                case "З":
                                    type = str.Replace("З", "ДМИН").Substring(0, 2);
                                    break;
                                case "И":
                                    type = str.Replace("И", "ЧЧАС").Substring(0, 2);
                                    break;
                                case "К":
                                    type = str.Replace("К", "ПЧАС").Substring(0, 2);
                                    break;
                                case "П":
                                    type = str.Replace("П", "СУТ").Substring(0, 2);
                                    break;
                                case "У":
                                    type = str.Replace("У", "МЕС").Substring(0, 2);
                                    break;
                                case "Ф":
                                    type = str.Replace("Ф", "ТМИН").Substring(0, 2);
                                    break;
                                default:
                                    type = str.Substring(0, 1);
                                    break;
                            }
                        try
                        {
                            id = Convert.ToInt32(str.Remove(0, 1));
                        }
                        catch
                        {
                            id = 0;
                        }     
                        CIMobjCollect.Add(new CIMObject()
                        {
                            UIDvalue = (string)reader[0],
                            Namevalue = (string)reader[1],
                            SourceID = id,
                            Type= type,
                            HISvalue = (string)reader[3],
                            UIDparentObj = (string)reader[4],
                            NameparentObj = (string)reader[5],
                            MeasValueType=(string)reader[6],
                            MeasValueSource=(string)reader[7],
                            DC = (string)reader[8]
                        });
                    }
                }
                return CIMobjCollect;
            }
        }
        /// <summary>
        /// получение КС из БД
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<Secheniya> GetSech()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.SechQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<Secheniya> sechCollect = new MyObservableCollection<Secheniya>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        var form = "";
                        if (reader[7] != System.DBNull.Value)
                        {                        
                            form = (string)reader[7];
                        }
                        if ((string)reader[6] == "ТИ")
                        sechCollect.Add(new Secheniya()
                        {
                            DispatchCenter = (string)reader[0],
                            NameSech = (string)reader[1],
                            IdSech=(int)reader[2],
                            EnergyObject=(string)reader[3],
                            NameOI = (string)reader[4],
                            NumberOI = (int)reader[5],
                            TypeOI =(string)reader[6],
                            Formula=form,
                            
                        });
                    }
                }
                return sechCollect;
            }
        }
        /// <summary>
        /// получение ДЦ-получателей
        /// </summary>
        /// <param name="oti"></param>
        /// <returns></returns>
        public MyObservableCollection<OperTechInform> GetReception(OperTechInform oti)
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query =  SQLquery.OtiReception(oti);
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<OperTechInform> otiCollect = new MyObservableCollection<OperTechInform>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        otiCollect.Add(new OperTechInform() { NumberOI = (int)reader[0], NameOI = (string)reader[1], TypeOI = (string)reader[2], TypeTI = (string)reader[3], EnergyObject = (string)reader[4], DispatchCenter = (string)reader[5] });
                    }
                }
                return otiCollect;
            }
        }
        /// <summary>
        /// получение МУН
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<Voltage> GetMUN()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.MUNQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<Voltage> voltageCollect = new MyObservableCollection<Voltage>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Voltage newVoltage = new Voltage();
                        newVoltage.IdVoltage = (int)reader[0];
                        if (reader[1] != System.DBNull.Value)
                            newVoltage.DispatchCenter = (string)reader[1];
                        if (reader[2] != System.DBNull.Value)
                            newVoltage.EnergyObject = (string)reader[2];
                        if (reader[3] != System.DBNull.Value)
                            newVoltage.VoltageLevel = (int)reader[3];
                        if (reader[4] != System.DBNull.Value)
                            newVoltage.NumberOIitog = (int)reader[4];
                        if (reader[5] != System.DBNull.Value)
                            newVoltage.NameOIitog = (string)reader[5];
                        //if (reader[6] != System.DBNull.Value)
                        //    newVoltage.TypeControl = (string)reader[6];
                        if (reader[6] != System.DBNull.Value)
                            newVoltage.Formula = (string)reader[6];
                        if (reader[7] != System.DBNull.Value)
                            newVoltage.FormulaCode = (string)reader[7];
                        //if (reader[9] != System.DBNull.Value)
                        //    newVoltage.TypeOI = (string)reader[9];
                        newVoltage.TypeOIitog = "ТИ";
                        //if (reader[10] != System.DBNull.Value)
                        //    newVoltage.NumberOI = (int)reader[10];
                        //if (reader[11] != System.DBNull.Value)
                        //    newVoltage.NameOI = (string)reader[11];
                        //if (reader[12] != System.DBNull.Value)
                        //    newVoltage.FormulaOI = (string)reader[12];
                        //if (newVoltage.TypeOI=="ТИ")
                        voltageCollect.Add(newVoltage);
                    }
                    
                }

                return voltageCollect;
            }
        }
        /// <summary>
        /// Получение списка СМТН ВЛ
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<CurrentLine> GetSmtnLine()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.SmtnLineQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CurrentLine> currentLineCollect = new MyObservableCollection<CurrentLine>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CurrentLine currentLine = new CurrentLine();
                        if (reader[0] != System.DBNull.Value)
                            currentLine.DispatchCenter = (string)reader[0];
                        if (reader[1] != System.DBNull.Value)
                            currentLine.NameLine = (string)reader[1];
                        if (reader[2] != System.DBNull.Value)
                            currentLine.IdLine = (int)reader[2];
                        if (reader[3] != System.DBNull.Value)
                            currentLine.EnergyObject = (string)reader[3];
                        if (reader[4] != System.DBNull.Value)
                            currentLine.IdEnergyObject = (int)reader[4];                        
                        if (reader[5] != System.DBNull.Value)
                            currentLine.NameOIfact = (string)reader[5];
                        if (reader[6] != System.DBNull.Value)
                            currentLine.NumberIfact = (int)reader[6];
                        if (reader[7] != System.DBNull.Value)
                            currentLine.Formula = (string)reader[7];
                        if (reader[8] != System.DBNull.Value)
                            currentLine.FormulaCode = (string)reader[8];
                        if (reader[9] != System.DBNull.Value)
                            currentLine.NameIa = (string)reader[9];
                        if (reader[10] != System.DBNull.Value)
                            currentLine.NumberIa = (int)reader[10];
                        if (reader[11] != System.DBNull.Value)
                            currentLine.NameIb = (string)reader[11];
                        if (reader[12] != System.DBNull.Value)
                            currentLine.NumberIb = (int)reader[12];
                        if (reader[13] != System.DBNull.Value)
                            currentLine.NameIc = (string)reader[13];
                        if (reader[14] != System.DBNull.Value)
                            currentLine.NumberIc = (int)reader[14];
                        if (reader[15] != System.DBNull.Value)
                            currentLine.NameItnv = (string)reader[15];
                        if (reader[16] != System.DBNull.Value)
                            currentLine.NumberItnv = (int)reader[16];
                        if (reader[17] != System.DBNull.Value)
                            currentLine.NameS = (string)reader[17];
                        if (reader[18] != System.DBNull.Value)
                            currentLine.NumberS = (int)reader[18];                     

                            currentLineCollect.Add(currentLine);
                    }
                }
                return currentLineCollect;
            }
        }

        /// <summary>
        /// Получение списка СМТН АТ(Т)
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<CurrentTransform> GetSmtnTransform()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.SmtnTransformQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CurrentTransform> currentTransformCollect = new MyObservableCollection<CurrentTransform>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CurrentTransform currentTransform = new CurrentTransform();
                        if (reader[0] != System.DBNull.Value)
                            currentTransform.DispatchCenter = (string)reader[0];
                        if (reader[1] != System.DBNull.Value)
                            currentTransform.EnergyObject = (string)reader[1];
                        if (reader[2] != System.DBNull.Value)
                            currentTransform.NameTransform = (string)reader[2];
                        if (reader[3] != System.DBNull.Value)
                            currentTransform.idTransform = (int)reader[3];
                        if (reader[4] != System.DBNull.Value)
                            currentTransform.TransformWinding = (string)reader[4];
                        if (reader[5] != System.DBNull.Value)
                            currentTransform.idWinding = (int)reader[5];
                        if (reader[6] != System.DBNull.Value)
                            currentTransform.NameIfact = (string)reader[6];
                        if (reader[7] != System.DBNull.Value)
                            currentTransform.NumberIfact = (int)reader[7];
                        if (reader[8] != System.DBNull.Value)
                            currentTransform.Formula = (string)reader[8];
                        if (reader[9] != System.DBNull.Value)
                            currentTransform.FormulaCode = (string)reader[9];                       
                        if (reader[10] != System.DBNull.Value)
                            currentTransform.NameRpn = (string)reader[10];
                        if (reader[11] != System.DBNull.Value)
                            currentTransform.NumberRpn = (int)reader[11];
                        if (reader[12] != System.DBNull.Value)
                            currentTransform.NameItnv = (string)reader[12];
                        if (reader[13] != System.DBNull.Value)
                            currentTransform.NumberItnv = (int)reader[13];
                        if (reader[14] != System.DBNull.Value)
                            currentTransform.NameS = (string)reader[14];
                        if (reader[15] != System.DBNull.Value)
                            currentTransform.NumberS = (int)reader[15];

                        currentTransformCollect.Add(currentTransform);
                    }
                }
                return currentTransformCollect;
            }
        }
        /// <summary>
        /// Получение списка СМТН Выкл
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<CurrentBreaker> GetSmtnBreaker()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.SmtnBreakerQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CurrentBreaker> currentBreakerCollect = new MyObservableCollection<CurrentBreaker>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CurrentBreaker currentBreaker = new CurrentBreaker();
                        if (reader[0] != System.DBNull.Value)
                            currentBreaker.DispatchCenter = (string)reader[0];
                        if (reader[1] != System.DBNull.Value)
                            currentBreaker.EnergyObject = (string)reader[1];
                        if (reader[2] != System.DBNull.Value)
                            currentBreaker.NameBreaker = (string)reader[2];
                        if (reader[3] != System.DBNull.Value)
                            currentBreaker.IdBreaker = (int)reader[3];
                        if (reader[4] != System.DBNull.Value)
                            currentBreaker.NameOIfact = (string)reader[4];
                        if (reader[5] != System.DBNull.Value)
                            currentBreaker.NumberIfact = (int)reader[5];
                        if (reader[6] != System.DBNull.Value)
                            currentBreaker.Formula = (string)reader[6];
                        if (reader[7] != System.DBNull.Value)
                            currentBreaker.FormulaCode = (string)reader[7];
                        if (reader[8] != System.DBNull.Value)
                            currentBreaker.NameIa = (string)reader[8];
                        if (reader[9] != System.DBNull.Value)
                            currentBreaker.NumberIa = (int)reader[9];
                        if (reader[10] != System.DBNull.Value)
                            currentBreaker.NameIb = (string)reader[10];
                        if (reader[11] != System.DBNull.Value)
                            currentBreaker.NumberIb = (int)reader[11];
                        if (reader[12] != System.DBNull.Value)
                            currentBreaker.NameIc = (string)reader[12];
                        if (reader[13] != System.DBNull.Value)
                            currentBreaker.NumberIc = (int)reader[13];
                        if (reader[14] != System.DBNull.Value)
                            currentBreaker.NameItnv = (string)reader[14];
                        if (reader[15] != System.DBNull.Value)
                            currentBreaker.NumberItnv = (int)reader[15];
                        if (reader[16] != System.DBNull.Value)
                            currentBreaker.NameS = (string)reader[16];
                        if (reader[17] != System.DBNull.Value)
                            currentBreaker.NumberS = (int)reader[17];

                        currentBreakerCollect.Add(currentBreaker);
                    }
                }
                return currentBreakerCollect;
            }
        }
        /// <summary>
        /// Получение списка СМТН Дополнительно
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<CurrentEquipment> GetSmtnEquipment()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.SmtnExtraQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CurrentEquipment> currentEquipmentCollect = new MyObservableCollection<CurrentEquipment>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        currentEquipmentCollect.Add(new CurrentEquipment() { DispatchCenter = (string)reader[0], EnergyObject = (string)reader[1], NameEquipment=(string)reader[2], TypeOTI=(string)reader[3], NumberOTI=(int)reader[4],
                        Formula=(string)reader[5], FormulaCode=(string)reader[6],NameOTI=(string)reader[7]});

                    }
                }
                return currentEquipmentCollect;
            }
        }
        /// <summary>
        /// Источники ТИ
        /// </summary>
        /// <param name="oti"></param>
        /// <returns></returns>
        public MyObservableCollection<OperTechInform> GetSource(OperTechInform oti)
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.OtiSource(oti);
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<OperTechInform> otiCollect = new MyObservableCollection<OperTechInform>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        otiCollect.Add(new OperTechInform() { NumberOI = (int)reader[0], NameOI = (string)reader[1], TypeOI = (string)reader[2], TypeTI = (string)reader[3], EnergyObject = (string)reader[4], DispatchCenter = (string)reader[5] });
                    }
                }
                return otiCollect;
            }
        }
        public MyObservableCollection<Protocol> GetChekInfo()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = @"LSA_test";
            var queryKPOS = SQLquery.CheckInfoKPOS;
            var queryMUN= SQLquery.CheckInfoMUN;
            var querySMTN_Line = SQLquery.CheckInfoSMTN_Line;
            var querySMTN_Transform = SQLquery.CheckInfoSMTN_Transform;

            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<Protocol> checkInfo = new MyObservableCollection<Protocol>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(queryKPOS, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Protocol check = new Protocol();
                        check.DispatchCenter = (string)reader[0];
                        check.KontrolObjectName = (string)reader[1];
                        check.NumberOI = (int)reader[2];
                        check.TypeOI = (string)reader[3];
                        if (reader[4] != System.DBNull.Value)
                            check.DcSource = (string)reader[4];
                        if (reader[5] != System.DBNull.Value)
                            check.DCReception = (string)reader[5];
                        if (reader[6] != System.DBNull.Value)
                            check.NumberOIsource = (int)reader[6];
                        if (reader[7] != System.DBNull.Value)
                            check.NumberOIreception = (int)reader[7];
                        if ((check.DcSource == null) || (check.DcSource == "-"))
                        {
                            check.Info = "КПОС: ОТИ контролируется только в одном ДЦ";
                        }
                        else
                        {
                            check.Info = "КПОС: Несоответсвие применения ОТИ";
                        }
                        checkInfo.Add(check);
                    }
                }


            }
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com1 = new SqlCommand(queryMUN, connection))
                {
                    com1.CommandTimeout = 150;
                    var reader1 = com1.ExecuteReader();
                    while (reader1.Read())
                    {
                        Protocol check = new Protocol();
                        check.DispatchCenter = (string)reader1[0];
                        check.KontrolObjectName = (string)reader1[1];
                        check.NumberOI = (int)reader1[5];
                        check.TypeOI = (string)reader1[6];
                        if (reader1[7] != System.DBNull.Value)
                            check.DcSource = (string)reader1[7];
                        if (reader1[8] != System.DBNull.Value)
                            check.DCReception = (string)reader1[8];
                        if (reader1[9] != System.DBNull.Value)
                            check.NumberOIsource = (int)reader1[9];
                        if (reader1[10] != System.DBNull.Value)
                            check.NumberOIreception = (int)reader1[10];
                        if ((check.DcSource == null) || (check.DcSource == "-"))
                        {
                            check.Info = "МУН: ОТИ контролируется только в одном ДЦ РУ: "+ (int)reader1[2] +
                                "кВ; контроль: " + (string)reader1[3];
                        }
                        else
                        {
                            check.Info = "МУН: Несоответсвие применения ОТИ в ДЦ РУ: " + (int)reader1[2]
                                + "кВ; контроль: "+(string)reader1[3];
                        }
                        checkInfo.Add(check);
                    }
                }

            }
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com2 = new SqlCommand(querySMTN_Line, connection))
                {
                    com2.CommandTimeout = 150;
                    var reader3 = com2.ExecuteReader();
                    while (reader3.Read())
                    {
                        Protocol check = new Protocol();
                        check.DispatchCenter = (string)reader3[1];
                        check.KontrolObjectName ="Объект: "+(string)reader3[2]+" Присоединение: " + (string)reader3[3];
                        check.NumberOI = (int)reader3[4];
                        check.TypeOI ="ТИ";
                        if (reader3[9] != System.DBNull.Value)
                            check.DcSource = (string)reader3[9];
                        if (reader3[10] != System.DBNull.Value)
                            check.DCReception = (string)reader3[10];
                        if (reader3[11] != System.DBNull.Value)
                            check.NumberOIsource = (int)reader3[11];
                        if (reader3[12] != System.DBNull.Value)
                            check.NumberOIreception = (int)reader3[12];
                        if ((check.DcSource == null)|| (check.DcSource == "-"))
                        {
                            check.Info = "СМТН_ЛЭП: ОТИ контролируется только в одном ДЦ! Присоединение : " + (string)reader3[3];
                        }
                        else
                        {
                            check.Info = "СМТН_ЛЭП: Несоответсвие применения ОТИ в ДЦ! Присоединение : " + (string)reader3[3];
                        }
                        checkInfo.Add(check);
                    }
                }

            }
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com3 = new SqlCommand(querySMTN_Transform, connection))
                {
                    com3.CommandTimeout = 150;
                    var reader4 = com3.ExecuteReader();
                    while (reader4.Read())
                    {
                        Protocol check = new Protocol();
                        check.DispatchCenter = (string)reader4[1];
                        check.KontrolObjectName = "Объект: " + (string)reader4[2] + " АТ(Т): " + (string)reader4[3] + " " + (string)reader4[4];
                        check.NumberOI = (int)reader4[5];
                        check.TypeOI = "ТИ";
                        if (reader4[6] != System.DBNull.Value)
                            check.DcSource = (string)reader4[6];
                        if (reader4[7] != System.DBNull.Value)
                            check.DCReception = (string)reader4[7];
                        if (reader4[8] != System.DBNull.Value)
                            check.NumberOIsource = (int)reader4[8];
                        if (reader4[9] != System.DBNull.Value)
                            check.NumberOIreception = (int)reader4[9];
                        if ((check.DcSource == null) || (check.DcSource == "-"))
                        {
                            check.Info = "СМТН_АТ(Т): ОТИ контролируется только в одном ДЦ! АТ(Т) : " + (string)reader4[3] +" "+ (string)reader4[4];
                        }
                        else
                        {
                            check.Info = "СМТН_АТ(Т): Несоответсвие применения ОТИ в ДЦ! АТ(Т) : " + (string)reader4[3] + " " + (string)reader4[4];
                        }
                        checkInfo.Add(check);
                    }
                }

            }
            return checkInfo;
        }
        /// <summary>
        /// получение МУН
        /// </summary>
        /// <returns></returns>
        public MyObservableCollection<Aopo> GetAOPO()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.AOPOQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString; 
            MyObservableCollection<Aopo> aopoCollect = new MyObservableCollection<Aopo>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Aopo newAopo = new Aopo();
                        if (reader[0] != System.DBNull.Value)
                            newAopo.DispatchCenter = (string)reader[0];
                        if (reader[1] != System.DBNull.Value)
                            newAopo.Equipment = (string)reader[1];
                        if (reader[2] != System.DBNull.Value)
                            newAopo.EquipmentID = (int)reader[2];
                        if (reader[3] != System.DBNull.Value)
                            newAopo.EnergyObject = (string)reader[3];
                        if (reader[4] != System.DBNull.Value)
                            newAopo.Step = (int)reader[4];                        
                        if (reader[5] != System.DBNull.Value)
                            newAopo.ElementName = (string)reader[5];

                        if (reader[6] != System.DBNull.Value)
                            newAopo.NumberTIobject = (int)reader[6];
                        if (reader[7] != System.DBNull.Value)
                            newAopo.NameTIobject = (string)reader[7];
                        if (reader[8] != System.DBNull.Value)
                            newAopo.NumberTIstandart = (int)reader[8];
                        if (reader[9] != System.DBNull.Value)
                            newAopo.NameTIstandart = (string)reader[9];
                        if (reader[10] != System.DBNull.Value)
                            newAopo.NumberTI = (int)reader[10];
                        if (reader[11] != System.DBNull.Value)
                            newAopo.NameTI = (string)reader[11];
                        if (reader[12] != System.DBNull.Value)
                            newAopo.NumberSitog = (int)reader[12];
                        if (reader[13] != System.DBNull.Value)
                            newAopo.NameSitog = (string)reader[13];
                        if (reader[14] != System.DBNull.Value)
                            newAopo.NumberS = (int)reader[14];
                        if (reader[15] != System.DBNull.Value)
                            newAopo.NameS = (string)reader[15];
                        aopoCollect.Add(newAopo);
                    }
                }
                return aopoCollect;
            }
        }

        public bool SaveDevice(ObservableCollection<OperTechInform> otiCollection)
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = @"LSA_test";
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                var query = SQLquery.savePasport();
                SqlCommand sqlcom = new SqlCommand("TRUNCATE TABLE LSa_test..PTIpasport", connection);
                sqlcom.ExecuteNonQuery();               
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];               
                for (int i = 0; i < otiCollection.Count; i++)
                {                    
                    OperTechInform oti = otiCollection[i];
                    DataRow newRow = dt.NewRow();
                    if (oti.CurrentTransform!="" || oti.VoltagetTransform!="" || oti.MeasureTransduse!="")
                    {
                        newRow["DC"] = oti.DispatchCenter;
                        newRow["NumbOti"] = oti.NumberOI;
                        newRow["TypeOI"] = oti.TypeOI;
                        newRow["TT"] = oti.CurrentTransform;
                        newRow["TN"] = oti.VoltagetTransform;
                        newRow["MeasTrans"] = oti.MeasureTransduse;
                        dt.Rows.Add(newRow);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                        adapter.Update(dt);
                        ds.Clear();
                    }
                    
                }           
                return true;
            }
        }
        public bool SaveApplication(ObservableCollection<OperTechInform> otiCollection)
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = @"LSA_test";
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                var query = SQLquery.saveApp();
                SqlCommand sqlcom = new SqlCommand("TRUNCATE TABLE LSa_test..ApplicationOTI", connection);
                sqlcom.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < otiCollection.Count; i++)
                {
                    OperTechInform oti = otiCollection[i];
                    DataRow newRow = dt.NewRow();
                    if (oti.TriggerAIP ==true || oti.TriggerOIK == true || oti.TriggerCSPA == true || oti.TriggerKOSMOS == true)
                    {
                        newRow["DC"] = oti.DispatchCenter;
                        newRow["NumbOti"] = oti.NumberOI;
                        newRow["TypeOI"] = oti.TypeOI;
                        newRow["OIK"] = oti.TriggerOIK;
                        newRow["AIP"] = oti.TriggerAIP;
                        newRow["CSPA"] = oti.TriggerCSPA;
                        newRow["KOSMOS"] = oti.TriggerKOSMOS;
                        dt.Rows.Add(newRow);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                        adapter.Update(dt);
                        ds.Clear();
                    }

                }
                return true;
            }
        }
        public bool SaveAopo(ObservableCollection<Aopo> aopoCollection)
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = @"LSA_test";
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                var query = SQLquery.saveAopo();
                SqlCommand sqlcom = new SqlCommand("TRUNCATE TABLE LSa_test..SMTN_AOPO", connection);
                sqlcom.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < aopoCollection.Count; i++)
                {
                    Aopo aopo = aopoCollection[i];
                    DataRow newRow = dt.NewRow();
                    newRow["DC"] = aopo.DispatchCenter;
                    newRow["EquipmentName"] = aopo.Equipment;
                    newRow["EquipmentID"] = aopo.EquipmentID;
                    newRow["EnergyObject"] = aopo.EnergyObject;
                    newRow["Step"] = aopo.Step;
                    newRow["ElementName"] = aopo.ElementName;
                    newRow["NumbTIobject"] = aopo.NumberTIobject;
                    newRow["NameTIobject"] = aopo.NameTIobject;
                    newRow["NumbTIstandart"] = aopo.NumberTIstandart;
                    newRow["NameTIstandart"] = aopo.NameTIstandart;
                    newRow["NumbTIsetting"] = aopo.NumberTI;
                    newRow["NameTIsetting"] = aopo.NameTI;
                    newRow["NumbTSitog"] = aopo.NumberSitog;
                    newRow["NameTSitog"] = aopo.NameSitog;
                    newRow["NumbTS"] = aopo.NumberS;
                    newRow["NameTS"] = aopo.NameS;
                    dt.Rows.Add(newRow);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(dt);
                    ds.Clear();
                }
                return true;
            }
        }
        public MyObservableCollection<ExchangeOTI> GetSource()
        {
            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.DcTransmitQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<ExchangeOTI> sourceCollect = new MyObservableCollection<ExchangeOTI>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        //var t = (string)reader[0];
                        //var t1 = (string)reader[1];
                        //var t2 = (string)reader[3];
                        //var t3 = (int)reader[6];
                        //var t4= (string)reader[7];
                        sourceCollect.Add(new ExchangeOTI()
                        {
                            DCsource = (string)reader[0],
                            Address = (string)reader[1].ToString(),
                            TypeOIsour = (string)reader[3],
                            NumberOIsour = (int)reader[6],
                            DCreceiver = (string)reader[7]
                        });
                    }
                }
                return sourceCollect;
            }
        }
        public MyObservableCollection<ExchangeOTI> GetReception()
        {
            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.DcReceiverQuery;
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<ExchangeOTI> receiverCollect = new MyObservableCollection<ExchangeOTI>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        receiverCollect.Add(new ExchangeOTI()
                        {
                            DCreceiver = (string)reader[0],
                            Address = (string)reader[1],
                            TypeOIrec = (string)reader[3],
                            NumberOIrec = (int)reader[6],
                            DCsource = (string)reader[7]
                        });
                    }
                }
                return receiverCollect;
            }
        }

        public MyObservableCollection<CommonSech> GetCommonSech()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.CommonSech();
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CommonSech> commonSechCollect = new MyObservableCollection<CommonSech>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        commonSechCollect.Add(new CommonSech()
                        { DC1 = (string)reader[1], IDsech1 = (int)reader[2],
                            DC2 = (string)reader[3], IDsech2 = (int)reader[4]
                        });
                    }
                }
                return commonSechCollect;
            }
        }
        public MyObservableCollection<CommonVoltage> GetCommonVoltage()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.CommonVoltage();
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CommonVoltage> commonVoltageCollect = new MyObservableCollection<CommonVoltage>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        commonVoltageCollect.Add(new CommonVoltage()
                        {
                            DC1 = (string)reader[1],
                            IDvoltage1 = (int)reader[2],
                            DC2 = (string)reader[3],
                            IDvoltage2 = (int)reader[4]
                        });
                    }
                }
                return commonVoltageCollect;
            }
        }
        public MyObservableCollection<CommonLine> GetCommonLine()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.CommonLine();
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CommonLine> commonLineCollect = new MyObservableCollection<CommonLine>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        commonLineCollect.Add(new CommonLine()
                        {
                            DC1 = (string)reader[1],
                            IdLine1 = (int)reader[2],
                            idEO1 = (int)reader[3],
                            DC2 = (string)reader[4],
                            IdLine2 = (int)reader[5],
                            idEO2 = (int)reader[6]
                        });
                    }
                }
                return commonLineCollect;
            }
        }
        public MyObservableCollection<CommonTransform> GetCommonTransform()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.CommonTransform();
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CommonTransform> commonTransformCollect = new MyObservableCollection<CommonTransform>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        commonTransformCollect.Add(new CommonTransform()
                        {
                            DC1 = (string)reader[1],
                            idTrans1 = (int)reader[2],
                            idWinding1 = (int)reader[3],
                            DC2 = (string)reader[4],
                            idTrans2 = (int)reader[5],
                            idWinding2 = (int)reader[6]
                        });
                    }
                }
                return commonTransformCollect;
            }
        }
        public MyObservableCollection<CommonBreaker> GetCommonBreaker()
        {

            var serverName = Properties.Settings.Default.ServerName ?? @"ck07-test3";
            var dbName = Properties.Settings.Default.DataBaseName ?? @"master";
            var query = SQLquery.CommonBreaker();
            var strBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var connString = strBuilder.ConnectionString;
            MyObservableCollection<CommonBreaker> commonBreakerCollect = new MyObservableCollection<CommonBreaker>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand com = new SqlCommand(query, connection))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        commonBreakerCollect.Add(new CommonBreaker()
                        {
                            DC1 = (string)reader[1],
                            idbreaker1 = (int)reader[2],
                            DC2 = (string)reader[3],
                            idBreaker2 = (int)reader[4]
                        });
                    }
                }
                return commonBreakerCollect;
            }
        }
    }
}
