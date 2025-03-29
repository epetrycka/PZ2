using System;

namespace lab4;

public class Territories{
    public string? territoryid;
    public string? territorydescription;
    public string? regionid;
    public Territories(string? Territoryid, string? Territorydescription, string? Regionid){
        territoryid = Territoryid;
        territorydescription = Territorydescription;
        regionid = Regionid;
    }
}