using System.Collections.Generic;
using System.DirectoryServices;
using Newtonsoft.Json;

namespace PopOrgChart
{
    public class ADUser 
    {
        public string id { get; set; }
        public string name { get; set; }
        public string data { get; set; }
        public string mail { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public List<ADUser> children { get; set; }
        [JsonIgnore]
        public bool empty { get; set; }
        [JsonIgnore]
        public string Manager { get; set; }
        [JsonIgnore]
        public string FullName { get; set; }

        internal static string ConvertManager(string DN)
        {
            if (DN == "")
                return string.Empty;
            DirectoryEntry entry = new DirectoryEntry("LDAP://DC=Example,DC=com");
            DirectorySearcher DSearch = new DirectorySearcher(entry);
            DSearch.Filter = "(&(objectCategory=person)(!userAccountControl:1.2.840.113556.1.4.803:=2)(distinguishedName="+ DN + "))";
            DSearch.PropertiesToLoad.Add("sn");
            DSearch.PropertiesToLoad.Add("givenName");
            var manager = DSearch.FindOne();
            if (manager == null)
            {
                return string.Empty;
            }
            else
            {
                return (manager.Properties["givenName"][0].ToString() + " " + manager.Properties["sn"][0].ToString());
            }
        }
    }
}
