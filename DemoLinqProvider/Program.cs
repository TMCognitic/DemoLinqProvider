// See https://aka.ms/new-console-template for more information


using DemoLinqProvider;

Service service = new Service();
int id = 1;
IQueryable<Contact> contacts = service.Contacts.Where(c => c.Id == id);

Console.WriteLine(contacts.ToString());
