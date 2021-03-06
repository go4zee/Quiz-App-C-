﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizApp.Core.Database
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Website")]
	public partial class PortalDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public PortalDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["WebsiteConnectionString1"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PortalDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PortalDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PortalDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PortalDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.[User]")]
	public partial class User
	{
		
		private System.Guid _UserID;
		
		private string _UserName;
		
		private string _UserFirstName;
		
		private string _UserLastName;
		
		private string _Password;
		
		private int _UserAccessLevel;
		
		private string _UserEmailAddress;
		
		private System.Nullable<System.DateTime> _UserCreatedOn;
		
		private System.Nullable<System.DateTime> _UserModifiedOn;
		
		private bool _UserActive;
		
		private System.Nullable<System.Guid> _UserCreatedByUserID;
		
		private System.Nullable<System.Guid> _UserModifiedByUserID;
		
		public User()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this._UserID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserFirstName", DbType="VarChar(50)")]
		public string UserFirstName
		{
			get
			{
				return this._UserFirstName;
			}
			set
			{
				if ((this._UserFirstName != value))
				{
					this._UserFirstName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserLastName", DbType="VarChar(50)")]
		public string UserLastName
		{
			get
			{
				return this._UserLastName;
			}
			set
			{
				if ((this._UserLastName != value))
				{
					this._UserLastName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this._Password = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserAccessLevel", DbType="Int NOT NULL")]
		public int UserAccessLevel
		{
			get
			{
				return this._UserAccessLevel;
			}
			set
			{
				if ((this._UserAccessLevel != value))
				{
					this._UserAccessLevel = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserEmailAddress", DbType="VarChar(255)")]
		public string UserEmailAddress
		{
			get
			{
				return this._UserEmailAddress;
			}
			set
			{
				if ((this._UserEmailAddress != value))
				{
					this._UserEmailAddress = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserCreatedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> UserCreatedOn
		{
			get
			{
				return this._UserCreatedOn;
			}
			set
			{
				if ((this._UserCreatedOn != value))
				{
					this._UserCreatedOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserModifiedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> UserModifiedOn
		{
			get
			{
				return this._UserModifiedOn;
			}
			set
			{
				if ((this._UserModifiedOn != value))
				{
					this._UserModifiedOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserActive", DbType="Bit NOT NULL")]
		public bool UserActive
		{
			get
			{
				return this._UserActive;
			}
			set
			{
				if ((this._UserActive != value))
				{
					this._UserActive = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserCreatedByUserID", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> UserCreatedByUserID
		{
			get
			{
				return this._UserCreatedByUserID;
			}
			set
			{
				if ((this._UserCreatedByUserID != value))
				{
					this._UserCreatedByUserID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserModifiedByUserID", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> UserModifiedByUserID
		{
			get
			{
				return this._UserModifiedByUserID;
			}
			set
			{
				if ((this._UserModifiedByUserID != value))
				{
					this._UserModifiedByUserID = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
