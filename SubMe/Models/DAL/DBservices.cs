using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using SubMe.Models;

namespace SubMe.Models.DAL
{
    public class DBservices
    {
        public SqlDataAdapter da;
        public DataTable dt;

        public DBservices()
        {
        }

        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //public void Update()
        //{
        //    // the command build will automatically create insert/update/delete commands according to the select command
        //    SqlCommandBuilder builder = new SqlCommandBuilder(da);
        //    da.Update(dt);
        //}

        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }


        public int Insert(User use)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionStringName"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String cStr = BuildInsertCommand(use);      // helper method to build the insert string            

            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                int UserId = Convert.ToInt32(cmd.ExecuteScalar());
                //int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return UserId;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------
        // Build the Insert command String
        //--------------------------------------------------------------------
        private String BuildInsertCommand(User use)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string
            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}')", use.UserFBID, use.FirstName, use.LastName, use.Gender, use.Age.ToString(), use.UserImage, use.Email);
            String prefix = "INSERT INTO UsersTbl (UserFBID, FirstName, LastName, Gender, Age, UserImage, Email) output INSERTED.Id ";
            command = prefix + sb.ToString();

            return command;
        }

        public int CheckUserExist(string fbid)
        {
            SqlConnection con = null;
            try
            {
                con = connect("ConnectionStringName"); // create the connection

                String selectSTR = "SELECT UserFBID FROM UsersTbl";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                var Exist = 0;
                while (dr.Read())
                {
                    if (fbid == (string)dr["UserFBID"])
                    {
                        return (Exist = 1);
                    }
                }
                return (Exist);
                //throw new Exception("User not found!");
            }
            catch (Exception ex)
            {
                throw (ex); // write to log
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


        public List<Location> GetLocations()
        {
            SqlConnection con = null;
            List<Location> ll = new List<Location>();
            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM SubletTbl s inner join LocationTbl l on s.SubletID = l.SubletID ORDER BY Popularity DESC";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    if (ll.Count == 10) break;
                    Location l = new Location();
                    l.SubletID = Convert.ToInt32(dr["SubletID"]);
                    l.City = (string)dr["City"];
                    l.Rout = (string)dr["Rout"];
                    l.StreetAddress = (string)dr["StreetAddress"];
                    l.Lat = (string)dr["Lat"];
                    l.Lng = (string)dr["Lng"];
                    l.PlaceID = (string)dr["PlaceID"];
                    ll.Add(l);
                }

                return ll;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Location> GetSearchedLocations(string IdString)
        {
            SqlConnection con = null;
            List<Location> ll = new List<Location>();
            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM LocationTbl WHERE " + IdString;                
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Location l = new Location();
                    l.SubletID = Convert.ToInt32(dr["SubletID"]);
                    l.City = (string)dr["City"];
                    l.Rout = (string)dr["Rout"];
                    l.StreetAddress = (string)dr["StreetAddress"];
                    l.Lat = (string)dr["Lat"];
                    l.Lng = (string)dr["Lng"];
                    l.PlaceID = (string)dr["PlaceID"];
                    ll.Add(l);
                }

                return ll;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Sublet> SearchSublets(string City, string EnterDate, string ExitDate, string Type, int Rooms, int MinBudjet, int MaxBudjet)
        {
            SqlConnection con = null;
            SqlConnection con2 = null;
            List<Location> ll = new List<Location>();
            List<Sublet> sl = new List<Sublet>();

            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM LocationTbl WHERE City LIKE '" + City + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Find all sublets in requested city
                    Location l = new Location();
                    l.SubletID = Convert.ToInt32(dr["SubletID"]);
                    l.City = (string)dr["City"];
                    l.Rout = (string)dr["Rout"];
                    l.StreetAddress = (string)dr["StreetAddress"];
                    l.Lat = (string)dr["Lat"];
                    l.Lng = (string)dr["Lng"];
                    l.PlaceID = (string)dr["PlaceID"];
                    ll.Add(l);
                }

                if (ll.Count == 0)
                {
                    throw new Exception("NO RESULTS FOUND IN REQUESTED CITY!");
                }

                else
                { //Get all sublets from DB
                    string SubStr = "SELECT * from SubletTbl WHERE Price BETWEEN " + MinBudjet + " AND " + MaxBudjet;
                    con2 = connect("ConnectionStringName");
                    SqlCommand cmd2 = new SqlCommand(SubStr, con2);
                    SqlDataReader dr2 = cmd2.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr2.Read())
                    {   
                        Sublet s = new Sublet();
                        s.SubletID = Convert.ToInt32(dr2["SubletID"]);
                        s.CheckIn = (DateTime)dr2["CheckIn"];
                        s.CheckOut = (DateTime)dr2["CheckOut"];
                        s.Price = Convert.ToInt32(dr2["Price"]);
                        s.NomOfRooms = Convert.ToInt32(dr2["NumOfRooms"]);
                        s.SqMtr = Convert.ToInt32(dr2["SquareMeter"]);
                        s.FloorNo = Convert.ToInt32(dr2["FloorNu"]);
                        s.Description = (string)dr2["SubDescription"];
                        s.isFacebook = (bool)dr2["isFacebook"];
                        s.Roommates = Convert.ToInt32(dr2["Roommates"]);
                        sl.Add(s);
                    }
                }
                return sl;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con2.Close();
                }
            }
        }

        public List<Sublet> GetLikedSublets(string UserFBID)
        {
            SqlConnection con = null;
            List<Sublet> sl = new List<Sublet>();
            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM LikedSubletsTbl inner join SubletTbl on LikedSubletsTbl.SubletID = SubletTbl.SubletID WHERE UserFBID = '" + UserFBID + "';";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Sublet s = new Sublet();
                    s.SubletID = Convert.ToInt32(dr["SubletID"]);
                    s.CheckIn = (DateTime)dr["CheckIn"];
                    s.CheckOut = (DateTime)dr["CheckOut"];
                    s.Price = Convert.ToInt32(dr["Price"]);
                    s.NomOfRooms = Convert.ToInt32(dr["NumOfRooms"]);
                    s.SqMtr = Convert.ToInt32(dr["SquareMeter"]);
                    s.FloorNo = Convert.ToInt32(dr["FloorNu"]);
                    s.Description = (string)dr["SubDescription"];
                    s.isFacebook = (bool)dr["isFacebook"];
                    s.Roommates = Convert.ToInt32(dr["Roommates"]);
                    sl.Add(s);
                }

                return sl;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public int UpdateLikedSublets(LikedSublets[] ls)
        {
            SqlConnection con;
            SqlConnection con2;

            try
            {
                con = connect("ConnectionStringName"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String selectSTR = "DELETE FROM LikedSubletsTbl WHERE UserFBID = '" + ls[0].UserFBID + "';";
            SqlCommand cmd = new SqlCommand(selectSTR, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command

                //Insert Liked Sublets
                try
                {
                    con2 = connect("ConnectionStringName"); // create the connection
                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }

                //Build Insert Command
                String selectSTR2 = "";
                for (int i = 0; i < ls.Length; i++)
                {
                    selectSTR2 += "INSERT INTO LikedSubletsTbl (UserFBID, SubletID) VALUES ('" + ls[i].UserFBID + "', " + ls[i].SubletID + ");";
                }

                SqlCommand cmd2 = new SqlCommand(selectSTR2, con2);      // create the command
                numEffected = cmd2.ExecuteNonQuery();                    // execute the command
                //End Insert Liked Sublets

                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int DeleteLikedSublet(LikedSublets ls)
        {
            SqlConnection con;

            try
            {
                con = connect("ConnectionStringName"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String selectSTR = "DELETE FROM LikedSubletsTbl WHERE UserFBID = '" + ls.UserFBID + "' AND SubletID = " + ls.SubletID + ";";
            SqlCommand cmd = new SqlCommand(selectSTR, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<SubletImage> GetSubletImages(string SubId)
        {
            SqlConnection con = null;
            List<SubletImage> sil = new List<SubletImage>();
            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM SubImgTbl WHERE " + SubId + ";";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    SubletImage si = new SubletImage();
                    si.SubletID = Convert.ToInt32(dr["SubletID"]);
                    si.ImagePath1 = (string)dr["ImgPath1"];
                    si.ImagePath2 = (string)dr["ImgPath2"];
                    si.ImagePath3 = (string)dr["ImgPath3"];
                    si.ImagePath4 = (string)dr["ImgPath4"];
                    si.ImagePath5 = (string)dr["ImgPath5"];
                    sil.Add(si);
                }

                return sil;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Sublet> GetSubletSearchWithProp(string SubletQuery, string PropQuery)
        {
            SqlConnection con = null;
            List<Sublet> sl = new List<Sublet>();
            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM PropTbl p inner join SubletTbl s on p.SubPropID = s.SubletID WHERE " + SubletQuery + " AND " + PropQuery + ";";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Sublet s = new Sublet();
                    s.SubletID = Convert.ToInt32(dr["SubletID"]);
                    s.CheckIn = (DateTime)dr["CheckIn"];
                    s.CheckOut = (DateTime)dr["CheckOut"];
                    s.Price = Convert.ToInt32(dr["Price"]);
                    s.NomOfRooms = Convert.ToInt32(dr["NumOfRooms"]);
                    s.SqMtr = Convert.ToInt32(dr["SquareMeter"]);
                    s.FloorNo = Convert.ToInt32(dr["FloorNu"]);
                    s.Description = (string)dr["SubDescription"];
                    s.isFacebook = (bool)dr["isFacebook"];
                    s.Roommates = Convert.ToInt32(dr["Roommates"]);
                    sl.Add(s);
                }

                return sl;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // ------------------  SMART ALGO' START   ------------------------

        // פונקציה שמכניסה למסד שורה של העדפות חיפוש למשתמש 
        public int InsertUserPreferences(UserPreferences p)
        {
            SqlConnection con;
            SqlCommand cmd;
            string query;
            StringBuilder sb = new StringBuilder();
            String prefix;
            int numEffected;

            try
            {
                con = connect("ConnectionStringName"); // create the connection
            }

            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            sb.AppendFormat("Values('{0}', {1}, {2}, {3})", p.UserId, p.DurationKod, p.PriceKod, p.CityKod);
            prefix = "INSERT INTO UserPreferencesTbl (UserId, DurationKod, PriceKod, CityKod)";
            query = prefix + sb.ToString(); //כאן יש לי כרגע את הפקודה להכניס רשומות לטבלת - וקטור העדפות חיפוש למשתמש

            cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 3;

            try
            {
                numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            finally
            {
                con.Close();
            }
        }

        //פונקציה שמחזירה את כל העדפות החיפוש של משתמש יחיד בהתאם לתז שלו  
        public List <UserPreferences> GetListOfSearchAttribute(string uid)
        {
            SqlConnection con = null;
            List<UserPreferences> upl = new List<UserPreferences>();

            try
            {
                con = connect("ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM UserPreferencesTbl WHERE UserId = '" + uid + "'; ";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    UserPreferences P = new UserPreferences();
                    P.DurationKod = Convert.ToInt32(dr["DurationKod"]);
                    P.PriceKod = Convert.ToInt32(dr["PriceKod"]);
                    P.CityKod = Convert.ToInt32(dr["CityKod"]);

                    upl.Add(P);
                }

                return upl;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


        //========================================UserProfile

        // פונקציה שמכניסה למסד שורת פרופיל של משתמש 
        public int InsertUserProfile(UserProfile UP)
        {
            SqlConnection con;
            SqlCommand cmd;
            string query;
            StringBuilder sb = new StringBuilder();
            String prefix;
            int numEffected;

            try
            {
                con = connect("ConnectionStringName");
            }

            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            DeleteUserProfile(UP.UserId); // אור

            sb.AppendFormat("Values('{0}', {1}, {2}, {3}, {4}, {5}, {6})", UP.UserId, UP.DurationKod, UP.DurationBelonging, UP.DurationDeviationValue, UP.PriceKod, UP.PriceBelonging, UP.PriceDeviationValue);
            prefix = "INSERT INTO VectorProfileTbl (UserId, DurationKod, DurationBelonging, DurationDeviationValue, PriceKod, PriceBelonging, PriceDeviationValue)";
            query = prefix + sb.ToString(); //כאן יש לי כרגע את הפקודה להכניס רשומות לטבלת - וקטור פרופיל למשתמש

            cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 3;

            try
            {
                numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            finally
            {
                con.Close();
            }
        }

        //=============================CityBelonging


        //  פונקציה שמכניסה למסד שורת שיוך של משתמש לעיר מסויימת 
        public int InsertOneUserCityBelonging(CityBelonging CB)
        {
            SqlConnection con;
            SqlCommand cmd;
            string query;
            StringBuilder sb = new StringBuilder();
            String prefix;
            int numEffected;

            try
            {
                con = connect("ConnectionStringName");
            }

            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            sb.AppendFormat("Values('{0}', {1}, {2}, {3}, {4}, {5})", CB.UserId, CB.WithoutPreferencePercent, CB.HaifaPercent, CB.JerusalemPercent, CB.TlvPercent, CB.EilatPercent);
            prefix = "INSERT INTO CityBelongingTbl (UserId, WithoutPreferencePrecent, HaifaPercent, JerusalemPercent, TlvPercent, EilatPercent)";
            query = prefix + sb.ToString(); //כאן יש לי כרגע את הפקודה להכניס רשומות לטבלת - וקטור העדפות חיפוש למשתמש

            cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 3;

            try
            {
                numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            finally
            {
                con.Close();
            }
        }

        //פונקציה שמחזירה את נתוני השייכות של משתמש יחיד לעיר מסויימת בהתאם לתז שלו   
        public CityBelonging ReturnOneUserCityBelonging(string id)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con;

            try
            {
                con = connect("ConnectionStringName");
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            string q = "SELECT * FROM CityBelongingTbl WHERE UserId = " + id;
            cmd = new SqlCommand(q, con);

            CityBelonging CB = new CityBelonging();
            CB.UserId = (string)dr["UserId"]; 
            CB.WithoutPreferencePercent = (float)Convert.ToDouble(dr["WithoutPreferencePresent"]);
            CB.HaifaPercent = (float)Convert.ToDouble(dr["HaifaPercent"]);
            CB.JerusalemPercent = (float)Convert.ToDouble(dr["JerusalemPercent"]);
            CB.TlvPercent = (float)Convert.ToDouble(dr["TlvPercent"]);
            CB.EilatPercent = (float)Convert.ToDouble(dr["EilatPercent"]);

            con.Close();
            return CB;
        }

      //=================================  Notifications


        // פונקציה שמכניסה למסד שורה של העדפות חיפוש למשתמש 
        public int InsertNotifications(string UserId, int SubletId, float MatchPercentages)
        {
            SqlConnection con;
            SqlCommand cmd;
            string query;
            StringBuilder sb = new StringBuilder();
            String prefix;
            int numEffected;

            try
            {
                con = connect("ConnectionStringName"); // create the connection
            }

            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            sb.AppendFormat("Values('{0}', {1}, {2})", UserId, SubletId, MatchPercentages);
            prefix = "INSERT INTO NotificationsTbl (UserId, SubletId, MatchPercentages)";
            query = prefix + sb.ToString(); //כאן יש לי כרגע את הפקודה להכניס רשומות לטבלת - התראות , התראה חדשה למשתמש

            cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 3;

            try
            {
                numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            finally
            {
                con.Close();
            }
        }

        // פונקציה שמוחקת מהמסד שורת פרופיל משתמש מחפש עמ לשמור על כך שתהיה שורה אחת בלבד שמאפיינת אותו, הפונקציה מקבלת את התז של המשתמש . - אור חדש
        public int DeleteUserProfile(string UserId)
        {
            SqlConnection con;
            SqlCommand cmd;
            string query;
            StringBuilder sb = new StringBuilder();
            int numEffected;

            try
            {
                con = connect("ConnectionStringName");
            }

            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            query = "DELETE FROM VectorProfileTbl WHERE UserId = '" + UserId + "' ; ";
            //כאן יש לי כרגע את הפקודה למחוק פרופיל משתמש - כלומר וקטור פרופיל למשתמש

            cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 3;

            try
            {
                numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }

            finally
            {
                con.Close();
            }
        }

        // ------------------  SMART ALGO' END   ------------------------

    } //Closing the class - WRITE ONLY UP FROM HERE
}