using Entities;
using System.Diagnostics;

// var db = new Database.Database();
var UAV_dev1 = new UAV(11011, 8011, 99819, 100119);
var UAV_dev2 = new UAV(11000, 8000, 99820, 100120);

var gs = new GroundStation();

var t1 = new Stopwatch();

gs._registrationService.addUAV(UAV_dev2);

var t2 = t1.ElapsedMilliseconds;
t1.Restart();
Console.WriteLine("UAV 1 Add to DB {0}", t2);
gs._registrationService.addUAV(UAV_dev1);

var t3 = t1.ElapsedMilliseconds;
t1.Restart();
Console.WriteLine("UAV 2 Add to DB {0}", t3);

var s1 = UAV_dev1.Register(gs);

var t4 = t1.ElapsedMilliseconds;
t1.Restart();
Console.WriteLine("UAV 1 GS Auth {0}", t4);


var s2 = UAV_dev2.Register(gs);

var t5 = t1.ElapsedMilliseconds;
t1.Restart();
Console.WriteLine("UAV 2 GS Auth {0}", t5);

var sk12 = gs._athenticationService.SessionKeyGen(s1.ToString(), s2.ToString());
var t6 = t1.ElapsedMilliseconds;
t1.Restart();
Console.WriteLine("UAV auth session key generation {0}", t6);
// UAV_dev1.Authenticate(ref gs, UAV_dev2);