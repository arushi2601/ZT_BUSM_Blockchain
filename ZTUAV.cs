using Entities;

// var db = new Database.Database();
var UAV_dev1 = new UAV(11011, 8011, 99819, 100119);
var UAV_dev2 = new UAV(11000, 8000, 99820, 100120);

var gs = new GroundStation();
UAV_dev1.Register(gs);
UAV_dev2.Register(gs);

UAV_dev1.Authenticate(ref gs, UAV_dev2);