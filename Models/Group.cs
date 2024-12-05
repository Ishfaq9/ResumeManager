namespace ResumeManager.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime InsertedDate { get; set; } = DateTime.Now;
        public string CampaignType { get; set; }
        public virtual List<Member> Members { get; set; } 
    }
}
