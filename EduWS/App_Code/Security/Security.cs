using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
/// <summary>
/// ********************
/// *****Security*******
/// *****TOP SECRET*****
/// ********************
/// </summary>
public static class Security
{
    public static int SALT_SIZE = 32;//Salt size
    public static int HASH_SIZE = 64;//Hash size
    public static int PBKDF2_TTT = 512;//Hashing Iteration Count
    public static string CreateHash(string password)
    {
        //Generate random salt
        RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
        byte[] salt = new byte[SALT_SIZE];
        csprng.GetBytes(salt);

        //Generate the password hash
        byte[] hash = PBKDF2(password, salt);
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash) + ":" + Convert.ToBase64String(salt).Substring(6, 11);
    }

    private static byte[] PBKDF2(string password, byte[] salt)
    {
        //Generate hash with salt
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
        //Number of iterations
        pbkdf2.IterationCount = PBKDF2_TTT;
        //Return the hash
        return pbkdf2.GetBytes(HASH_SIZE);
    }
    private static bool SlowEqual(byte[] dbHash, byte[] passHash)
    {
        uint diff = (uint)dbHash.Length ^ (uint)passHash.Length;
        for (int i = 0; i < dbHash.Length && i < passHash.Length; i++)
            diff |= (uint)dbHash[i] ^ (uint)passHash[i];
        return (diff == 0);
    }
    public static bool ValidatePassword(string password, string dbHash)
    {
        char[] delimeter = { ':' };
        string[] split = dbHash.Split(delimeter);

        byte[] salt = Convert.FromBase64String(split[0]);
        byte[] hash = Convert.FromBase64String(split[1]);

        byte[] hashToValidate = PBKDF2(password, salt);

        return SlowEqual(hash, hashToValidate);
    }


}