using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Member structure for business logic
/// </summary>
public class Member
{
    //Attributes
    private int userID;//The DB id of the user (A_I-PK)
    private string userCivilianID;//The israeli/passport id of user
    private string userFirstName;//The first name of the user
    private string userLastName;//The last name of the user
    //END

    //GET/SET
    /// <summary>
    /// The DB id of the user (A_I-PK) - DO NOT ENTER YOURSELF
    /// </summary>
    public int UserID
    {
        get
        {
            return this.userID;
        }
        set
        {
            this.userID = value;
        }
    }
    /// <summary>
    /// The israeli/passport id of user
    /// </summary>
    public string ID
    {
        get
        {
            return this.userCivilianID;
        }
        set
        {
            this.userCivilianID = value;
        }
    }
    /// <summary>
    /// The first name of the user
    /// </summary>
    public string FirstName
    {
        get
        {
            return this.userFirstName;
        }
        set
        {
            this.userFirstName = value;
        }
    }
    /// <summary>
    /// The last name of the user
    /// </summary>
    public string LastName
    {
        get
        {
            return this.userLastName;
        }
        set
        {
            this.userLastName = value;
        }
    }
    public string Name
    {
        get
        {
            return this.FirstName + ' ' + this.LastName;
        }
        set
        {
            string[] val = value.Split(' ');
            if (val.Length == 2)
            {
                FirstName = val[0];
                LastName = val[1];
            }
        }
    }
 

}