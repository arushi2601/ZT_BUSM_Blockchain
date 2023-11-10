using System;
using System.Security.Cryptography;
using System.Text;
using utils;
using CRPair = System.Tuple<uint, uint>;

namespace Database;

public class Database
{
    private Dictionary<uint, CRPair?> _registered { get; } = new();


    public Database()
    {
        Console.WriteLine("\n=====================Initializing Db=====================\n");
    }

    public CRPair? getCrPair(uint tId)
    {
        if (_registered.TryGetValue(tId, out CRPair? crPair))
        {
            return crPair;
        }
        else
        {
            return null;
        }
    }

    public bool insert(uint tId, uint c, uint r)
    {
        if (_registered.ContainsKey(tId))
        {
            return false;
        }

        _registered.Add(tId, new CRPair(c, r));
        return true;
    }
}