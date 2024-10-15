namespace Classic.Demo;

public class PersonViewModel
{
    public bool Selected { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public PersonViewModel(string firstName , string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}