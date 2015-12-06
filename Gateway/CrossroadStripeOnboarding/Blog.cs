namespace CrossroadStripeOnboarding
{
    using System.Collections.Generic;

    public class Blog
    {
        public Blog()
        {
            Posts = new HashSet<Post>();
        }

        public int BlogId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
