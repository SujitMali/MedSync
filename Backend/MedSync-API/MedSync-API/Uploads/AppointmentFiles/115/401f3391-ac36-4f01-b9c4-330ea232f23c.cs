
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
/// Business class representing Tags
/// </summary>
public class Tags
{
#region Basic Functionality

#region Variable Declaration

   /// <summary>
   /// Variable to store Database object to interact with database.
   /// </summary>
   private Database db;
#endregion

#region Constructors

   /// <summary>
   /// Initializes a new instance of the Tags class.
   /// </summary>
   public Tags()
   {
      this.db  = DatabaseFactory.CreateDatabase();
   }

   /// <summary>
   /// Initializes a new instance of the Tags class.
   /// </summary>
   /// <param name="tagId">Sets the value of TagId.</param>
   public Tags(int tagId)
   {
      this.db = DatabaseFactory.CreateDatabase();
      this.TagId = tagId;
   }
#endregion

#region Properties

   /// <summary>
   /// Gets or sets TagId
   /// </summary>
   public int TagId
   {
      get; set;
   }

   /// <summary>
   /// Gets or sets Tag
   /// </summary>
   public string Tag
   {
      get; set;
   }

   /// <summary>
   /// Gets or sets IsActive
   /// </summary>
   public bool IsActive
   {
      get; set;
   }

   /// <summary>
   /// Gets or sets CreatedBy
   /// </summary>
   public int CreatedBy
   {
      get; set;
   }

   /// <summary>
   /// Gets or sets CreatedOn
   /// </summary>
   public DateTime CreatedOn
   {
      get; set;
   }

   /// <summary>
   /// Gets or sets ModifiedBy
   /// </summary>
   public int ModifiedBy
   {
      get; set;
   }

   /// <summary>
   /// Gets or sets ModifiedOn
   /// </summary>
   public DateTime ModifiedOn
   {
      get; set;
   }
#endregion

#region Actions

   /// <summary>
   /// Loads the details for Tags.
   /// </summary>
   /// <returns>True if Load operation is successful; Else False.</returns>
   public bool Load()
   {
      try
      {
         if (this.TagId != 0)
         {
            DbCommand com = this.db.GetStoredProcCommand("TagsGetDetails");
            this.db.AddInParameter(com, "TagId", DbType.Int32, this.TagId);
            DataSet ds = this.db.ExecuteDataSet(com);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               DataTable dt = ds.Tables[0];
               this.TagId = Convert.ToInt32(dt.Rows[0]["TagId"]);
               this.Tag = Convert.ToString(dt.Rows[0]["Tag"]);
               this.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
               this.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
               this.CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]);
               this.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
               this.ModifiedOn = Convert.ToDateTime(dt.Rows[0]["ModifiedOn"]);
               return true;
            }
         }

      return false;
      }
      catch (Exception ex)
      {
         // To Do: Handle Exception
         return false;
      }
   }

   /// <summary>
   /// Inserts details for Tags if TagId = 0.
   /// Else updates details for Tags.
   /// </summary>
   /// <returns>True if Save operation is successful; Else False.</returns>
   public bool Save()
   {
      if (this.TagId == 0)
      {
         return this.Insert();
      }
      else
      {
         if (this.TagId > 0)
         {
            return this.Update();
         }
         else
         {
            this.TagId = 0;
            return false;
         }
      }
   }

   /// <summary>
   /// Inserts details for Tags.
   /// Saves newly created Id in TagId.
   /// </summary>
   /// <returns>True if Insert operation is successful; Else False.</returns>
   private bool Insert()
   {
      try
      {
         DbCommand com = this.db.GetStoredProcCommand("TagsInsert");
         this.db.AddOutParameter(com, "TagId", DbType.Int32, 1024);
         if (!String.IsNullOrEmpty(this.Tag))
         {
            this.db.AddInParameter(com, "Tag", DbType.String, this.Tag);
         }
         else
         {
            this.db.AddInParameter(com, "Tag", DbType.String, DBNull.Value);
         }
         this.db.AddInParameter(com, "IsActive", DbType.Boolean, this.IsActive);
         if (this.CreatedBy > 0)
         {
            this.db.AddInParameter(com, "CreatedBy", DbType.Int32, this.CreatedBy);
         }
         else
         {
            this.db.AddInParameter(com, "CreatedBy", DbType.Int32, DBNull.Value);
         }
         if (this.CreatedOn > DateTime.MinValue)
         {
            this.db.AddInParameter(com, "CreatedOn", DbType.DateTime, this.CreatedOn);
         }
         else
         {
            this.db.AddInParameter(com, "CreatedOn", DbType.DateTime, DBNull.Value);
         }
        
         this.db.ExecuteNonQuery(com);
         this.TagId = Convert.ToInt32(this.db.GetParameterValue(com, "TagId"));      // Read in the output parameter value
      }
      catch (Exception ex)
      {
         // To Do: Handle Exception
         return false;
      }

      return this.TagId > 0; // Return whether ID was returned
   }

   /// <summary>
   /// Updates details for Tags.
   /// </summary>
   /// <returns>True if Update operation is successful; Else False.</returns>
   private bool Update()
   {
      try
      {
         DbCommand com = this.db.GetStoredProcCommand("TagsUpdate");
         this.db.AddInParameter(com, "TagId", DbType.Int32, this.TagId);
         if (!String.IsNullOrEmpty(this.Tag))
         {
            this.db.AddInParameter(com, "Tag", DbType.String, this.Tag);
         }
         else
         {
            this.db.AddInParameter(com, "Tag", DbType.String, DBNull.Value);
         }
         this.db.AddInParameter(com, "IsActive", DbType.Boolean, this.IsActive);
         if (this.ModifiedBy > 0)
         {
            this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, this.ModifiedBy);
         }
         else
         {
            this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, DBNull.Value);
         }
         if (this.ModifiedOn > DateTime.MinValue)
         {
            this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, this.ModifiedOn);
         }
         else
         {
            this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, DBNull.Value);
         }
         this.db.ExecuteNonQuery(com);
      }
      catch (Exception ex)
      {
         // To Do: Handle Exception
         return false;
      }

      return true;
   }

   /// <summary>
   /// Deletes details of Tags for provided TagId.
   /// </summary>
   /// <returns>True if Delete operation is successful; Else False.</returns>
   public bool Delete()
   {
      try
      {
         DbCommand com = this.db.GetStoredProcCommand("TagsDelete");

         this.db.AddInParameter(com, "TagId", DbType.Int32, this.TagId);

         if (this.ModifiedBy > 0)
         {
            this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, this.ModifiedBy);
         }
         else
         {
            this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, DBNull.Value);
         }
         if (this.ModifiedOn > DateTime.MinValue)
         {
            this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, this.ModifiedOn);
         }
         else
         {
            this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, DBNull.Value);
         }
         this.db.ExecuteNonQuery(com);
      }
      catch (Exception ex)
      {
         // To Do: Handle Exception
         return false;
      }

      return true;
   }

   /// <summary>
   /// Get list of Tags for provided parameters.
   /// </summary>
   /// <returns>DataSet of result</returns>
   /// <remarks></remarks>
   public DataSet GetList()
   {
      DataSet ds = null;
      try
      {
         DbCommand com = db.GetStoredProcCommand("TagsGetList");
         ds = db.ExecuteDataSet(com);
      }
      catch (Exception ex)
      {
         //To Do: Handle Exception
      }

      return ds;
   }
#endregion

#endregion
}

