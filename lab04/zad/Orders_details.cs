using System;

namespace lab4;

public class Orders_details{
    public string? orderid;
    public string? productid;
    public string? unitprice;
    public string? quantity;
    public string? discount;
    public Orders_details(string? Orderid, string? Productid, string? Unitprice, string? Quantity, string? Discount){
        orderid = Orderid;
        productid = Productid;
        unitprice = Unitprice;
        quantity = Quantity;
        discount = Discount;
    }
}