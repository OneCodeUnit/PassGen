using System;
using System.Security.Cryptography;
using PassGenNewUI.Models;

namespace PassGenNewUI.Services;

public static class PasswordGeneratorService
{
    private static string AlphabetCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string AlphabetLower = "abcdefghijklmnopqrstuvwxyz";
    private static string AlphabetNumber = "0123456789";
    private static string AlphabetLine = "-_";
    private static string AlphabetSpecial = "`~!@#$%^&*()+={}[]\\|:;\"\'<>,.?/";
    private static string Password = string.Empty;
    private static int PasswordLength = 12;
    
    public static string GeneratePassword(PasswordOptions options)
    {
        PasswordLength = options.Length;
        
        if (options.ForceReadable)
        {
            AlphabetCapital = "ABCDEFGHJKMNPQRSTUVWXYZ";
            AlphabetLower = "abcdefghjkpqrstuvwxyz";
            AlphabetSpecial = "@#$%+=?";
            AlphabetNumber = "23456789";
        }

        var alphabet = string.Empty;
        if (options.UseCapital) { alphabet += AlphabetCapital; }
        if (options.UseLower) { alphabet += AlphabetLower; }
        if (options.UseNumber) { alphabet += AlphabetNumber; }
        if (options.UseLine) { alphabet += AlphabetLine; }
        if (options.UseSpecial) { alphabet += AlphabetSpecial; }

        var alphabetSize = alphabet.Length;
        if (alphabetSize == 0)
        {
            return string.Empty;
        }

        Password = string.Empty;
        for (var i = 0; i < PasswordLength; i++)
        {
            var randomNumber = RandomNumberGenerator.GetInt32(0, alphabetSize);
            var randomSymbol = alphabet[randomNumber].ToString();
            Password += randomSymbol;
        }

        if (options.ForceEachType)
        {
            ForceAddition(options);
        }

        return Password;
    }
    
    private static void ForceAddition(PasswordOptions options)
    {
        var checkedCount = 0;
        if (options.UseCapital) { checkedCount++; }
        if (options.UseLower) { checkedCount++; }
        if (options.UseNumber) { checkedCount++; }
        if (options.UseLine) { checkedCount++; }
        if (options.UseSpecial) { checkedCount++; }

        if (checkedCount > PasswordLength)
        {
            Password = string.Empty;
            return;
        }

        var positions = string.Empty;
        var position = 0;
        for (var i = 0; i < checkedCount; i++)
        {
            int randomPosition;
            do
            {
                randomPosition = RandomNumberGenerator.GetInt32(0, PasswordLength);
            }
            while (positions.Contains(randomPosition.ToString()));
            positions += randomPosition;
        }
        if (options.UseCapital)
        {
            SymbolForceAddition(positions, position, AlphabetCapital);
            position++;
        }
        if (options.UseLower)
        {
            SymbolForceAddition(positions, position, AlphabetLower);
            position++;
        }
        if (options.UseNumber)
        {
            SymbolForceAddition(positions, position, AlphabetNumber);
            position++;
        }
        if (options.UseLine)
        {
            SymbolForceAddition(positions, position, AlphabetLine);
            position++;
        }
        if (options.UseSpecial)
        {
            SymbolForceAddition(positions, position, AlphabetSpecial);
            position++;
        }
    }

    private static void SymbolForceAddition(string positions, int position, string alphabet)
    {
        var randomNumber = RandomNumberGenerator.GetInt32(0, alphabet.Length);
        var randomSymbol = alphabet[randomNumber].ToString();
        Password = Password.Remove(Convert.ToInt32(positions[position].ToString()), 1);
        Password = Password.Insert(Convert.ToInt32(positions[position].ToString()), randomSymbol);
    }
}