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

    public List<GiftList> GetGiftList()
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

        cmd = CreateCommandWithStoredProcedureNoParams("sp_GetGiftList", con);   // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<GiftList> list = new List<GiftList>();
            while (dataReader.Read())
            {
                GiftList i = new GiftList();
                i.GiftName = dataReader["GiftName"].ToString();
                i.AttrId = Convert.ToInt32(dataReader["AttrId"].ToString());
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

    public List<GiftIdea> GetUnifiedList()
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

        cmd = CreateCommandWithStoredProcedureNoParams("sp_GetGiftIdea", con);   // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<GiftIdea> list = new List<GiftIdea>();
            while (dataReader.Read())
            {
                GiftIdea i = new GiftIdea();
                i.GiftName = dataReader["GiftName"].ToString();
                i.Description = dataReader["Description"].ToString();
                i.Price = Convert.ToInt32(dataReader["Price"].ToString());
                i.Image = dataReader["Image"].ToString();
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

    public List<GiftListInterest> GetGiftListInterest()
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

        cmd = CreateCommandWithStoredProcedureNoParams("sp_GetGiftListInterest", con);   // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<GiftListInterest> list = new List<GiftListInterest>();
            while (dataReader.Read())
            {
                GiftListInterest i = new GiftListInterest();
                i.GiftName = dataReader["GiftName"].ToString();
                i.InterestName = dataReader["InterestName"].ToString();
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


    public List<Recipient> GetRecipient(string email)
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

        cmd = CreateCommandWithStoredProcedureGetMyRecipient("sp_GetRecipient", con, email);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Recipient> list = new List<Recipient>();
            while (dataReader.Read())
            {
                Recipient r = new Recipient();
                r.Id = Convert.ToInt32(dataReader["Id"].ToString());
                r.Phone = dataReader["phone"].ToString();
                r.Name = dataReader["Name"].ToString();
                r.Gender = dataReader["Gender"].ToString();
                r.RelationType = dataReader["RelationType"].ToString();
                r.Birthday = DateTime.Parse(dataReader["Birthday"].ToString());
                r.RelationshipScore = Convert.ToInt32(dataReader["RelationshipScore"].ToString());
                r.Image = dataReader["Image"].ToString();
                r.UserEmail = dataReader["UserEmail"].ToString();
                r.IdBasket = Convert.ToInt32(dataReader["IdBasket"].ToString());


                list.Add(r);

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
    public User connect(string email, string password)
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
    public int InsertGiftAttr(string giftName, string attrString)
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

        cmd = InsertGiftAttrCommandWithStoredProcedure("sp_insertGiftAttr", con, giftName, attrString);             // create the command
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
    public int InsertGiftInterest(string giftName, string InterestsString)
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

        cmd = InsertGiftInterestCommandWithStoredProcedure("sp_insertGiftInterest", con, giftName, InterestsString);             // create the command
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

    public int InsertGiftIdea(GiftIdea gift)
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

        cmd = giftideaInsertCommandWithStoredProcedure("sp_PostNewGiftIdea", con, gift);             // create the command
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
    private SqlCommand InsertGiftInterestCommandWithStoredProcedure(String spName, SqlConnection con, string giftName, string InterestsString)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@giftIdeaString", InterestsString);
        cmd.Parameters.AddWithValue("@giftName", giftName);

        return cmd;
    }
    private SqlCommand InsertGiftAttrCommandWithStoredProcedure(String spName, SqlConnection con, string giftName, string attrString)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@giftAttrString", attrString);
        cmd.Parameters.AddWithValue("@giftName", giftName);

        return cmd;
    }
    private SqlCommand giftideaInsertCommandWithStoredProcedure(String spName, SqlConnection con, GiftIdea gift)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@GiftName", gift.GiftName);
        cmd.Parameters.AddWithValue("@Description", gift.Description);
        cmd.Parameters.AddWithValue("@Price", gift.Price);
        cmd.Parameters.AddWithValue("@Image", gift.Image);
        cmd.Parameters.AddWithValue("@UserEmail", gift.UserName);

        return cmd;
    }
    private SqlCommand CreateCommandWithStoredProcedureGetMyRecipient(String spName, SqlConnection con, string email)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@email", email);
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
        cmd.Parameters.AddWithValue("@phone", recipient.Phone);
        return cmd;
    }
    private SqlCommand ConnectUserCommandWithStoredProcedure(String spName, SqlConnection con, string email, string password)
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
