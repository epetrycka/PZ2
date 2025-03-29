using System;

namespace lab4;

public class Orders{
    public string? orderid;
    public string? customerid;
    public string? employeeid;
    public string? orderdate;
    public string? requireddate;
    public string? shippeddate;
    public string? shipvia;
    public string? freight;
    public string? shipname;
    public string? shipaddress;
    public string? shipcity;
    public string? shipregion;
    public string? shippostalcode;
    public string? shipcountry;
    public Orders(string? Orderid, string? Customerid, string? Employeeid, 
                    string? Orderdate, string? Requireddate, string? Shippeddate, 
                    string? Shipvia, string? Freight, string? Shipname, 
                    string? Shipaddress, string? Shipcity, string? Shipregion, 
                    string? Shippostalcode, string? Shipcountry)
    {
        orderid = Orderid;
        customerid = Customerid;
        employeeid = Employeeid;
        orderdate = Orderdate;
        requireddate = Requireddate;
        shippeddate = Shippeddate;
        shipvia = Shipvia;
        freight = Freight;
        shipname = Shipname;
        shipaddress = Shipaddress;
        shipcity = Shipcity;
        shipregion = Shipregion;
        shippostalcode = Shippostalcode;
        shipcountry = Shipcountry;
    }
}