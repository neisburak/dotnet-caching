namespace Shared.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Website { get; set; } = default!;

}

public class UserAddress
{
    public string Street { get; set; } = default!;
    public string Suite { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Zipcode { get; set; } = default!;
    public AddressGeo Geo { get; set; } = default!;
}

public class AddressGeo
{
    public string Lat { get; set; } = default!;
    public string Lng { get; set; } = default!;
}

public class UserCompany
{
    public string Name { get; set; } = default!;
    public string CatchPrase { get; set; } = default!;
    public string Bs { get; set; } = default!;
}