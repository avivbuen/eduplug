using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///  MajorMember
/// </summary>
public class MajorMember
{
    private int userID;
    private int majorID;
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
    public int MajorID
    {
        get
        {
            return this.majorID;
        }
        set
        {
            this.majorID = value;
        }
    }
}