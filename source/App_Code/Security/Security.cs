﻿using System;
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
/* base64://8J2UsfCdlKXwnZSm8J2UsCDwnZSm8J2UsCDwnZSeIPCdlKDwnZSp8J2UnvCdlLDwnZSwIPCdlLHwnZSl8J2UnvCdlLEg8J2UovCdlKvwnZSg8J2Ur/CdlLbwnZSt8J2UsfCdlLAg8J2UnvCdlKnwnZSpIPCdlK3wnZSe8J2UsPCdlLDwnZS08J2UrPCdlK/wnZSh8J2UsCDwnZS08J2UpvCdlLHwnZSlIPCdlJ4g8J2UsPCdlLHwnZSv8J2UrPCdlKvwnZSkIPCdlKLwnZSr8J2UoPCdlK/wnZS28J2UrfCdlLHwnZSm8J2UrPCdlKs= */
public static class Security
{
public static int SALT_SIZE = 32;//Salt size
public static int HASH_SIZE = 64;//Hash size
public static int PBKDF2_TTT = 512;//Hashing Iteration Count

/// <summary>
/// Encryption creator
/// </summary>
/// <param name="password">string to encrypt</param>
/// <returns></returns>
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
