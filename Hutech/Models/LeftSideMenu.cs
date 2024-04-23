namespace Hutech.Models
{
    public class LeftSideMenu
    {
        public LeftSideMenu()
        {
            SubMenus = new List<SubMenu>();
        }
        public string Name { get; set; }
        public long Id { get; set; }  
        public long ParentId { get; set; }  
        public bool Isdeleted { get; set; }
        public string URL { get; set; }
        public int Sort { get; set; }
        public List<SubMenu> SubMenus { get; set; } 
        public string ParentName { get; set; }
    }
    public class SubMenu
    {
        public string SubMenuName { get; set; }
        public string URL { get; set; }
        public int Sort { get; set; }
    }
}
