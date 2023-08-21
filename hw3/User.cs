namespace hw5
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string city { get; set; }

        public string Info => $"{first_name}, {last_name}, {city}";

        public User(int id, string first_name, string last_name, string phone_number, string email, string city)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.phone_number = phone_number;
            this.email = email;
            this.city = city;
        }
    }

}
