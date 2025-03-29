using System;

namespace lab4;

public class Employees{
    public string? employeeid;
    public string? lastname;
    public string? firstname;
    public string? title;
    public string? titleofcourtesy;
    public string? birthdate;
    public string? hiredate;
    public string? address;
    public string? city;
    public string? region;
    public string? postalcode;
    public string? country;
    public string? homephone;
    public string? extension;
    public string? photo;
    public string? notes;
    public string? reportsto;
    public string? photopath;
    public Employees(string? Employeeid, string? Lastname, string? Firstname, 
                     string? Title, string? Titleofcourtesy, string? Birthdate, 
                     string? Hiredate, string? Address, string? City, 
                     string? Region, string? Postalcode, string? Country, 
                     string? Homephone, string? Extension, string? Photo, 
                     string? Notes, string? Reportsto, string? Photopath) 
    {
        employeeid = Employeeid;
        lastname = Lastname;
        firstname = Firstname;
        title = Title;
        titleofcourtesy = Titleofcourtesy;
        birthdate = Birthdate;
        hiredate = Hiredate;
        address = Address;
        city = City;
        region = Region;
        postalcode = Postalcode;
        country = Country;
        homephone = Homephone;
        extension = Extension;
        photo = Photo;
        notes = Notes;
        reportsto = Reportsto;
        photopath = Photopath;
    }
}