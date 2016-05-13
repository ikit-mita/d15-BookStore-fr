namespace BookStore.DataAccess.WebApi
{
    class AuthModel
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public string UserName { get; set; }
    }
}
