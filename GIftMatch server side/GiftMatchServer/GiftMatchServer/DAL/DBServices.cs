using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using GiftMatchServer.BL;
using System.Text.Json;

public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    //--------------------------------------------------------------------------------------------------
    // This method login a user from the user table 
    //--------------------------------------------------------------------------------------------------
    public List<Interests> getInterests()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureNoParams("sp_GetAllInterests", con);   // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Interests> list = new List<Interests>();
            while (dataReader.Read())
            {
                Interests i = new Interests();
                i.Id = Convert.ToInt32(dataReader["id"].ToString());
                i.InterestName = dataReader["InterestName"].ToString();
                i.LogoPath = dataReader["LogoPath"].ToString();
                list.Add(i);
                
            }
            return list;

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
                // close the db connection
                con.Close();
            }
        }

    } 
    public List<Big5Q> GetQuestion()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureNoParams("sp_GetBig5Q", con);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Big5Q> list = new List<Big5Q>();
            while (dataReader.Read())
            {
                Big5Q q = new Big5Q();
                q.Id = Convert.ToInt32(dataReader["id"].ToString());
                q.Qname = dataReader["Qname"].ToString();
                q.AttId = Convert.ToInt32(dataReader["attId"].ToString());
                
                list.Add(q);
                
            }
            return list;

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
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------------------------------------
    // This method Inserts a user to the user table 
    //--------------------------------------------------------------------------------------------------
    public int InsertUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUserInsertCommandWithStoredProcedure("sp_PostNewUser", con, user);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
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
                // close the db connection
                con.Close();
            }
        }

    }
    public User connect(string email,string password)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = ConnectUserCommandWithStoredProcedure("sp_Post_connectUser", con, email, password);                    // create the command
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User u = new User();
                u.Email = dataReader["email"].ToString();
                u.FirstName = dataReader["firstName"].ToString();
                u.LastName = dataReader["lastName"].ToString();
                u.Password = "";
                return u;

            }
            return null;

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
                // close the db connection
                con.Close();
            }
        }

    }

    public int InsertRecipient(Recipient recipient)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUserInsertCommandWithStoredProcedure("sp_PostNewRecipient", con, recipient);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
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
                // close the db connection
                con.Close();
            }
        }

    }
      public int InsertGiftIdea(JsonElement data)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = giftideaInsertCommandWithStoredProcedure("sp_PostNewGiftIdea", con, data);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
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
                // close the db connection
                con.Close();
            }
        }

    }
    

    ////--------------------------------------------------------------------------------------------------
    //// This method Inserts a flat to the flat table 
    ////--------------------------------------------------------------------------------------------------
    //public int InsertFlat(Flat flat)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateInsertFlatCommandWithStoredProcedure("sp_InsertFlat1", con, flat);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method Inserts a vacation to the vacation table 
    ////--------------------------------------------------------------------------------------------------
    //public int InsertVacation(Vacation vacation)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateInsertVacationCommandWithStoredProcedure("sp_InsertVacation1", con, vacation);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method updates a user from the user table 
    ////--------------------------------------------------------------------------------------------------
    //public int UpdateUser(User user, String email)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateUpdateUserCommandWithStoredProcedure("sp_UpdateUser1", con, user, email);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}

    ////--------------------------------------------------------------------------------------------------
    //// This method read avg price per night for spesicif city and month 
    ////--------------------------------------------------------------------------------------------------
    //public List<Object> ReadAvgPricePerNight(int month)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    List<Object> AvgPricePerNightList = new List<Object>();

    //    cmd = BuildReadAvgPricePerNightStoredProcedureCommand(con, "sp_CalculateAveragePricePerNight1", month);

    //    SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //    while (dataReader.Read())//run untill the table end
    //    {
    //        AvgPricePerNightList.Add(new
    //        {
    //            city = dataReader["city"].ToString(),
    //            averagePricePerNight = Convert.ToDouble(dataReader["averagePricePerNight"])
    //        });

    //    }

    //    if (con != null)
    //    {
    //        // close the db connection
    //        con.Close();
    //    }

    //    return AvgPricePerNightList;

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method reads users from the user table
    ////--------------------------------------------------------------------------------------------------
    //public List<User> ReadUsers()
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;
    //    List<User> UsersList = new List<User>();

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateReadUsersCommandWithStoredProcedureWithoutParameters("sp_ReadUsers1", con);             // create the command

    //    try
    //    {
    //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //        while (dataReader.Read())
    //        {
    //            User user = new User();
    //            user.Email = dataReader["email"].ToString();
    //            user.FirstName= dataReader["firstName"].ToString();
    //            user.FamilyName = dataReader["familyName"].ToString();
    //            user.Password = dataReader["password"].ToString();
    //            user.IsActive = Convert.ToBoolean(dataReader["isActive"]);
    //            user.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);

    //            UsersList.Add(user);
    //        }

    //        return UsersList;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method reads falts from the flat table
    ////--------------------------------------------------------------------------------------------------
    //public List<Flat> ReadFlats()
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;
    //    List<Flat> FlatsList = new List<Flat>();

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateReadFlatsCommandWithStoredProcedureWithoutParameters("sp_ReadFlats1", con);             // create the command

    //    try
    //    {
    //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //        while (dataReader.Read())
    //        {
    //            Flat flat = new Flat();
    //            flat.Id = Convert.ToInt32(dataReader["id"]);
    //            flat.City = dataReader["city"].ToString();
    //            flat.Address = dataReader["address"].ToString();
    //            flat.Price = Convert.ToDouble(dataReader["price"]);
    //            flat.Numberofroom = Convert.ToInt32(dataReader["numberofroom"]);

    //            FlatsList.Add(flat);
    //        }
    //        return FlatsList;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method reads vacations from the vacation table
    ////--------------------------------------------------------------------------------------------------
    //public List<Vacation> ReadVacations()
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;
    //    List<Vacation> vacationList = new List<Vacation>();

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateReadVacationsCommandWithStoredProcedureWithoutParameters("sp_ReadVacations1", con);             // create the command

    //    try
    //    {
    //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //        while (dataReader.Read())
    //        {
    //            Vacation vacation = new Vacation();
    //            vacation.Id = Convert.ToInt32(dataReader["id"]);
    //            vacation.UserId =dataReader["userId"].ToString();
    //            vacation.FlatId = Convert.ToInt32(dataReader["flatId"]);
    //            vacation.StartDate = Convert.ToDateTime(dataReader["startDate"]);
    //            vacation.EndDate = Convert.ToDateTime(dataReader["endDate"]);

    //            vacationList.Add(vacation);
    //        }
    //        return vacationList;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}

    ////--------------------------------------------------------------------------------------------------
    //// This method reads vacations of specific user from the database (read with parameters)
    ////--------------------------------------------------------------------------------------------------
    //public List<Vacation> GetVacationByEmail(string email)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;
    //    List<Vacation> vacationList = new List<Vacation>();

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateReadVacationsByEmailCommandWithStoredProcedureWithoutParameters("sp_ReadVacationsByEmail1", con, email);  // create the command


    //    try
    //    {
    //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //        while (dataReader.Read())
    //        {
    //            Vacation vacation = new Vacation();
    //            vacation.Id = Convert.ToInt32(dataReader["id"]);
    //            vacation.UserId = dataReader["userId"].ToString();
    //            vacation.FlatId = Convert.ToInt32(dataReader["flatId"]);
    //            vacation.StartDate = Convert.ToDateTime(dataReader["startDate"]);
    //            vacation.EndDate = Convert.ToDateTime(dataReader["endDate"]);

    //            vacationList.Add(vacation);
    //        }
    //        return vacationList;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }
    //}

    ////--------------------------------------------------------------------------------------------------
    //// This method deletes a user from the user table 
    ////--------------------------------------------------------------------------------------------------
    //public int DeleteUser(string email)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateDeleteUserCommandWithStoredProcedure("sp_DeleteUsers1", con, email);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method deletes a flat from the flat table 
    ////--------------------------------------------------------------------------------------------------
    //public int DeleteFlat(int id)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateDeleteFlatCommandWithStoredProcedure("sp_DeleteFlats1", con, id);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------------------------------------
    //// This method deletes a vacation from the vacation table 
    ////--------------------------------------------------------------------------------------------------
    //public int DeleteVacation(int id)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateDeleteVacationCommandWithStoredProcedure("sp_DeleteVacations1", con, id);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand login user using a stored procedure
    ////---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureNoParams(String spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand insert user using a stored procedure
    //---------------------------------------------------------------------------------

    private SqlCommand CreateUserInsertCommandWithStoredProcedure(String spName, SqlConnection con, User user)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@firstName", user.FirstName);
        cmd.Parameters.AddWithValue("@lastName", user.LastName);
        cmd.Parameters.AddWithValue("@password", user.Password);
        return cmd;
    }  
    private SqlCommand giftideaInsertCommandWithStoredProcedure(String spName, SqlConnection con, JsonElement data)
    {
        string GiftName = data.GetProperty("giftName").GetString();
        string Description = data.GetProperty("description").GetString();
        int Price = Convert.ToInt32(data.GetProperty("price").GetString());
        string Image = data.GetProperty("image").GetString();
        string UserName = data.GetProperty("userEmail").GetString();
        string Intrests = data.GetProperty("intrests").GetString();
        
        
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@GiftName", GiftName);
        cmd.Parameters.AddWithValue("@Description", Description);
        cmd.Parameters.AddWithValue("@Price", Price);
        cmd.Parameters.AddWithValue("@Image", Image);
        cmd.Parameters.AddWithValue("@UserEmail", UserName);
        cmd.Parameters.AddWithValue("@Intrests", Intrests);
        return cmd;
    }
    private SqlCommand CreateUserInsertCommandWithStoredProcedure(String spName, SqlConnection con, Recipient recipient)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@id", recipient.Id);
        cmd.Parameters.AddWithValue("@name", recipient.Name);
        cmd.Parameters.AddWithValue("@gender", recipient.Gender);
        cmd.Parameters.AddWithValue("@relationType", recipient.RelationType);
        cmd.Parameters.AddWithValue("@relationshipScore", recipient.RelationshipScore);
        cmd.Parameters.AddWithValue("@image", recipient.Image);
        cmd.Parameters.AddWithValue("@userEmail", recipient.UserEmail);
        cmd.Parameters.AddWithValue("@idBasket", recipient.IdBasket);
        cmd.Parameters.AddWithValue("@birthday", recipient.Birthday);
        return cmd;
    }
    private SqlCommand ConnectUserCommandWithStoredProcedure(String spName, SqlConnection con, string email,string password )
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);
        return cmd;
    }

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand update user using a stored procedure
    ////---------------------------------------------------------------------------------

    //private SqlCommand CreateUpdateUserCommandWithStoredProcedure(String spName, SqlConnection con, User user, string email)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@firstName", user.FirstName);

    //    cmd.Parameters.AddWithValue("@familyName", user.FamilyName);

    //    cmd.Parameters.AddWithValue("@email", email);

    //    cmd.Parameters.AddWithValue("@password", user.Password);

    //    cmd.Parameters.AddWithValue("@isActive", user.IsActive);

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand read user using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateReadUsersCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand delete user using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateDeleteUserCommandWithStoredProcedure(String spName, SqlConnection con, string email)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@email", email);

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand insert flat using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateInsertFlatCommandWithStoredProcedure(String spName, SqlConnection con, Flat flat)
    //{
    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


    //    cmd.Parameters.AddWithValue("@city", flat.City);

    //    cmd.Parameters.AddWithValue("@address", flat.Address);

    //    cmd.Parameters.AddWithValue("@numberofroom", flat.Numberofroom);

    //    cmd.Parameters.AddWithValue("@price", flat.Price);

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand read flat using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateReadFlatsCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand delete flat using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateDeleteFlatCommandWithStoredProcedure(String spName, SqlConnection con, int id)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@id", id);

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand insert vacation using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateInsertVacationCommandWithStoredProcedure(String spName, SqlConnection con, Vacation vacation)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@userId", vacation.UserId);

    //    cmd.Parameters.AddWithValue("@flatId", vacation.FlatId);

    //    cmd.Parameters.AddWithValue("@startDate", vacation.StartDate);

    //    cmd.Parameters.AddWithValue("@endDate", vacation.EndDate);


    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand read vacation using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateReadVacationsCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand read vacation by email using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateReadVacationsByEmailCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con, string email)
    //{
    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@email", email);

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand delete vacation using a stored procedure
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateDeleteVacationCommandWithStoredProcedure(String spName, SqlConnection con, int id)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@id", id);

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand read average price per night using a stored procedure
    ////---------------------------------------------------------------------------------
    //SqlCommand BuildReadAvgPricePerNightStoredProcedureCommand(SqlConnection con, string spName, int month)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@month", month);

    //    return cmd;

    //}

}
