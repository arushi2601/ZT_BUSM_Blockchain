using utils;

namespace Entities;

using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Database;

public class GroundStation
{
    private static readonly Database _db = new ();
    private static readonly Random _rand = new();
    public readonly RegistrationService _registrationService = new RegistrationService();
    public readonly AuthenticationService _athenticationService = new AuthenticationService();


    public class RegistrationService
    {
        private readonly Random _rand = new();

        public void addUAV(UAV dev)
        {
            _db.insert(dev.t_id, dev.c_i, dev.r_i);
        }

        public string getResponse(string hashed)
        {
            var vals = Utilities.DecodeRawHash(hashed);
            var rI = Convert.ToInt32(vals[0]);
            var tId = Convert.ToUInt32(vals[1]);
            var nounce = Convert.ToInt32(vals[2]);

            // step 3
            //Tuple<int, int> suc = new Tuple<int, int>(1,1);
            var t1 = new Stopwatch();

            var suc = _db.getCrPair(tId);
            var t2 = t1.ElapsedMilliseconds;
            t1.Restart();
            Console.WriteLine("get C R pair from DB {0}", t2);


            if (suc == null)
            {
                Console.WriteLine("UAV t_id, r_i doesnt lie in fixed range");
                System.Environment.Exit(-1);
            }

            var c = suc.Item1;
            var r = suc.Item2;
            var k1 = r ^ c;
            var k2 = c;

            var nb= _rand.Next((int)Math.Pow(2, 16));
            var X1 = nounce ^ k2;
            var Y2 = nb ^ X1 ^ k1;

            var Q = (Y2 | X1) ^ (k2 | k1);

            var t3 = t1.ElapsedMilliseconds;
            t1.Restart();
            Console.WriteLine("generated Q {0}", t3);
            Console.WriteLine("Generated Nonce : {0}", nb);
            Console.WriteLine("Q: {0}", Q);

            return Utilities.GetRawHash(Q, 1, nounce, nb);
        }


    }


    public class AuthenticationService
    {
 
        public string SessionKeyGen(string s1, string s2)
        {   
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s1 + s2)));
        }
    }
}