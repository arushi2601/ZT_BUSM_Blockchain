using System.ComponentModel.DataAnnotations;
using utils;

namespace Entities;

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

public class UAV
{
    public uint t_id { get; }
    public uint r_i { get; }
    public int nounce { get; }
    public DateTime timestamp { get; }
    public uint c_i { get; }

    private readonly Random _rand = new();

    private uint gId { get; set; }


    public UAV(uint tId, uint rI, uint cI, int Nounce)
    {
        t_id = tId;
        // r = puf(c)
        r_i = rI;
        c_i = cI;
        // Na
        nounce = Nounce;
    }

    public long Register(GroundStation gs)
    {
        string GetUavRawHash()
        {
            return Utilities.GetRawHash(r_i, t_id, nounce);
        }


        long getSessionKey(string hashedResponse)
        {
            var vals = hashedResponse.Split("@@@@@");

            // var Q = Convert.ToInt32(vals[0]);
            var nounce = Convert.ToInt32(vals[2]);
            var nb= Convert.ToInt32(vals[3]);


            var k1 = r_i ^ c_i;
            var k2 = c_i;
            var X1 = nounce ^ k2;
            var Y2 = nb ^ X1 ^ k1;

            var n_c = _rand.Next((int)(Math.Pow(2, 31) - 1));
            return (k1 ^ nb )| (k2 ^ n_c);
        }
        // step 1,2,5

        // step 1
        var t1 = new Stopwatch();
        
        var uavHash = GetUavRawHash();
        
        var t2 = t1.ElapsedMilliseconds;
        t1.Restart();
        Console.WriteLine("UAV raw hash {0}", t2.ToString());

        var res = gs._registrationService.getResponse(uavHash);
        
        var t3 = t1.ElapsedMilliseconds;
        t1.Restart();
        Console.WriteLine("Response from Registration Service {0}", t3.ToString());


        //step 6
        var suc = getSessionKey(res);
        
        var t4 = t1.ElapsedMilliseconds;
        t1.Restart();
        Console.WriteLine("Response from Registration Service {0}", t3);

        //gId = gs._registrationService.AddToDb(c_i, r_i);
        return suc;
    }


}